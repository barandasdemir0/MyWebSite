using DtoLayer.GuestBookDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class GuestBookApiService : GenericApiService<GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto>   , IGuestBookApiService
{
    public GuestBookApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "guestbooks")
    {
    }

    public async Task ApproveAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/approve/{guid}", null);
        if (!response.IsSuccessStatusCode)
        {
            // bu satır API'den dönen hata mesajını okuyarak bir Exception fırlatır
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        // Eğer başarılı ise, işlem tamamlanır ve herhangi bir değer döndürülmez
    }

    public async Task<PagedResult<GuestBookListDto>> GetAllAdminAsync(PaginationQuery paginationQuery)
    {
        var result = await _httpClient.GetFromJsonAsync<PagedResult<GuestBookListDto>>($"{_endpoint}/admin-all?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}");
        return result ?? new PagedResult<GuestBookListDto>();

    }

    public async Task<PagedResult<GuestBookListDto>> GetAllUserAsync(PaginationQuery paginationQuery)
    {
        var result = await _httpClient.GetFromJsonAsync<PagedResult<GuestBookListDto>>($"{_endpoint}/user-all?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}");
        return result ?? new PagedResult<GuestBookListDto>();
    }

    public async Task RestoreAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/restore/{guid}", null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}
