using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.JobSkillCategoryDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class JobSkillCategoryManager : IJobSkillCategoryService
{
    private readonly IJobSkillCategoryDal _jobSkillCategoryDal;
    private readonly IMapper _mapper;

    public JobSkillCategoryManager(IJobSkillCategoryDal jobSkillCategoryDal, IMapper mapper)
    {
        _jobSkillCategoryDal = jobSkillCategoryDal;
        _mapper = mapper;
    }

    public async Task<JobSkillCategoryDto> AddAsync(CreateJobSkillCategoryDto dto)
    {
        var query = _mapper.Map<JobSkillCategory>(dto);
        await _jobSkillCategoryDal.AddAsync(query);
        await _jobSkillCategoryDal.SaveAsync();
        return _mapper.Map<JobSkillCategoryDto>(query); //neden create değilde liste olan dto dönüyor çünkü veritabanında otomatik ıd gibi alanlar atanıyor 
    }

    public async Task DeleteAsync(Guid guid)
    {
        var entity = await _jobSkillCategoryDal.GetByIdAsync(guid);
        if (entity != null)
        {
            await _jobSkillCategoryDal.DeleteAsync(entity);
            await _jobSkillCategoryDal.SaveAsync();
        }
    }

    public async Task<List<JobSkillCategoryDto>> GetAdminAllAsync()
    {
        var entity = await _jobSkillCategoryDal.GetAllAdminAsync(tracking:false);
        return _mapper.Map<List<JobSkillCategoryDto>>(entity);
    }

    public async Task<List<JobSkillCategoryDto>> GetAllAsync()
    {
        var entity = await _jobSkillCategoryDal.GetAllAsync(tracking: false);
        return _mapper.Map<List<JobSkillCategoryDto>>(entity);
    }

    public async Task<JobSkillCategoryDto?> GetByIdAsync(Guid guid)
    {
        var entity = await _jobSkillCategoryDal.GetByIdAsync(guid,tracking:false);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<JobSkillCategoryDto>(entity);
    }

    public async Task<JobSkillCategoryDto?> RestoreAsync(Guid guid)
    {
        var query = await _jobSkillCategoryDal.RestoreDeleteByIdAsync(guid);
        if (query == null)
        {
            return null;
        }
        query.IsDeleted = false;
        query.DeletedAt = null;
        await _jobSkillCategoryDal.UpdateAsync(query);
        await _jobSkillCategoryDal.SaveAsync();
        return _mapper.Map<JobSkillCategoryDto>(query);
    }

    public async Task<JobSkillCategoryDto?> UpdateAsync(Guid guid, UpdateJobSkillCategoryDto dto)
    {
        var entity = await _jobSkillCategoryDal.GetByIdAsync(guid);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _jobSkillCategoryDal.UpdateAsync(entity);
        await _jobSkillCategoryDal.SaveAsync();
        return _mapper.Map<JobSkillCategoryDto>(entity);
    }
}
