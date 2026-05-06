using DtoLayer.LogDtos;
using EntityLayer.Entities;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface ISystemLogService:IGenericService<SystemLog,ResultLogDto,ResultLogDto,ResultLogDto>
{
    Task<PagedResult<ResultLogDto>> GetAllPagedAsync(PaginationQuery query, CancellationToken cancellationToken = default);
}
