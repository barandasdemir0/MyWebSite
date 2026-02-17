using DtoLayer.TopicDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class TopicController : Controller
{
    private readonly ITopicApiService _topicApiService;

    public TopicController(ITopicApiService topicApiService)
    {
        _topicApiService = topicApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _topicApiService.GetAllAdminAsync();
        // API'den tüm konuları çek (silinmişler dahil — Admin panel)
        return View(query);
        // Views/Admin/Topic/Index.cshtml sayfasına gönder
        // @model List<TopicDto> → View'da bu listeyi kullanacak
    }

    [HttpGet]
    public IActionResult Create() => View();
    // Boş bir form göster. API çağrısı yok.
    // Views/Admin/Topic/Create.cshtml açılır


    [HttpPost]
    public async Task<IActionResult> Create(CreateTopicDto createTopicDto)
    {
        //  Validasyon kontrolü
        if (!ModelState.IsValid)
        {
            return View(createTopicDto); // Hatalarla birlikte formu tekrar göster
        }
        try
        {
            //  API'ye gönder
            var query = await _topicApiService.AddAsync(createTopicDto);
            //  Başarılıysa listeye yönlendir
            return RedirectToAction(nameof(Index));
            // Tarayıcıyı /Admin/Topic/Index'e yönlendir
        }
        catch (Exception ex)
        {
            // API'den hata dönerse formu tekrar göster
            ModelState.AddApiError(ex); // Hata mesajını forma ekle
            return View(createTopicDto);
        }


    }
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        // Mevcut veriyi API'den çek
        var query = await _topicApiService.GetByIdAsync(id);
        if (query==null)
        {
            return NotFound();
        }
        // TopicDto → UpdateTopicDto'ya dönüştür (Mapster ile)
        return View(query.Adapt<UpdateTopicDto>()); //Neden Adapt<UpdateTopicDto>()?
                                                    //        API döndürür:   TopicDto { Id, TopicName, Description, IsDeleted, ... }
                                                    //        Form bekler:    UpdateTopicDto { Id, TopicName, Description }
                                                    //        TopicDto'da IsDeleted var ama UpdateTopicDto'da yok.
                                                    //Adapt ile sadece ortak alanlar kopyalanır.
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateTopicDto updateTopicDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateTopicDto);
        }
        try
        {
            await _topicApiService.UpdateAsync(updateTopicDto.Id, updateTopicDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateTopicDto);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await this.SafeAction
            (
            action : () => _topicApiService.DeleteAsync(id),
            successMessage: "Silme işlemi Başarılı oldu",
            ErrorMessage : "Silme İşlemi Başarısız oldu"
            );
    }
      
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        return await this.SafeAction
            (
            action: () => _topicApiService.RestoreAsync(id),
            successMessage: "Geri Alma işlemi Başarılı oldu",
            ErrorMessage: "Geri Alma İşlemi Başarısız oldu"
            );
    }

}
