using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfChatbotCacheDal : GenericRepository<ChatbotCache>, IChatbotCacheDal
{
    public EfChatbotCacheDal(AppDbContext context) : base(context)
    {
    }

    public async Task<ChatbotCache?> GetAnswerByQuestionAsync(string question, string url)
    {
        var trimmed = question.Trim().ToLowerInvariant();
        var trimmedUrl = url.Trim().ToLowerInvariant();
        return await _context.ChatbotCaches.FirstOrDefaultAsync(x => x.UserQuestion == trimmed && x.CurrentUrl == trimmedUrl );
    }
}
