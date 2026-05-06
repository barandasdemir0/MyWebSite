using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.LogDtos;
using EntityLayer.Entities;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class SystemLogManager : GenericManager<SystemLog, ResultLogDto, ResultLogDto, ResultLogDto>, ISystemLogService
{
    private readonly ISystemLogDal _systemLogDal;
    public SystemLogManager(IGenericRepository<SystemLog> repository, IMapper mapper, IUnitOfWork unitOfWork, ISystemLogDal systemLogDal) : base(repository, mapper, unitOfWork)
    {
        _systemLogDal = systemLogDal;
    }

    public async Task<PagedResult<ResultLogDto>> GetAllPagedAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        // DAL'daki özel metodu çağırıyoruz (Blog'daki gibi)
        var (items, totalCount) = await _systemLogDal.GetLogsPagedAsync(query.PageNumber, query.PageSize, cancellationToken);

        // Mapped liste üzerinden ToPagedResult çağrısı
        return _mapper.Map<List<ResultLogDto>>(items).ToPagedResult(query.PageNumber, query.PageSize, totalCount);
    }
}
