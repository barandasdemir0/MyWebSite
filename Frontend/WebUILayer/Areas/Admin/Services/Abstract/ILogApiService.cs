using DtoLayer.LogDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface ILogApiService:IGenericApiService<ResultLogDto,ResultLogDto,ResultLogDto>
{
    Task<PagedResult<ResultLogDto>> GetResultLogsAsync(PaginationQuery query);
}
