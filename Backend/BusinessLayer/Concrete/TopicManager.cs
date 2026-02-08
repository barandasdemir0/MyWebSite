using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.TopicDtos;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class TopicManager : ITopicService
    {

        private readonly ITopicDal _topicDal;
        private readonly IMapper _mapper;

        public TopicManager(ITopicDal topicDal, IMapper mapper)
        {
            _topicDal = topicDal;
            _mapper = mapper;
        }

        public async Task<TopicDto> AddAsync(CreateTopicDto dto)
        {
            var entity = _mapper.Map<Topic>(dto);
            await _topicDal.AddAsync(entity);
            await _topicDal.SaveAsync();
            return _mapper.Map<TopicDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _topicDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _topicDal.DeleteAsync(entity);
                await _topicDal.SaveAsync();
            }

        }

        public async Task<List<TopicDto>> GetAllAdminAsync()
        {
            var entity = await _topicDal.GetAllAdminAsync(tracking: false);
            return _mapper.Map<List<TopicDto>>(entity);
        }

        public async Task<List<TopicDto>> GetAllAsync()
        {
            var entity = await _topicDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<TopicDto>>(entity);
        }

        public async Task<TopicDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _topicDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<TopicDto>(entity);
        }

        public async Task<TopicDto?> RestoreAsync(Guid guid)
        {
            var entity = await _topicDal.RestoreDeleteByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            await _topicDal.UpdateAsync(entity);
            await _topicDal.SaveAsync();

            return _mapper.Map<TopicDto>(entity);
        }

        public async Task<TopicDto?> UpdateAsync(Guid guid, UpdateTopicDto dto)
        {
            var entity = await _topicDal.GetByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _topicDal.UpdateAsync(entity);
            await _topicDal.SaveAsync();
            return _mapper.Map<TopicDto>(entity);
        }
    }
}
