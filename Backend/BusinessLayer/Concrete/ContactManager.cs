using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ContactDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public sealed class ContactManager : IContactService
    {

        private readonly IContactDal _contactDal;
        private readonly IMapper _mapper;

        public ContactManager(IContactDal contactDal, IMapper mapper)
        {
            _contactDal = contactDal;
            _mapper = mapper;
        }

        public async Task<ContactDto> AddAsync(CreateContactDto dto)
        {
            var entity = _mapper.Map<Contact>(dto);
            await _contactDal.AddAsync(entity);
            await _contactDal.SaveAsync();
            return _mapper.Map<ContactDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _contactDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _contactDal.DeleteAsync(entity);
                await _contactDal.SaveAsync();
            }
        }

        public async Task<List<ContactDto>> GetAllAsync()
        {
            var entity = await _contactDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<ContactDto>>(entity);
        }

        public async Task<ContactDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _contactDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ContactDto>(entity);
        }

        public async Task<ContactDto?> UpdateAsync(UpdateContactDto dto)
        {
            var entity = await _contactDal.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _contactDal.UpdateAsync(entity);
            await _contactDal.SaveAsync();
            return _mapper.Map<ContactDto>(entity);
        }
    }
}
