using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.ChatbotSettingsDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class ChatbotSettingsManager : IChatbotSettingsService
{
    private readonly IChatbotSettingsDal _chatbotSettingsDal;
    private readonly IMapper _mapper;

    public ChatbotSettingsManager(IChatbotSettingsDal chatbotSettingsDal, IMapper mapper)
    {
        _chatbotSettingsDal = chatbotSettingsDal;
        _mapper = mapper;
    }

    public async Task<ChatbotSettingsDto> AddAsync(CreateChatbotSettingsDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ChatbotSettings>(dto);
        await _chatbotSettingsDal.AddAsync(entity, cancellationToken);
        await _chatbotSettingsDal.SaveAsync(cancellationToken);
        return _mapper.Map<ChatbotSettingsDto>(entity);
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _chatbotSettingsDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity != null)
        {
            await _chatbotSettingsDal.DeleteAsync(entity, cancellationToken);
            await _chatbotSettingsDal.SaveAsync(cancellationToken);
        }
    }

    public async Task<List<ChatbotSettingsDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _chatbotSettingsDal.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<ChatbotSettingsDto>>(entity);
    }

    public async Task<ChatbotSettingsDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _chatbotSettingsDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<ChatbotSettingsDto>(entity);
    }

    public async Task<ChatbotSettingsDto?> UpdateAsync(Guid guid, UpdateChatbotSettingsDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _chatbotSettingsDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _chatbotSettingsDal.UpdateAsync(entity, cancellationToken);
        await _chatbotSettingsDal.SaveAsync(cancellationToken);
        return _mapper.Map<ChatbotSettingsDto>(entity);
    }
}
