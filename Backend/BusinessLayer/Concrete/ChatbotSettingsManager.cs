using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ChatbotSettingsDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class ChatbotSettingsManager : GenericManager<ChatbotSettings,ChatbotSettingsDto,CreateChatbotSettingsDto,UpdateChatbotSettingsDto> ,IChatbotSettingsService
{
    private readonly IChatbotSettingsDal _chatbotSettingsDal;

    public ChatbotSettingsManager(IChatbotSettingsDal chatbotSettingsDal, IMapper mapper) : base(chatbotSettingsDal, mapper)
    {
        _chatbotSettingsDal = chatbotSettingsDal;

    }

}
