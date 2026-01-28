using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.ProjectDto;
using DtoLayer.SkillDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class SkillManager : ISkillService
    {
        private readonly ISkillDal _skillDal;
        private readonly IMapper _mapper;

        public SkillManager(ISkillDal skillDal, IMapper mapper)
        {
            _skillDal = skillDal;
            _mapper = mapper;
        }

        public async Task<SkillDto> AddAsync(CreateSkillDto dto)
        {
            var entity = _mapper.Map<Skill>(dto);
            await _skillDal.AddAsync(entity);
            await _skillDal.SaveAsync();
            return _mapper.Map<SkillDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _skillDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _skillDal.DeleteAsync(entity);
                await _skillDal.SaveAsync();
            }
        }

        public async Task<List<SkillDto>> GetAllAsync()
        {
            var entity = await _skillDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<SkillDto>>(entity);
        }

        public async Task<SkillDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _skillDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<SkillDto>(entity);
        }

        public async Task<SkillDto?> RestoreAsync(Guid guid)
        {
            var entity = await _skillDal.RestoreDeleteByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            await _skillDal.UpdateAsync(entity);
            await _skillDal.SaveAsync();

            return _mapper.Map<SkillDto>(entity);
        }

        public async Task<SkillDto?> UpdateAsync(UpdateSkillDto dto)
        {
            var entity = await _skillDal.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _skillDal.UpdateAsync(entity);
            await _skillDal.SaveAsync();
            return _mapper.Map<SkillDto>(entity);
        }
    }
}
