using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.JobSkillCategoryDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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

    public async Task<JobSkillCategoryDto> AddAsync(CreateJobSkillCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var query = _mapper.Map<JobSkillCategory>(dto); //dtodan entitye dönüşüm
        await _jobSkillCategoryDal.AddAsync(query,cancellationToken:cancellationToken); //veitabanına ekle
        await _jobSkillCategoryDal.SaveAsync(cancellationToken); //veritabanına kaydet
        return _mapper.Map<JobSkillCategoryDto>(query); //neden create değilde liste olan dto dönüyor çünkü veritabanında otomatik ıd gibi alanlar atanıyor  // ve entityden dtoya dönüyor
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetByIdAsync(guid,cancellationToken:cancellationToken); //veritabanında varmı
        if (entity != null)
        {
            await _jobSkillCategoryDal.DeleteAsync(entity,cancellationToken); //varsa sil
            await _jobSkillCategoryDal.SaveAsync(cancellationToken); //kaydet
        }
    }

    public async Task<List<JobSkillCategoryDto>> GetAdminAllAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetAllAdminAsync(tracking:false,includes:source=>source.Include(x=>x.JobSkills),cancellationToken:cancellationToken); //tracking yani izlemeyi kapat yetenekleride dahil et yani backend klasöründe C# ve yüzdeliği getir
        return _mapper.Map<List<JobSkillCategoryDto>>(entity); //entityi dtoya dönüştür
    }

    public async Task<List<JobSkillCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetAllAsync(tracking: false,includes:source=>source.Include(x=>x.JobSkills),cancellationToken:cancellationToken);
        return _mapper.Map<List<JobSkillCategoryDto>>(entity);
    }

    public async Task<JobSkillCategoryDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetByIdAsync(guid,tracking:false,includes:source=>source.Include(x=>x.JobSkills),cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<JobSkillCategoryDto>(entity);
    }

    public async Task<JobSkillCategoryDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var query = await _jobSkillCategoryDal.RestoreDeleteByIdAsync(guid,cancellationToken:cancellationToken); // silinen kaydı bul
        if (query == null)
        {
            return null;
        }
        query.IsDeleted = false;
        query.DeletedAt = null;
        await _jobSkillCategoryDal.UpdateAsync(query,cancellationToken);
        await _jobSkillCategoryDal.SaveAsync(cancellationToken);
        return _mapper.Map<JobSkillCategoryDto>(query);
    }

    public async Task<JobSkillCategoryDto?> UpdateAsync(Guid guid, UpdateJobSkillCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetByIdAsync(guid,cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _jobSkillCategoryDal.UpdateAsync(entity, cancellationToken);
        await _jobSkillCategoryDal.SaveAsync(cancellationToken);
        return _mapper.Map<JobSkillCategoryDto>(entity);
    }
}
