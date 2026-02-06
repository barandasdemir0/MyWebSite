using DtoLayer.HeroDtos;
using Mapster;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class HeroApiService : GenericApiService<HeroDto, CreateHeroDto, UpdateHeroDto>, IHeroApiService
    {
        public HeroApiService(HttpClient httpClient) : base(httpClient, "heroes")
        {
        }

        public async Task<UpdateHeroDto> GetHeroForEditAsync()
        {
            var list = await GetAllAsync();
            var query = list.FirstOrDefault();
            if (query == null)
            {
                return new UpdateHeroDto();
            }
            return query.Adapt<UpdateHeroDto>();
        }

        public async Task SaveHeroAsync(UpdateHeroDto updateHeroDto)
        {
            var list = await GetAllAsync();
            var query = list.FirstOrDefault();
            if (query == null)
            {
                var create = updateHeroDto.Adapt<CreateHeroDto>();
                await AddAsync(create);
            }
            else
            {
                await UpdateAsync(query.Id, updateHeroDto);
            }
        }
    }
}
