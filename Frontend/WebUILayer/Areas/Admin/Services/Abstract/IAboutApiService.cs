using DtoLayer.AboutDtos;


namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IAboutApiService:IGenericApiService<AboutDto,CreateAboutDto,UpdateAboutDto>
    {
        //sayfa yüklenirken çağırılacak veri varsa dolu yoksa boş dönecek
        Task<UpdateAboutDto> GetAboutForEditAsync();

        Task SaveAboutAsync(UpdateAboutDto updateAboutDto);
    }
}
