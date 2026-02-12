using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ExperienceDtos;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class ExperienceManager : IExperienceService
    {

        private readonly IExperienceDal _experienceDal;
        private readonly IMapper _mapper;

        public ExperienceManager(IExperienceDal experienceDal, IMapper mapper)
        {
            _experienceDal = experienceDal;
            _mapper = mapper;
        }

        public async Task<ExperienceDto> AddAsync(CreateExperienceDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Experience>(dto);
            await _experienceDal.AddAsync(entity,cancellationToken);
            await _experienceDal.SaveAsync(cancellationToken);
            return _mapper.Map<ExperienceDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _experienceDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
            if (entity != null)
            {
                await _experienceDal.DeleteAsync(entity, cancellationToken);
                await _experienceDal.SaveAsync(cancellationToken);
            }
        }

        public async Task<List<ExperienceDto>> GetAllAdminAsync( CancellationToken cancellationToken = default)
        {
            var entity = await _experienceDal.GetAllAdminAsync(tracking: false,cancellationToken:cancellationToken);
            return _mapper.Map<List<ExperienceDto>>(entity.OrderBy(x => x.DisplayOrder));
        }

        public async Task<List<ExperienceDto>> GetAllAsync( CancellationToken cancellationToken = default)
        {
            var entity = await _experienceDal.GetAllAsync(tracking: false, cancellationToken:cancellationToken);
            return _mapper.Map<List<ExperienceDto>>(entity.OrderBy(x => x.DisplayOrder));
        }

        public async Task<ExperienceDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _experienceDal.GetByIdAsync(guid, tracking: false,cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ExperienceDto>(entity);
        }

        public async Task<ExperienceDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _experienceDal.RestoreDeleteByIdAsync(guid,cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            await _experienceDal.UpdateAsync(entity, cancellationToken);
            await _experienceDal.SaveAsync(cancellationToken);
            return _mapper.Map<ExperienceDto>(entity);
        }

        public async Task<ExperienceDto?> UpdateAsync(Guid guid, UpdateExperienceDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _experienceDal.GetByIdAsync(guid,cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _experienceDal.UpdateAsync(entity, cancellationToken);
            await _experienceDal.SaveAsync(cancellationToken);
            return _mapper.Map<ExperienceDto>(entity);
        }
    }
}
