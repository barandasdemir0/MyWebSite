using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ContactDtos;
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

        public async Task<ContactDto> AddAsync(CreateContactDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Contact>(dto);
            await _contactDal.AddAsync(entity,cancellationToken);
            await _contactDal.SaveAsync(cancellationToken);
            return _mapper.Map<ContactDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _contactDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
            if (entity != null)
            {
                await _contactDal.DeleteAsync(entity, cancellationToken);
                await _contactDal.SaveAsync(cancellationToken);
            }
        }

        public async Task<List<ContactDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _contactDal.GetAllAsync(tracking: false,cancellationToken:cancellationToken);
            return _mapper.Map<List<ContactDto>>(entity);
        }

        public async Task<ContactDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _contactDal.GetByIdAsync(guid, tracking: false,cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ContactDto>(entity);
        }

        public async Task<ContactDto?> UpdateAsync(Guid guid, UpdateContactDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _contactDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _contactDal.UpdateAsync(entity, cancellationToken);
            await _contactDal.SaveAsync(cancellationToken);
            return _mapper.Map<ContactDto>(entity);
        }
    }
}
