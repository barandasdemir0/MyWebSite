using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.GuestBookDtos;
using DtoLayer.ProjectDtos;
using MapsterMapper;
using SharedKernel.Exceptions;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class GuestBookManager : GenericManager<GuestBook, GuestBookDto, CreateGuestBookDto, UpdateGuestBookDto>, IGuestBookService
{
    private readonly IGuestBookDal _guestBookDal;
    private readonly INotificationDal _notificationDal;

    public GuestBookManager(IGuestBookDal guestBookDal, IMapper mapper, IUnitOfWork unitOfWork, INotificationDal notificationDal) : base(guestBookDal, mapper, unitOfWork)
    {
        _guestBookDal = guestBookDal;
        _notificationDal = notificationDal;
    }

    public override async Task<GuestBookDto> AddAsync(CreateGuestBookDto dto, CancellationToken cancellationToken = default)
    {
        var result = await base.AddAsync(dto, cancellationToken);
        await _notificationDal.AddAsync(new Notification
        {
            Type = "comment",
            Message = $"{dto.AuthorName} ziyaretçi defterine onay bekleyen yeni bir kayıt bıraktı.",
            IsRead = false,
        }, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }


    public async Task<GuestBookDto?> ApproveAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _guestBookDal.GetByIdAsync(guid, tracking: true, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsApproved = true;
        await _guestBookDal.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GuestBookDto>(entity);
    }

    public async Task<PagedResult<GuestBookDto>> GetAllAdminAsync(PaginationQuery paginationQuery,  bool? isApproved = null, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _guestBookDal.GetAdminListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize,isApproved, cancellationToken);
        return _mapper.Map<List<GuestBookDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }



    public async Task<PagedResult<GuestBookDto>> GetAllUserAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _guestBookDal.GetUserListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
        return _mapper.Map<List<GuestBookDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }



    public async Task<GuestBookDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _guestBookDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<GuestBookDto>(entity);
    }

    public async Task<List<GuestBookDto>> GetLatestAsync(int count, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(
            filter: x => x.IsApproved,
            tracking: false,
            options: new QueryOptions<GuestBook>
            {
                OrderBy = x => x.CreatedAt,
                Descending = true,
                Take = count,
            }, cancellationToken: cancellationToken);

        return _mapper.Map<List<GuestBookDto>>(entities);
    }

    public async Task<GuestBookDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _guestBookDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;

        await _guestBookDal.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GuestBookDto>(entity);
    }

    public override Task<GuestBookDto?> UpdateAsync(Guid guid, UpdateGuestBookDto dto, CancellationToken cancellationToken = default)
    {
        throw new BusinessException("Ziyaretçi Mesajları güncellenemez!");
    }
}
