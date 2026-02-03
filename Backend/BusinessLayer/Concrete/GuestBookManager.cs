using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.GuestBookDtos;
using DtoLayer.TopicDtos;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<GuestBookListDto> AddAsync(CreateGuestBookDto dto)
        {
            var entity = _mapper.Map<GuestBook>(dto);
            await _guestBookDal.AddAsync(entity);
            await _guestBookDal.SaveAsync();
            return _mapper.Map<GuestBookListDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _guestBookDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _guestBookDal.DeleteAsync(entity);
                await _guestBookDal.SaveAsync();
            }
        }

        public async Task<List<GuestBookListDto>> GetAllAdminAsync()
        {
            var entity = await _guestBookDal.GetAllAdminAsync(tracking: false);
            return _mapper.Map<List<GuestBookListDto>>(entity);
        }

        public async Task<List<GuestBookListDto>> GetAllAsync()
        {
            var entity = await _guestBookDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<GuestBookListDto>>(entity);
        }

        public async Task<GuestBookListDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _guestBookDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<GuestBookListDto>(entity);
        }

        public async Task<GuestBookDto?> GetDetailsByIdAsync(Guid guid)
        {
            var entity = await _guestBookDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<GuestBookDto>(entity);
        }

        public async Task<GuestBookDto?> RestoreAsync(Guid guid)
        {
            var entity = await _guestBookDal.RestoreDeleteByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            await _guestBookDal.UpdateAsync(entity);
            await _guestBookDal.SaveAsync();

            return _mapper.Map<GuestBookDto>(entity);
        }

        public Task<GuestBookListDto?> UpdateAsync(Guid guid, UpdateGuestBookDto dto)
        {
            throw new NotSupportedException("Ziyaretçi Mesajları güncellenemez!");
        }
    }
}
