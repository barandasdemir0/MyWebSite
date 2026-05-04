namespace BusinessLayer.Abstract;

public interface IGroqApiService
{
    Task<string> FetchResponseFromGroqAsync(string apiKey, string? modelName, string systemPromt, string userMessage);
}
