using DtoLayer.MessageDtos;
using SharedKernel.Enums;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class MessageApiService : GenericApiService<MessageDto, CreateMessageDto, UpdateMessageDto>, IMessageApiService
{
    public MessageApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "messages")
    {
    }

    public async Task<PagedResult<MessageDto>> GetAllAdminAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/user-all");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<MessageDto>>(url);
        if (result==null)
        {
            return new PagedResult<MessageDto>();
        }
        return result;
    }

    public async Task<PagedResult<MessageDto>> GetByFolderAsync(MessageFolder folder, PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/folder/{folder}");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<MessageDto>>(url);
        if (result==null)
        {
            return new PagedResult<MessageDto>();
        }
        return result;
    }

    public async Task<MessageDto?> GetDetailAsync(Guid guid)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/detail/{guid}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        return await response.Content.ReadFromJsonAsync<MessageDto>();

    }

    public async Task<Dictionary<string, int>> GetFolderCountsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>($"{_endpoint}/folder-counts");
        if (result==null)
        {
            return new Dictionary<string, int>();
        }
        return result;
    }

    public async Task<List<MessageDto>> GetLatestAsync(int count)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<MessageDto>();
        }
        var result = await response.Content.ReadFromJsonAsync<List<MessageDto>>();
        if (result==null)
        {
            return new List<MessageDto>();
        }
        return result;
    }

    public async Task<PagedResult<MessageDto>> GetReadAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/read");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<MessageDto>>(url);
        if (result==null)
        {
            return new PagedResult<MessageDto>();
        }
        return result;
    }

    public async Task<PagedResult<MessageDto>> GetStarredAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/starred");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<MessageDto>>(url);
        if (result==null)
        {
            return new PagedResult<MessageDto>();
        }
        return result;
    }

    public async Task MarkAsReadAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/mark-read/{guid}", null);
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

    public async Task ToggleStarAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/toggle-star/{guid}", null);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
