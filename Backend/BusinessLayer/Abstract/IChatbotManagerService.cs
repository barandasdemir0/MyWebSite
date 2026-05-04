namespace BusinessLayer.Abstract;

public interface IChatbotManagerService
{
    Task<string> ProcessUserMessageAsync(string question, string currentUrl);
}
