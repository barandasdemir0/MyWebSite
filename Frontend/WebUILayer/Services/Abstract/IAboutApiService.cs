using DtoLayer.AboutDto;

namespace WebUILayer.Services.Abstract
{
    public interface IAboutApiService:IGenericApiService<AboutDto,CreateAboutDto,UpdateAboutDto>
    {
        //sayfa yüklenirken çağırılacak veri varsa dolu yoksa boş dönecek
        Task<UpdateAboutDto> GetAboutForEditAsync();

        Task SaveAboutAsync(UpdateAboutDto updateAboutDto);
    }
}
