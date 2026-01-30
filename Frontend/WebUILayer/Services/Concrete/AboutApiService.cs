using DtoLayer.AboutDto;
using Humanizer;
using Mapster;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete
{
    public class AboutApiService : GenericApiService<AboutDto, CreateAboutDto, UpdateAboutDto>, IAboutApiService
    {
        public AboutApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "abouts")
        {
        }

        public async Task<UpdateAboutDto> GetAboutForEditAsync()
        {
            var list = await GetAllAsync();
            var existing = list.FirstOrDefault();
            if (existing == null)
            {
                return new UpdateAboutDto();
            }
            return existing.Adapt<UpdateAboutDto>();

        }

        public async Task SaveAboutAsync(UpdateAboutDto updateAboutDto)
        {
            var list = await GetAllAsync();
            var existing = list.FirstOrDefault();
            if (existing == null)
            {
                var create = updateAboutDto.Adapt<CreateAboutDto>();
                await AddAsync(create);
            }
            else
            {
              
         
           
                await UpdateAsync(existing.Id,updateAboutDto);
            }
        }
    }
}
