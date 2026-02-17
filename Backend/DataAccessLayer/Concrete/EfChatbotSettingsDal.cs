using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;

namespace DataAccessLayer.Concrete;

public class EfChatbotSettingsDal : GenericRepository<ChatbotSettings>, IChatbotSettingsDal
{
    public EfChatbotSettingsDal(AppDbContext context) : base(context)
    {
    }
}
