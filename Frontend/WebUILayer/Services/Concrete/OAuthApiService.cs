using DtoLayer.GuestBookDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class OAuthApiService : IOAuthApiService
{
    private readonly HttpClient _httpClient;

    public OAuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OAuthUserProfileDto?> GithubLoginAsync(GithubAuthRequestDto githubAuthRequestDto)
    {
        var res = await _httpClient.PostAsJsonAsync("OAuth/github", githubAuthRequestDto);
        if (!res.IsSuccessStatusCode)
        {
            return null;
        }
        return await res.Content.ReadFromJsonAsync<OAuthUserProfileDto>();
    }

    public async Task<OAuthUserProfileDto?> LinkedinLoginAsync(LinkedinAuthRequestDto linkedinAuthRequestDto)
    {
        var res = await _httpClient.PostAsJsonAsync("OAuth/linkedin", linkedinAuthRequestDto);
        if (!res.IsSuccessStatusCode)
        {
            return null;
        }
        return await res.Content.ReadFromJsonAsync<OAuthUserProfileDto>();
    }
}
