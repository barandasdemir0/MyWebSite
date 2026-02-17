using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IGenericService<TEntity, TListDto, TCreateDto, TUpdateDto> : ICrudService<TListDto, TCreateDto, TUpdateDto> 
    where TEntity : class
    where TListDto : class,IHasId

{

}
