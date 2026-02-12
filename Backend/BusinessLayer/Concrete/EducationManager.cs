using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.EducationDtos;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class EducationManager : IEducationService
{
    private readonly IEducationDal _educationDal;
    private readonly IMapper _mapper;

    public EducationManager(IEducationDal educationDal, IMapper mapper)
    {
        _educationDal = educationDal;
        _mapper = mapper;
    }

    public async Task<EducationDto> AddAsync(CreateEducationDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Education>(dto);
        await _educationDal.AddAsync(entity,cancellationToken);
        await _educationDal.SaveAsync( cancellationToken);
        return _mapper.Map<EducationDto>(entity);
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.GetByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity != null)
        {
            await _educationDal.DeleteAsync(entity,cancellationToken);
            await _educationDal.SaveAsync(cancellationToken);
        }

    }

    public async Task<List<EducationDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.GetAllAdminAsync(tracking: false, cancellationToken:cancellationToken);
        return _mapper.Map<List<EducationDto>>(entity.OrderBy(x=>x.DisplayOrder));
    }

    public async Task<List<EducationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.GetAllAsync(tracking: false,cancellationToken:cancellationToken);
        return _mapper.Map<List<EducationDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<EducationDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.GetByIdAsync(guid, tracking: false,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<EducationDto>(entity);
    }

    public async Task<EducationDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.RestoreDeleteByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;

        await _educationDal.UpdateAsync(entity, cancellationToken);
        await _educationDal.SaveAsync(cancellationToken);

        return _mapper.Map<EducationDto>(entity);
    }

    public async Task<EducationDto?> UpdateAsync(Guid guid, UpdateEducationDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _educationDal.GetByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _educationDal.UpdateAsync(entity, cancellationToken);
        await _educationDal.SaveAsync(cancellationToken);
        return _mapper.Map<EducationDto>(entity);
    }
}
