using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.MessageDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {

        private readonly IMessageDal _messageDal;
        private readonly IMapper _mapper;

        public MessageManager(IMessageDal messageDal, IMapper mapper)
        {
            _messageDal = messageDal;
            _mapper = mapper;
        }

        public async Task<MessageListDto> AddAsync(CreateMessageDto dto)
        {
            var entity = _mapper.Map<Message>(dto);
            await _messageDal.AddAsync(entity);
            await _messageDal.SaveAsync();
            return _mapper.Map<MessageListDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _messageDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _messageDal.DeleteAsync(entity);
                await _messageDal.SaveAsync();
            }
        }

        public async Task<List<MessageListDto>> GetAllAdminAsync()
        {
            var entity = await _messageDal.GetAllAdminAsync(tracking: false);
            return _mapper.Map<List<MessageListDto>>(entity);
        }

        public async Task<List<MessageListDto>> GetAllAsync()
        {
            var entity = await _messageDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<MessageListDto>>(entity);
        }

        public async Task<MessageListDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _messageDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<MessageListDto>(entity);
        }

        public async Task<MessageDto?> GetDetailsByIdAsync(Guid guid)
        {
            var entity = await _messageDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<MessageDto>(entity);
        }

        public async Task<MessageListDto?> UpdateAsync(Guid guid, UpdateMessageDto dto)
        {
            throw new NotSupportedException("Mesajlar güncellenemez!");
        }
    }
}
