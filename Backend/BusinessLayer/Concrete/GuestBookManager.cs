using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.GuestBookDtos;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class GuestBookManager : GenericManager<GuestBook,GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto> ,IGuestBookService
{
    private readonly IGuestBookDal _guestBookDal;

    public GuestBookManager(IGuestBookDal guestBookDal, IMapper mapper) : base(guestBookDal, mapper)
    {
        _guestBookDal = guestBookDal;
    }

  
    public async Task<PagedResult<GuestBookListDto>> GetAllAdminAsync(PaginationQuery paginationQuery , CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _guestBookDal.GetAdminListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
        return _mapper.Map<List<GuestBookListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }

  

    public async Task<PagedResult<GuestBookListDto>> GetAllUserAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _guestBookDal.GetUserListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
        return _mapper.Map<List<GuestBookListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }

 

    public async Task<GuestBookDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _guestBookDal.GetByIdAsync(guid, tracking: false,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<GuestBookDto>(entity);
    }

    public async Task<GuestBookDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _guestBookDal.RestoreDeleteByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;

        await _guestBookDal.UpdateAsync(entity, cancellationToken);
        await _guestBookDal.SaveAsync(cancellationToken);

        return _mapper.Map<GuestBookDto>(entity);
    }

    public override Task<GuestBookListDto?> UpdateAsync(Guid guid, UpdateGuestBookDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Ziyaretçi Mesajları güncellenemez!");
    }
}
