using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.GuestBookDto;
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

        public Task<GuestBookListDto?> UpdateAsync(UpdateGuestBookDto dto)
        {
            throw new NotSupportedException("Ziyaretçi Mesajları güncellenemez!");
        }
    }
}
