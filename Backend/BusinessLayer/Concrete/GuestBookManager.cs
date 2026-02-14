using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.BlogpostDtos;
using DtoLayer.GuestBookDtos;
using DtoLayer.TopicDtos;
using MapsterMapper;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusinessLayer.Concrete
{
    public class GuestBookManager : IGuestBookService
    {
        private readonly IGuestBookDal _guestBookDal;
        private readonly IMapper _mapper;

        public GuestBookManager(IGuestBookDal guestBookDal, IMapper mapper)
        {
            _guestBookDal = guestBookDal;
            _mapper = mapper;
        }

        public async Task<GuestBookListDto> AddAsync(CreateGuestBookDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<GuestBook>(dto);
            await _guestBookDal.AddAsync(entity, cancellationToken);
            await _guestBookDal.SaveAsync(cancellationToken);
            return _mapper.Map<GuestBookListDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _guestBookDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
            if (entity != null)
            {
                await _guestBookDal.DeleteAsync(entity, cancellationToken);
                await _guestBookDal.SaveAsync(cancellationToken);
            }
        }

        public async Task<PagedResult<GuestBookListDto>> GetAllAdminAsync(PaginationQuery paginationQuery , CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _guestBookDal.GetAdminListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
            return _mapper.Map<List<GuestBookListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
        }

        public async Task<List<GuestBookListDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _guestBookDal.GetAllAsync(tracking: false,cancellationToken:cancellationToken);
            return _mapper.Map<List<GuestBookListDto>>(entity);
        }

        public async Task<PagedResult<GuestBookListDto>> GetAllUserAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _guestBookDal.GetUserListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
            return _mapper.Map<List<GuestBookListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
        }

        public async Task<GuestBookListDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _guestBookDal.GetByIdAsync(guid, tracking: false,cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<GuestBookListDto>(entity);
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

        public Task<GuestBookListDto?> UpdateAsync(Guid guid, UpdateGuestBookDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Ziyaretçi Mesajları güncellenemez!");
        }
    }
}
