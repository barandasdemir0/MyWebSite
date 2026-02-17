using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.SocialMediaDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class SocialMediaManager : GenericManager<SocialMedia,SocialMediaDto,CreateSocialMediaDto,UpdateSocialMediaDto> ,ISocialMediaService
{

    private readonly ISocialMediaDal _socialMediaDal;

    public SocialMediaManager(ISocialMediaDal socialMediaDal, IMapper mapper) : base(socialMediaDal, mapper)
    {
        _socialMediaDal = socialMediaDal;
    }

    public async Task<List<SocialMediaDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _socialMediaDal.GetAllAdminAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<SocialMediaDto>>(entity);
    }


    public async Task<SocialMediaDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var query = await _socialMediaDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
        if (query == null)
        {
            return null;
        }
        query.IsDeleted = false;
        query.DeletedAt = null;
        await _socialMediaDal.UpdateAsync(query, cancellationToken);
        await _socialMediaDal.SaveAsync(cancellationToken);
        return _mapper.Map<SocialMediaDto>(query);
    }

}
