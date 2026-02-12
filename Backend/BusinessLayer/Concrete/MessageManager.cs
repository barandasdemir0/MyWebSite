using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.MessageDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class MessageManager : IMessageService
{

    private readonly IMessageDal _messageDal;
    private readonly IMapper _mapper;

    public MessageManager(IMessageDal messageDal, IMapper mapper)
    {
        _messageDal = messageDal;
        _mapper = mapper;
    }

    public async Task<MessageListDto> AddAsync(CreateMessageDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Message>(dto);
        await _messageDal.AddAsync(entity, cancellationToken);
        await _messageDal.SaveAsync(cancellationToken);
        return _mapper.Map<MessageListDto>(entity);
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity != null)
        {
            await _messageDal.DeleteAsync(entity, cancellationToken);
            await _messageDal.SaveAsync(cancellationToken);
        }
    }

    public async Task<List<MessageListDto>> GetAllAdminAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetAllAdminAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<MessageListDto>>(entity);
    }

    public async Task<List<MessageListDto>> GetAllAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<MessageListDto>>(entity);
    }

    public async Task<MessageListDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<MessageListDto>(entity);
    }

    public async Task<MessageDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<MessageDto>(entity);
    }

    public async Task<MessageListDto?> UpdateAsync(Guid guid, UpdateMessageDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Mesajlar güncellenemez!");
    }
}
