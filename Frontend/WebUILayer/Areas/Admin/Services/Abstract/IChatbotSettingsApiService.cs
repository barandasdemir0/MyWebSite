using DtoLayer.ChatbotSettingsDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IChatbotSettingsApiService
{
    Task<UpdateChatbotSettingsDto> GetChatbotSettingForEditAsync();
    Task SaveChatbotSettingAsync(UpdateChatbotSettingsDto updateChatbotSettingsDto);
}
