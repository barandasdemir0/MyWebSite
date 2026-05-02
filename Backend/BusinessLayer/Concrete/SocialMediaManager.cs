using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.SocialMediaDtos;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class SocialMediaManager : GenericManager<SocialMedia,SocialMediaDto,CreateSocialMediaDto,UpdateSocialMediaDto> ,ISocialMediaService
{

    private readonly ISocialMediaDal _socialMediaDal;

    public SocialMediaManager(ISocialMediaDal socialMediaDal, IMapper mapper, IUnitOfWork unitOfWork) : base(socialMediaDal, mapper, unitOfWork)
    {
        _socialMediaDal = socialMediaDal;
    }

    public async Task<List<SocialMediaDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _socialMediaDal.GetAllAsync(tracking: false, options: new QueryOptions<SocialMedia>
        {
            IgnoreQueryFilters = true // Sihir burada!
        }, cancellationToken: cancellationToken);
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<SocialMediaDto>(query);
    }

}
