namespace WebUILayer.Services.Abstract;

public interface IPublicReadApiService<T>
{
    Task<List<T>> GetAllAsync();
}
