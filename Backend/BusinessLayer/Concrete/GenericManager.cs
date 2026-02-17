using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class GenericManager<TEntity, TListDto, TCreateDto, TUpdateDto> : IGenericService<TEntity, TListDto, TCreateDto, TUpdateDto> where TEntity : BaseEntity where TListDto : class
{

    protected readonly IGenericRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    public GenericManager(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<TListDto> AddAsync(TCreateDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto); //dto eğer null ise anında çıkış yap alttaki işlemleri yapma Metot burada durur repositorye girmez dbye gitmez about eklenmez 
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return _mapper.Map<TListDto>(entity);
    }

    public virtual async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(guid, cancellationToken:cancellationToken);
        if (entity != null)
        {
            await _repository.DeleteAsync(entity, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
        }
    }

    public virtual async Task<List<TListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<TListDto>>(entities);
    }

    public virtual async Task<TListDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<TListDto>(entity);
    }

    public virtual async Task<TListDto?> UpdateAsync(Guid guid, TUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity ==null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity,cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return _mapper.Map<TListDto>(entity);
    }
}
