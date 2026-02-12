using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete;

public class EfChatbotSettingsDal : GenericRepository<ChatbotSettings>, IChatbotSettingsDal
{
    public EfChatbotSettingsDal(AppDbContext context) : base(context)
    {
    }
}
