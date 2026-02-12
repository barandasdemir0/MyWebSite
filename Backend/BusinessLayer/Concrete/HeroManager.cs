using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.HeroDtos;
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

        public async Task<HeroDto> AddAsync(CreateHeroDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Hero>(dto);
            await _heroDal.AddAsync(entity, cancellationToken);
            await _heroDal.SaveAsync(cancellationToken);
            return _mapper.Map<HeroDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _heroDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
            if (entity != null)
            {
                await _heroDal.DeleteAsync(entity, cancellationToken);
                await _heroDal.SaveAsync(cancellationToken);
            }
        }

        public async Task<List<HeroDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _heroDal.GetAllAsync(tracking: false,cancellationToken:cancellationToken);
            return _mapper.Map<List<HeroDto>>(entity);
        }

        public async Task<HeroDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _heroDal.GetByIdAsync(guid, tracking: false,cancellationToken:cancellationToken);
            if (entity==null)
            {
                return null;
            }
            return _mapper.Map<HeroDto>(entity);
        }

        public async Task<HeroDto?> UpdateAsync(Guid guid, UpdateHeroDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _heroDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _heroDal.UpdateAsync(entity, cancellationToken);
            await _heroDal.SaveAsync(cancellationToken);
            return _mapper.Map<HeroDto>(entity);
        }
    }
}
