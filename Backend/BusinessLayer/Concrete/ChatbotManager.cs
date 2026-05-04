using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Concrete;

public class ChatbotManager : IChatbotManagerService
{

    private readonly IChatbotSettingsDal _chatbotSettingsService;
    private readonly IChatbotCacheDal _chatbotCacheDal;
    private readonly IGroqApiService _groqApiService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChatbotManager> _logger;
    private readonly IPortfolioContextService _contextService;

  

    private static readonly string[] BannedInputKeywords = {
            "kural", "prompt", "ignore", "unut", "simülasyon", "test mod",
            "json", "talimat", "sistem", "whitelist", "geliştirici", "developer"
        };
    private static readonly string[] BannedOutputKeywords = {
        "<DatabaseContext>", "WHITELIST", "GÖREV:", "KATI KURALLAR"
        };

    public ChatbotManager(IChatbotSettingsDal chatbotSettingsService, IChatbotCacheDal chatbotCacheDal, IGroqApiService groqApiService, IUnitOfWork unitOfWork, ILogger<ChatbotManager> logger, IPortfolioContextService contextService)
    {
        _chatbotSettingsService = chatbotSettingsService;
        _chatbotCacheDal = chatbotCacheDal;
        _groqApiService = groqApiService;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _contextService = contextService;
    }

    public async Task<string> ProcessUserMessageAsync(string question, string currentUrl)
    {

        string normalizedQuestion = question.Trim().ToLowerInvariant().TrimEnd('?');
        string normalizedUrl = currentUrl.Split('?')[0].ToLowerInvariant().TrimEnd('/');
        if (string.IsNullOrEmpty(normalizedUrl)) normalizedUrl = "/";

        // 1. Giriş Koruması (Input Guardrail)
        if (!IsInputSafe(normalizedQuestion))
        {
            return "Size sadece Baran'ın profesyonel portföyü hakkında yardımcı olabilirim.";
        }
        // 2. Önbellek (Cache) Kontrolü
        var cached = await _chatbotCacheDal.GetAnswerByQuestionAsync(normalizedQuestion,normalizedUrl);
        if (cached != null) return cached.BotResponse;
        // 3. Ayarları Çek
        var settings = await _chatbotSettingsService.GetActiveSettingsAsync();
        if (settings == null || string.IsNullOrEmpty(settings.ApiKey)) throw new Exception("Chatbot ayarları eksik.");
        // 4. Veritabanı Bağlamını (Context) Oluştur
        string contextData = await _contextService.BuildContextAsync(currentUrl, normalizedQuestion);
        string finalPromt = $@"{settings.SystemPrompt}
        <SystemNote>Kullanıcı şu anda '{normalizedUrl}' sayfasında bulunuyor.</SystemNote>
        <DatabaseContext> {contextData} </DatabaseContext>";
        try
        {
            // 5. Groq API İsteği
            string botReply = await _groqApiService.FetchResponseFromGroqAsync(settings.ApiKey, settings.ModelName, finalPromt, question);
            // 6. Çıkış Koruması (Output Guardrail)
            if (!IsOutputSafe(botReply))
            {
                return "Size sadece Baran'ın profesyonel portföyü hakkında yardımcı olabilirim.";
            }
            // 7. Başarılı Cevabı Kaydet ve Dön
            var exists = await _chatbotCacheDal.GetAnswerByQuestionAsync(normalizedQuestion, normalizedUrl);
            if (exists == null)
            {
                await _chatbotCacheDal.AddAsync(new EntityLayer.Entities.ChatbotCache
                {
                    UserQuestion = normalizedQuestion,
                    BotResponse = botReply,
                    CurrentUrl = normalizedUrl,
                    CreatedAt = DateTime.UtcNow
                });
                await _unitOfWork.SaveChangesAsync();
            }

      
            return botReply;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chatbot API isteği sırasında hata oluştu.");
            return "Asistan şu an hizmet veremiyor. Lütfen daha sonra tekrar deneyiniz.";
        }


    }


    private bool IsInputSafe(string normalizedQuestion)
    {
        if (normalizedQuestion.Length > 250) return false;
        foreach (var banned in BannedInputKeywords)
        {
            if (normalizedQuestion.Contains(banned)) return false;
        }
        return true;
    }


    private bool IsOutputSafe(string botReply)
    {
        foreach (var banned in BannedOutputKeywords)
        {
            if (botReply.Contains(banned, StringComparison.OrdinalIgnoreCase)) return false;
        }
        return true;
    }
  
}


