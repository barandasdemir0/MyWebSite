using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.ViewComponents.AdminComponents;

public class _ResumeCertificatePartialComponent:ViewComponent
{
    private readonly ICertificateApiService _certificateApiService;

    public _ResumeCertificatePartialComponent(ICertificateApiService certificateApiService)
    {
        _certificateApiService = certificateApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var values = await _certificateApiService.GetAdminAllAsync();
        return View(values);
    }
}
