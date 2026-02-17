using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AboutDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class AboutManager : GenericManager<About, AboutDto, CreateAboutDto, UpdateAboutDto>, IAboutService
{
    private readonly IAboutDal _aboutDal;

    public AboutManager(IAboutDal aboutDal, IMapper mapper) : base(aboutDal, mapper)
    {
        _aboutDal = aboutDal;
    }

    public async Task<AboutDto?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _aboutDal.GetSingleAsync(cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<AboutDto>(entity);
    }

    public async Task<AboutDto> SaveAsync(UpdateAboutDto update, CancellationToken cancellation = default)
    {
        var query = await _aboutDal.GetSingleAsync(cancellation);
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


}
