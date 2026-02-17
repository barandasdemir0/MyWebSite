using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
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
