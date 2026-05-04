using EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IChatbotCacheDal:IGenericRepository<ChatbotCache>
{
    Task<ChatbotCache?> GetAnswerByQuestionAsync(string question, string url);
}
