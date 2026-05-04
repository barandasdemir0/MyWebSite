using DtoLayer.AboutDtos;
using Mapster;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class AboutApiService : GenericApiService<AboutDto, CreateAboutDto, UpdateAboutDto>, IAboutApiService
{
    public AboutApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "abouts")
    {
    }

    public async Task<UpdateAboutDto> GetAboutForEditAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/single");
        if (!response.IsSuccessStatusCode)
        {
            return new UpdateAboutDto();
        }
        var query = await response.Content.ReadFromJsonAsync<AboutDto>();
        if (query == null)
        {
            return new UpdateAboutDto();
        }
        return query.Adapt<UpdateAboutDto>();
    }

    public async Task SaveAboutAsync(UpdateAboutDto updateAboutDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/save", updateAboutDto);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
