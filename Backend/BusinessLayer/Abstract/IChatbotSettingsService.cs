using CV.EntityLayer.Entities;
using DtoLayer.ChatbotSettingsDtos;

namespace BusinessLayer.Abstract;

public interface IChatbotSettingsService:IGenericService<ChatbotSettings,ChatbotSettingsDto,CreateChatbotSettingsDto,UpdateChatbotSettingsDto>
{
}
