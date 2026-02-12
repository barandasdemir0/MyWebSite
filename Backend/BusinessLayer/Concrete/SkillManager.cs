using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.ProjectDtos;
using DtoLayer.SkillDtos;
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

        public async Task<SkillDto> AddAsync(CreateSkillDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Skill>(dto);
            await _skillDal.AddAsync(entity, cancellationToken);
            await _skillDal.SaveAsync(cancellationToken);
            return _mapper.Map<SkillDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _skillDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity != null)
            {
                await _skillDal.DeleteAsync(entity, cancellationToken);
                await _skillDal.SaveAsync(cancellationToken);
            }
        }

        public async Task<List<SkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _skillDal.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
            return _mapper.Map<List<SkillDto>>(entity);
        }

        public async Task<SkillDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _skillDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<SkillDto>(entity);
        }

        public async Task<SkillDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _skillDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            await _skillDal.UpdateAsync(entity, cancellationToken: cancellationToken);
            await _skillDal.SaveAsync(cancellationToken);

            return _mapper.Map<SkillDto>(entity);
        }

        public async Task<SkillDto?> UpdateAsync(Guid guid, UpdateSkillDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _skillDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _skillDal.UpdateAsync(entity, cancellationToken);
            await _skillDal.SaveAsync(cancellationToken);
            return _mapper.Map<SkillDto>(entity);
        }
    }
}
