using BusinessLayer.Abstract;
using System.Text;
using System.Text.Json;

namespace BusinessLayer.Services;

public class GroqApiManager : IGroqApiService
{

    private readonly HttpClient _httpClient;

    public GroqApiManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> FetchResponseFromGroqAsync(string apiKey, string? modelName, string systemPromt, string userMessage)
    {
        var requeestBody = new
        {
            model = string.IsNullOrEmpty(modelName) ? "llama3-8b-8192" : modelName,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = systemPromt
                },
                new
                {
                    role = "user",
                    content = userMessage
                }
            },
            temperature = 0.2

        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var content = new StringContent(JsonSerializer.Serialize(requeestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("AI Servisine erişilemiyor.");
        }

        var jsonStr = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(jsonStr);

        return jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()!;

    }
}
