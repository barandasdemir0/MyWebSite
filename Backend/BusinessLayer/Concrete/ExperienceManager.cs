using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ExperienceDto;
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

        public async Task<ExperienceDto> AddAsync(CreateExperienceDto dto)
        {
            var entity = _mapper.Map<Experience>(dto);
            await _experienceDal.AddAsync(entity);
            await _experienceDal.SaveAsync();
            return _mapper.Map<ExperienceDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _experienceDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _experienceDal.DeleteAsync(entity);
                await _experienceDal.SaveAsync();
            }
        }

        public async Task<List<ExperienceDto>> GetAllAsync()
        {
            var entity = await _experienceDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<ExperienceDto>>(entity);
        }

        public async Task<ExperienceDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _experienceDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ExperienceDto>(entity);
        }

        public async Task<ExperienceDto?> RestoreAsync(Guid guid)
        {
            var entity = await _experienceDal.RestoreDeleteByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            await _experienceDal.UpdateAsync(entity);
            await _experienceDal.SaveAsync();
            return _mapper.Map<ExperienceDto>(entity);
        }

        public async Task<ExperienceDto?> UpdateAsync(UpdateExperienceDto dto)
        {
            var entity = await _experienceDal.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _experienceDal.UpdateAsync(entity);
            await _experienceDal.SaveAsync();
            return _mapper.Map<ExperienceDto>(entity);
        }
    }
}
