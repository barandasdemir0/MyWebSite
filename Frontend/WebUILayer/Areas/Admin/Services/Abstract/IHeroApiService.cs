using DtoLayer.HeroDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IHeroApiService:IGenericApiService<HeroDto,CreateHeroDto,UpdateHeroDto>
    {
        Task<UpdateHeroDto> GetHeroForEditAsync();
        Task SaveHeroAsync(UpdateHeroDto updateHeroDto);
    }
}
