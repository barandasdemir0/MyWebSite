using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.HeroDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class HeroManager : IHeroService
    {
        private readonly IHeroDal _heroDal;
        private readonly IMapper _mapper;

        public HeroManager(IHeroDal heroDal, IMapper mapper)
        {
            _heroDal = heroDal;
            _mapper = mapper;
        }

        public async Task<HeroDto> AddAsync(CreateHeroDto dto)
        {
            var entity = _mapper.Map<Hero>(dto);
            await _heroDal.AddAsync(entity);
            await _heroDal.SaveAsync();
            return _mapper.Map<HeroDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _heroDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _heroDal.DeleteAsync(entity);
                await _heroDal.SaveAsync();
            }
        }

        public async Task<List<HeroDto>> GetAllAsync()
        {
            var entity = await _heroDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<HeroDto>>(entity);
        }

        public async Task<HeroDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _heroDal.GetByIdAsync(guid, tracking: false);
            if (entity==null)
            {
                return null;
            }
            return _mapper.Map<HeroDto>(entity);
        }

        public async Task<HeroDto?> UpdateAsync(UpdateHeroDto dto)
        {
            var entity = await _heroDal.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _heroDal.UpdateAsync(entity);
            await _heroDal.SaveAsync();
            return _mapper.Map<HeroDto>(entity);
        }
    }
}
