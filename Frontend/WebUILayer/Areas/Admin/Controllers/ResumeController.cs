using DtoLayer.CertificateDtos;
using DtoLayer.EducationDtos;
using DtoLayer.ExperienceDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class ResumeController : Controller
{
    private readonly IExperienceApiService _experienceApiService;
    private readonly IEducationApiService _educationApiService;
    private readonly ICertificateApiService _certificateApiService;

    public ResumeController(IExperienceApiService experienceApiService, IEducationApiService educationApiService, ICertificateApiService certificateApiService)
    {
        _experienceApiService = experienceApiService;
        _educationApiService = educationApiService;
        _certificateApiService = certificateApiService;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region experience için create update delete restore işlemleri

    [HttpGet]
    public IActionResult CreateExperience() => View();
    [HttpPost]
    public async Task<IActionResult> CreateExperience(CreateExperienceDto createExperienceDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createExperienceDto);
        }
        try
        {
            var query = await _experienceApiService.AddAsync(createExperienceDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(createExperienceDto);
        }
       
    }

    [HttpGet]
    public async Task<IActionResult> UpdateExperience(Guid guid)
    {
        var query = await _experienceApiService.GetByIdAsync(guid);
        if (query == null)
        {
            return NotFound();
        }
        return View(query.Adapt<UpdateExperienceDto>());
    }

    [HttpPost]
    public async Task<IActionResult> UpdateExperience(UpdateExperienceDto updateExperienceDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateExperienceDto);
        }
        try
        {
            await _experienceApiService.UpdateAsync(updateExperienceDto.Id, updateExperienceDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateExperienceDto);
        }
    }




    [HttpPost]
    public async Task<IActionResult> DeleteExperience(Guid id)
    {
        try
        {
            await _experienceApiService.DeleteAsync(id);
            TempData["Success"] = "Silme işlemi başarılı.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Silme işlemi başarısız oldu.";
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> RestoreExperience(Guid id)
    {
        try
        {
            await _experienceApiService.RestoreAsync(id);
            TempData["Success"] = "Geri yükleme işlemi başarılı oldu.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Geri yükleme işlemi başarısız oldu.";
        }

        return RedirectToAction(nameof(Index));
    }


   

    #endregion

    #region Education için create update delete restore işlemleri
    [HttpGet]
    public IActionResult CreateEducation() => View();

    [HttpPost]
    public async Task<IActionResult> CreateEducation(CreateEducationDto createEducationDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createEducationDto);
        }
        try
        {
            var query = await _educationApiService.AddAsync(createEducationDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {

            ModelState.AddApiError(ex);
            return View(createEducationDto);
        }
      
    }

    [HttpGet]
    public async Task<IActionResult> UpdateEducation(Guid guid)
    {
        var query = await _educationApiService.GetByIdAsync(guid);
        if (query == null)
        {
            return NotFound();
        }
        return View(query.Adapt<UpdateEducationDto>());
    }
    [HttpPost]
    public async Task<IActionResult> UpdateEducation(UpdateEducationDto updateEducationDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateEducationDto);
        }
        try
        {
            await _educationApiService.UpdateAsync(updateEducationDto.Id, updateEducationDto);
            return View(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateEducationDto);
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEducation(Guid id)
    {
        try
        {
            await _educationApiService.DeleteAsync(id);
            TempData["Success"] = "Silme işlemi başarılı.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Silme işlemi başarısız oldu.";
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> RestoreEducation(Guid id)
    {
        try
        {
            await _educationApiService.RestoreAsync(id);
            TempData["Success"] = "Geri yükleme işlemi başarılı oldu.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Geri yükleme işlemi başarısız oldu.";
        }

        return RedirectToAction(nameof(Index));
    }





    #endregion

    #region Certificates için create update delete restore işlemleri

    [HttpGet]
    public IActionResult CreateCertificates() => View();
    [HttpPost]
    public async Task<IActionResult> CreateCertificates(CreateCertificateDto createCertificateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createCertificateDto);
        }
        try
        {
            var query = await _certificateApiService.AddAsync(createCertificateDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(createCertificateDto);
        }
      
    }
    [HttpGet]
    public async Task<IActionResult> UpdateCertificates(Guid guid)
    {
        var values = await _certificateApiService.GetByIdAsync(guid);
        if (values == null)
        {
            return NotFound();
        }
        return View(values.Adapt<UpdateCertificateDto>());
    }
    [HttpPost]
    public async Task<IActionResult> UpdateCertificates (UpdateCertificateDto updateCertificateDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateCertificateDto);
        }
        try
        {
            var values = await _certificateApiService.UpdateAsync(updateCertificateDto.Id, updateCertificateDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateCertificateDto);
        }
    }


    [HttpPost]
    public async Task<IActionResult> DeleteCertificates(Guid id)
    {
        try
        {
            await _certificateApiService.DeleteAsync(id);
            TempData["Success"] = "Silme işlemi başarılı.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Silme işlemi başarısız oldu.";
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> RestoreCertificates(Guid id)
    {
        try
        {
            await _certificateApiService.RestoreAsync(id);
            TempData["Success"] = "Geri yükleme işlemi başarılı oldu.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Geri yükleme işlemi başarısız oldu.";
        }

        return RedirectToAction(nameof(Index));
    }



    

    #endregion




}
