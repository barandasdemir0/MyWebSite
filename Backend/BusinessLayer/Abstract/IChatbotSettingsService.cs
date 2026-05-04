using CV.EntityLayer.Entities;
using DtoLayer.ChatbotSettingsDtos;

namespace BusinessLayer.Abstract;

public interface IChatbotSettingsService:IGenericService<ChatbotSettings,ChatbotSettingsDto,CreateChatbotSettingsDto,UpdateChatbotSettingsDto>
{
    Task<ChatbotSettingsDto?> GetSingleAsync(CancellationToken cancellationToken = default);
    Task<ChatbotSettingsDto> SaveAsync(UpdateChatbotSettingsDto updateDto, CancellationToken cancellationToken = default);
}
