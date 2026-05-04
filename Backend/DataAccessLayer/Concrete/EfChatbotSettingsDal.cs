using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfChatbotSettingsDal : GenericRepository<ChatbotSettings>, IChatbotSettingsDal
{
    public EfChatbotSettingsDal(AppDbContext context) : base(context)
    {
    }

    public async Task<ChatbotSettings?> GetActiveSettingsAsync()
    {
        return await _context.ChatbotSettings.FirstOrDefaultAsync();
    }
}
