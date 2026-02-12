using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.SocialMediaDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class SocialMediaManager : ISocialMediaService
    {

        private readonly ISocialMediaDal _socialMediaDal;
        private readonly IMapper _mapper;

        public SocialMediaManager(ISocialMediaDal socialMediaDal, IMapper mapper)
        {
            _socialMediaDal = socialMediaDal;
            _mapper = mapper;
        }

        public async Task<SocialMediaDto> AddAsync(CreateSocialMediaDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<SocialMedia>(dto);
            await _socialMediaDal.AddAsync(entity, cancellationToken);
            await _socialMediaDal.SaveAsync(cancellationToken);
            return _mapper.Map<SocialMediaDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _socialMediaDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity != null)
            {
                await _socialMediaDal.DeleteAsync(entity, cancellationToken);
                await _socialMediaDal.SaveAsync(cancellationToken);
            }
        }

        public async Task<List<SocialMediaDto>> GetAllAdminAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _socialMediaDal.GetAllAdminAsync(tracking: false, cancellationToken: cancellationToken);
            return _mapper.Map<List<SocialMediaDto>>(entity);
        }

        public async Task<List<SocialMediaDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entity = await _socialMediaDal.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
            return _mapper.Map<List<SocialMediaDto>>(entity);
        }

        public async Task<SocialMediaDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _socialMediaDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<SocialMediaDto>(entity);
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

        public async Task<SocialMediaDto?> UpdateAsync(Guid guid, UpdateSocialMediaDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _socialMediaDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _socialMediaDal.UpdateAsync(entity, cancellationToken);
            await _socialMediaDal.SaveAsync(cancellationToken);
            return _mapper.Map<SocialMediaDto>(entity);
        }
    }
}
