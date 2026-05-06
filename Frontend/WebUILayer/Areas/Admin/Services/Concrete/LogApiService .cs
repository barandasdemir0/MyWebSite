using DtoLayer.LogDtos;
using DtoLayer.NotificationDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class LogApiService : GenericApiService<ResultLogDto, ResultLogDto, ResultLogDto>, ILogApiService
{
    public LogApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "logs")
    {
    }

    public async Task<PagedResult<ResultLogDto>> GetResultLogsAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/logs");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<ResultLogDto>>(url);
        if (result == null)
        {
            return new PagedResult<ResultLogDto>();
        }
        return result;
    }
}
