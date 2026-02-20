using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.GithubRepoDtos;
using MapsterMapper;
using SharedKernel.Shared;
using System.Net.Http.Json;

namespace BusinessLayer.Concrete;

public class GithubRepoManager : GenericManager<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto> , IGithubRepoService
{

    private readonly IGithubRepoDal _githubRepoDal;
    private readonly IHttpClientFactory _httpClientFactory;

    public GithubRepoManager(IGithubRepoDal githubRepoDal, IMapper mapper, IHttpClientFactory httpClientFactory) : base(githubRepoDal, mapper)
    {
        _githubRepoDal = githubRepoDal;
        _httpClientFactory = httpClientFactory;
    }

    public override async Task<List<GithubRepoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _githubRepoDal.GetAllAsync(filter:x=>x.IsVisible,tracking: false, cancellationToken:cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<PagedResult<GithubRepoDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _githubRepoDal.GetUserListPagesAsync(query.PageNumber, query.PageSize, cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(items).ToPagedResult(query.PageNumber, query.PageSize, totalCount);
    }

    public override async Task<GithubRepoDto?> UpdateAsync(Guid guid, UpdateGithubRepoDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.IsVisible)
        {
            var visibleCount = (await _repository.GetAllAsync(
                filter: x => x.IsVisible && x.Id != guid,
                tracking: false,
                cancellationToken: cancellationToken
                )).Count;
            if (visibleCount>=4)
            {
                throw new InvalidOperationException("En fazla 4 repo görünür yapılabilir!");
            }
        }
        return await base.UpdateAsync(guid, dto, cancellationToken);
    }

    // GitHub API'den repoları çek (sayfalı)
    public async Task<PagedResult<GithubApiRepoDto>> FetchFromGithubAsync(PaginationQuery query, string username, CancellationToken cancellationToken = default)
    {

        var allRepos = await FetchAllFromGithubAsync(username, cancellationToken); //adını al 
        var totalCount = allRepos.Count; //tüm repoları say ve totalcounta aktar
        var items = allRepos
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        return items.ToPagedResult(query.PageNumber, query.PageSize, totalCount); //pagelama işlemi yaptık
    }


    // Seçilenleri kaydet
    public async Task<List<GithubRepoDto>> SyncSelectedAsync(string username, List<string> repoNames, CancellationToken cancellationToken = default)
    {
        var allExisting = await _repository.GetAllAsync(tracking: true, cancellationToken: cancellationToken); //veritabanından hepsini çek
        foreach (var r in allExisting) // bunları döngüye al
        {
            r.IsVisible = false; // hepsini visible yap
            r.DisplayOrder = 0; //seçilenleri aktif yap
            await _repository.UpdateAsync(r, cancellationToken);
        }
        var githubRepos = await FetchAllFromGithubAsync(username, cancellationToken); // aşağıdaki metot çalışıyor 

        var selected = githubRepos // githubdan gelen tüm repolar 
            .Where(r => repoNames.Contains(r.RepoName, StringComparer.OrdinalIgnoreCase))// büyük küçük harf duyarsızlaştırma yapılıyor
            .ToList();
        int order = 1; //adminin seçtiği projeleri sıralı göstereceksin bu değişken her repo için artacak
        foreach (var repo in selected) // seçilen her repo işlem 
        {
            var existing = await _repository.GetAsync(x => x.RepoUrl == repo.RepoUrl, cancellationToken: cancellationToken); 
            // dbde varmı kontrolü dbde varsa tekrar dbye alma
            if (existing != null) //eğer dbde varsa
            {
                _mapper.Map(repo, existing); //bunları maple
                existing.Description ??= string.Empty; // description ve language boş gelebilir o halde hata verme
                existing.Language ??= string.Empty;
                existing.IsVisible = true;
                existing.DisplayOrder = order++; //repo sırasını ayarla
                await _repository.UpdateAsync(existing, cancellationToken); // update al
            }
            else
            {
                var entity = _mapper.Map<GithubRepo>(repo); //dtodan entity dönüşümünü yap
                entity.Description ??= string.Empty; // dönüşümleri hallet
                entity.Language ??= string.Empty;
                entity.IsVisible = true; // görünür yap
                await _repository.AddAsync(entity, cancellationToken); // ekleme işlemini yap
            }
        }
        await _repository.SaveAsync(cancellationToken); // güncelleme veya ekleme her ne yaparsan bunu veritabanına ekle
        return await GetAllAsync(cancellationToken); // hepsini listele
    }



    // Görünürlük toggle — tüm mantık burada
    public async Task<GithubRepoDto?> ToggleVisibilityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);// id ile kayıt aranıyor
        if (entity == null) return null; // eğer repo yoksa null dön 
        if (!entity.IsVisible) // dbye git visible false olanlarda işlem yap 
        {
            var visibleCount = (await _repository.GetAllAsync( 
                filter: x => x.IsVisible,
                tracking: false,
                cancellationToken: cancellationToken)).Count; //dbye git visible olanların sayısı al
            if (visibleCount >= 4) // 4den fazla ise
                throw new InvalidOperationException("En fazla 4 repo görünür yapılabilir!"); // 4den fazladır yaz
            entity.DisplayOrder = visibleCount + 1; //görünür yapılacaksa sıra veriliyor
        }
        else
        {
            entity.DisplayOrder = 0; // repo görünür ise şimdi gizlenecek 
        }
        entity.IsVisible = !entity.IsVisible; // eğer true ise false false ise true olur
        await _repository.UpdateAsync(entity, cancellationToken); // sonra bunu update yap
        await _repository.SaveAsync(cancellationToken); // sonra bunu güncelle veritabanında
        return _mapper.Map<GithubRepoDto>(entity); //ve mapleme işlemi yap 
    }


    // GitHub API çağrısı (private helper)
    private async Task<List<GithubApiRepoDto>> FetchAllFromGithubAsync(string username, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("GithubApi"); // githuba bağlanmak için bağlantı adresi oluşturur

        var allRepos = new List<GithubApiRepoDto>(); //githubdan gelen her sayfadaki repılar buraya eklenecek 

        int page = 1; //github pagination 1 den başlar

        while (true) //sonsuz döngüye sokuyoruz burada ama sokma nedenimiz repo gelmeyene kadar 
        {
            var repos = await client.GetFromJsonAsync<List<GithubApiRepoDto>>(
                $"users/{username}/repos?per_page=100&page={page}&sort=updated", cancellationToken
                );
            //githubdan sayfa çekme github en fazla 100 döndürür per_page = 100   page hangi sayfa çekilecek en son güncellenenler en üstte
            if (repos == null || repos.Count == 0) //github boş liste döndürdüyse yani artık repo kalmadıysa döngü durur
                break;
            allRepos.AddRange(repos);//gelen repoları listeye ekle addrange toplu ekleme yapar
            page++; //buradaki ilede bir sonraki request atılır yani github 100 repoyu getirir page 1 de page 2 ile 200e kadar getirir
        }
        return allRepos.Where(r => !r.Fork).ToList(); //fork olanları çıkar yani fork = bizim yazmadığımız kendi githubumuza kopyaladığımız dosyalardır

      
    }
}
