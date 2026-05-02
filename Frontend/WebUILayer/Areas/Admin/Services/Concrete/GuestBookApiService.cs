using DtoLayer.GuestBookDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class GuestBookApiService : GenericApiService<GuestBookDto, CreateGuestBookDto,UpdateGuestBookDto>   , IGuestBookApiService
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

    public async Task<PagedResult<GuestBookDto>> GetAllAdminAsync(PaginationQuery paginationQuery, bool? isApproved = null)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/admin-all");
        if (isApproved.HasValue)
        {
            url += $"&isApproved={isApproved.Value}";
        }
        var result = await _httpClient.GetFromJsonAsync<PagedResult<GuestBookDto>>(url);
        return result ?? new PagedResult<GuestBookDto>();

    }

    public async Task<PagedResult<GuestBookDto>> GetAllUserAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/user-all");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<GuestBookDto>>(url);
        return result ?? new PagedResult<GuestBookDto>();
    }

    public async Task<List<GuestBookDto>> GetLatestAsync(int count)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<GuestBookDto>();
        }
        var result = await response.Content.ReadFromJsonAsync<List<GuestBookDto>>();
        if (result==null)
        {
            return new List<GuestBookDto>();
        }
        return result;
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
