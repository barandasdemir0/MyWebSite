using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AboutDtos;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class AboutManager : IAboutService
{
    private readonly IAboutDal _repository;
    private readonly IMapper _mapper;
    public AboutManager(IAboutDal repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AboutDto> AddAsync(CreateAboutDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<About>(dto); // Create -> Entity yani elimdeki dto nesnesini al about entitysine çevir
        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return _mapper.Map<AboutDto>(entity); // Entity -> Read (ID var artık)  yani elimdeki aentity nesnesini al onu aboutdto tipine çevir 
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity != null)
        {
            await _repository.DeleteAsync(entity, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
        }

    }

    public async Task<List<AboutDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<AboutDto>>(entity);
    }

    public async Task<AboutDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<AboutDto>(entity);
    }

    public async Task<AboutDto?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetSingleAsync(cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<AboutDto>(entity);
    }

    public async Task<AboutDto> SaveAsync(UpdateAboutDto update, CancellationToken cancellation = default)
    {
        var query = await _repository.GetSingleAsync(cancellation);
        if (query == null)
        {
            query = _mapper.Map<About>(update);
            await _repository.AddAsync(query, cancellation);
        }
        else
        {
            _mapper.Map(update, query);
            await _repository.UpdateAsync(query, cancellation);
        }
        await _repository.SaveAsync(cancellation);
        return _mapper.Map<AboutDto>(query);
    }

    public async Task<AboutDto?> UpdateAsync(Guid guid, UpdateAboutDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return _mapper.Map<AboutDto>(entity);
    }

    #region  eski kodlar

    //public async Task<AboutDto> UpdateAsync(UpdateAboutDto dto)
    //{
    //    var entity = await _repository.GetByIdAsync(dto.Id);
    //    if (entity==null)
    //    {
    //        return null;
    //    }
    //    _mapper.Map(dto, entity);
    //    await _repository.UpdateAsync(entity);
    //    await _repository.SaveAsync();
    //    return _mapper.Map<AboutDto>(entity);
    //}

    //eski yapı aşşağıda


    //public async Task<AboutDto> AddAsync(AboutDto dto)
    //{
    //    var entity = _mapper.Map<About>(dto);
    //    await _repository.AddAsync(entity);
    //    await _repository.SaveAsync();
    //    return _mapper.Map<AboutDto>(entity);

    //}

    //public async Task DeleteAsync(Guid guid)
    //{
    //    var entity = await _repository.GetByIdAsync(guid);
    //    if (entity!=null)
    //    {
    //        await _repository.DeleteAsync(entity);
    //        await _repository.SaveAsync();
    //    }
    //}

    //public async Task<List<AboutDto>> GetAllAsync()
    //{
    //    var entity = await _repository.GetAllAsync(tracking: false);
    //    return _mapper.Map<List<AboutDto>>(entity);
    //}

    //public async Task<AboutDto?> GetByIdAsync(Guid guid)
    //{
    //    var entity = await _repository.GetByIdAsync(guid,tracking: false);
    //    if (entity == null)
    //    {
    //        return null;
    //    }
    //    return _mapper.Map<AboutDto>(entity);

    //}

    //public async Task<AboutDto> UpdateAsync(AboutDto dto)
    //{
    //    var entity =  _mapper.Map<About>(dto);
    //    await _repository.UpdateAsync(entity);
    //    await _repository.SaveAsync();
    //    return _mapper.Map<AboutDto>(entity);

    //}

    #endregion
}
