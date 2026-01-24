using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AboutDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class AboutManager : IAboutService
{
    private readonly IGenericRepository<About> _repository;
    private readonly IMapper _mapper;
    public AboutManager(IGenericRepository<About> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<AboutDto> AddAsync(AboutDto dto)
    {
        var entity = _mapper.Map<About>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveAsync();
        return _mapper.Map<AboutDto>(entity);

    }

    public async Task DeleteAsync(Guid guid)
    {
        var entity = await _repository.GetByIdAsync(guid);
        if (entity!=null)
        {
            await _repository.DeleteAsync(entity);
            await _repository.SaveAsync();
        }
    }

    public async Task<List<AboutDto>> GetAllAsync()
    {
        var entity = await _repository.GetAllAsync(tracking: false);
        return _mapper.Map<List<AboutDto>>(entity);
    }

    public async Task<AboutDto?> GetByIdAsync(Guid guid)
    {
        var entity = await _repository.GetByIdAsync(guid,tracking: false);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<AboutDto>(entity);

    }

    public async Task<AboutDto> UpdateAsync(AboutDto dto)
    {
        var entity =  _mapper.Map<About>(dto);
        await _repository.UpdateAsync(entity);
        await _repository.SaveAsync();
        return _mapper.Map<AboutDto>(entity);

    }
}
