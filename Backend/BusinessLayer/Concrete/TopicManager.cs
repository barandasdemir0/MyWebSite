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

        public async Task<TopicDto> AddAsync(CreateTopicDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Topic>(dto);
            await _topicDal.AddAsync(entity, cancellationToken);
            await _topicDal.SaveAsync(cancellationToken);
            return _mapper.Map<TopicDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _topicDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity != null)
            {
                await _topicDal.DeleteAsync(entity, cancellationToken);
                await _topicDal.SaveAsync(cancellationToken);
            }

        }

        public async Task<List<TopicDto>> GetAllAdminAsync( CancellationToken cancellationToken = default)
        {
            var entity = await _topicDal.GetAllAdminAsync(tracking: false, cancellationToken: cancellationToken);
            return _mapper.Map<List<TopicDto>>(entity);
        }

        public async Task<List<TopicDto>> GetAllAsync( CancellationToken cancellationToken = default)
        {
            var entity = await _topicDal.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
            return _mapper.Map<List<TopicDto>>(entity);
        }

        public async Task<TopicDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _topicDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<TopicDto>(entity);
        }

        public async Task<TopicDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _topicDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            await _topicDal.UpdateAsync(entity, cancellationToken: cancellationToken);
            await _topicDal.SaveAsync(cancellationToken);

            return _mapper.Map<TopicDto>(entity);
        }

        public async Task<TopicDto?> UpdateAsync(Guid guid, UpdateTopicDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _topicDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _topicDal.UpdateAsync(entity, cancellationToken: cancellationToken);
            await _topicDal.SaveAsync(cancellationToken);
            return _mapper.Map<TopicDto>(entity);
        }
    }
}
