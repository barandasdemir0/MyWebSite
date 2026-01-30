namespace WebUILayer.Services.Abstract
{
    public interface IGenericApiService<TListDto, TCreateDto, TUpdateDto> where TListDto :class
    {
        Task<List<TListDto>> GetAllAsync();
        Task<TListDto?> GetByIdAsync(Guid guid);
        Task<TListDto?> AddAsync(TCreateDto dto);
        Task<TListDto?> UpdateAsync(Guid guid,TUpdateDto dto);
        Task DeleteAsync(Guid guid);
    }
}
