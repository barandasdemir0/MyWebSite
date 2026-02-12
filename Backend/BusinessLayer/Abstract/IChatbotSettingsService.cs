using DtoLayer.ChatbotSettingsDtos;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IChatbotSettingsService:IGenericService<ChatbotSettings,ChatbotSettingsDto,CreateChatbotSettingsDto,UpdateChatbotSettingsDto>
    {
    }
}
