using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.JobSkillsDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class JobSkillManager : IJobSkillService
{
    private readonly IJobSkillDal _jobSkillDal;
    private readonly IMapper _mapper;

    public JobSkillManager(IJobSkillDal jobSkillDal, IMapper mapper)
    {
        _jobSkillDal = jobSkillDal;
        _mapper = mapper;
    }

    public async Task<JobSkillDto> AddAsync(CreateJobSkillDto dto)
    {
        var query = _mapper.Map<JobSkill>(dto);
        await _jobSkillDal.AddAsync(query);
        await _jobSkillDal.SaveAsync();
        return _mapper.Map<JobSkillDto>(query);
    }

    public async Task DeleteAsync(Guid guid)
    {
        var query = await _jobSkillDal.GetByIdAsync(guid);
        if (query != null)
        {
            await _jobSkillDal.DeleteAsync(query);
            await _jobSkillDal.SaveAsync();
        }
    }

    public async Task<List<JobSkillDto>> GetAdminAllAsync()
    {
        var entity = await _jobSkillDal.GetAllAdminAsync(tracking: false,includes:source=>source.Include(x=>x.JobSkillCategory));
        return _mapper.Map<List<JobSkillDto>>(entity);
    }

    public async Task<List<JobSkillDto>> GetAllAsync()
    {
        var entity = await _jobSkillDal.GetAllAsync(tracking: false,includes:source=>source.Include(x=>x.JobSkillCategory));
        return _mapper.Map<List<JobSkillDto>>(entity);
    }

    public async Task<JobSkillDto?> GetByIdAsync(Guid guid)
    {
        var entity = await _jobSkillDal.GetByIdAsync(guid, tracking: false,includes:source=>source.Include(x=>x.JobSkillCategory));
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<JobSkillDto>(entity);
    }

    public async Task<JobSkillDto?> RestoreAsync(Guid guid)
    {
        var query = await _jobSkillDal.RestoreDeleteByIdAsync(guid);
        if (query == null)
        {
            return null;
        }
        query.IsDeleted = false;
        query.DeletedAt = null;

        await _jobSkillDal.UpdateAsync(query);
        await _jobSkillDal.SaveAsync();
        return _mapper.Map<JobSkillDto>(query);
    }

    public async Task<JobSkillDto?> UpdateAsync(Guid guid, UpdateJobSkillDto dto)
    {
        var query = await _jobSkillDal.GetByIdAsync(guid);
        if (query == null)
        {
            return null;
        }
        _mapper.Map(dto, query);
        await _jobSkillDal.UpdateAsync(query);
        await _jobSkillDal.SaveAsync();
        return _mapper.Map<JobSkillDto>(query);
    }
}
