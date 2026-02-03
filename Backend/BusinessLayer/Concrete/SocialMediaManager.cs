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

        public async Task<SocialMediaDto> AddAsync(CreateSocialMediaDto dto)
        {
            var entity = _mapper.Map<SocialMedia>(dto);
            await _socialMediaDal.AddAsync(entity);
            await _socialMediaDal.SaveAsync();
            return _mapper.Map<SocialMediaDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _socialMediaDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _socialMediaDal.DeleteAsync(entity);
                await _socialMediaDal.SaveAsync();
            }
        }

        public async Task<List<SocialMediaDto>> GetAllAsync()
        {
            var entity = await _socialMediaDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<SocialMediaDto>>(entity);
        }

        public async Task<SocialMediaDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _socialMediaDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<SocialMediaDto>(entity);
        }

        public async Task<SocialMediaDto?> UpdateAsync(Guid guid, UpdateSocialMediaDto dto)
        {
            var entity = await _socialMediaDal.GetByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _socialMediaDal.UpdateAsync(entity);
            await _socialMediaDal.SaveAsync();
            return _mapper.Map<SocialMediaDto>(entity);
        }
    }
}
