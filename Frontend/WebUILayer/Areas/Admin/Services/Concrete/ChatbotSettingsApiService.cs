using DtoLayer.ChatbotSettingsDtos;
using Mapster;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class ChatbotSettingsApiService : GenericApiService<ChatbotSettingsDto, CreateChatbotSettingsDto, UpdateChatbotSettingsDto>, IChatbotSettingsApiService
{
    public ChatbotSettingsApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "chatbotsettings")
    {
    }

    public async Task<UpdateChatbotSettingsDto> GetChatbotSettingForEditAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/single");
        if (!response.IsSuccessStatusCode)
        {
            return new UpdateChatbotSettingsDto();
        }
        var query = await response.Content.ReadFromJsonAsync<ChatbotSettingsDto>();
        if (query==null)
        {
            return new UpdateChatbotSettingsDto();
        }

        return query.Adapt<UpdateChatbotSettingsDto>();
    }

    public async Task SaveChatbotSettingAsync(UpdateChatbotSettingsDto updateChatbotSettingsDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/save", updateChatbotSettingsDto);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Hata Kodu: {response.StatusCode} | Detay: {error}");
        }
    }
}
