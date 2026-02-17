using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.HeroDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class HeroManager :GenericManager<Hero,HeroDto,CreateHeroDto,UpdateHeroDto> ,IHeroService
{
    private readonly IHeroDal _heroDal;

    public HeroManager(IHeroDal heroDal, IMapper mapper) : base(heroDal, mapper)
    {
        _heroDal = heroDal;
    }
 
    public async Task<HeroDto?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        var query = await _heroDal.GetSingleAsync(cancellationToken);
        if (query == null)
        {
            return null;
        }
        return _mapper.Map<HeroDto>(query);
    }

    public async Task<HeroDto> SaveAsync(UpdateHeroDto updateHeroDto, CancellationToken cancellationToken = default)
    {
        var entity = await _heroDal.GetSingleAsync(cancellationToken);
        if (entity == null)
        {
            entity = _mapper.Map<Hero>(updateHeroDto);
            await _heroDal.AddAsync(entity);
        }
        else
        {
            _mapper.Map(updateHeroDto, entity);
            await _heroDal.UpdateAsync(entity, cancellationToken);
        }
        await _heroDal.SaveAsync(cancellationToken);
        return _mapper.Map<HeroDto>(entity);

    }
}
