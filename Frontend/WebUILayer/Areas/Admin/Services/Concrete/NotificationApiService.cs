using DtoLayer.NotificationDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class NotificationApiService : GenericApiService<NotificationDto, CreateNotificationDto, UpdateNotificationDto>, INotificationApiService
{
    public NotificationApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "notifications")
    {
    }

    public async Task<PagedResult<NotificationDto>> GetAllAdminAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/admin-all");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<NotificationDto>>(url);
        if (result == null)
        {
            return new PagedResult<NotificationDto>();
        }
        return result;
    }

    public async Task<List<NotificationDto>> GetTopUnreadAsync(int count)
    {
        var result = await _httpClient.GetFromJsonAsync<List<NotificationDto>>($"{_endpoint}/unread?count={count}");
        if (result==null)
        {
            return new List<NotificationDto>();
        }
        return result;
    }

    public async Task ReadMessageAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/read/{guid}", null);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task RestoreAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/restore/{guid}", null);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
