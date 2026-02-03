using DtoLayer.AboutDtos;
using Humanizer;
using Mapster;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

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
            //updateaboutdto.adap diğyerek girilen yerleri createye atıyorum yani mantık şu updatede de createde aynısı var updatedekini al createye yapıştır   Update formundaki veriyi alıp, kayıt yoksa Create için kullanıyorsun.
                        await AddAsync(create);
        }
        else
        {
            await UpdateAsync(existing.Id, updateAboutDto);
        }
    }
}
