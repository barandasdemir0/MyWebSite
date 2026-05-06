using DtoLayer.ContactDtos;
using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class ContactController : Controller
{

    private readonly IPublicMessageApiService _publicMessageApiService;
    private readonly IContactApiService _contactApiService;
    private readonly IPublicSocialMediaApiService _publicSocialMediaApiService;
    private readonly AdminSettings _adminSettings;

    public ContactController(IPublicMessageApiService publicMessageApiService, IContactApiService contactApiService, IOptions<AdminSettings> adminSettings, IPublicSocialMediaApiService publicSocialMediaApiService)
    {
        _publicMessageApiService = publicMessageApiService;
        _contactApiService = contactApiService;
        _adminSettings = adminSettings.Value;
        _publicSocialMediaApiService = publicSocialMediaApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            
            var contact = await _contactApiService.GetContactForEditAsync();
            var viewModel = new ContactMessageViewModel
        {
            contactDto = contact,
            SocialMediaDtos = await _publicSocialMediaApiService.GetAllAsync(),
            createMessageDto = new CreateMessageDto() // Form için boş bir nesne yarat
        };
        return View(viewModel);
        }
        catch (Exception)
        {
            // Eğer API veya DB çökerse sayfa patlamasın
            TempData["Error"] = "İletişim bilgileri şu anda yüklenemiyor.";
            return RedirectToAction("Index", "Default"); // Veya anasayfana (Home) yönlendir
        }
    }


    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactMessageViewModel ContactMessageViewModel)
    {
        if (!ModelState.IsValid)
        {
            // SAYFAYI GERİ DÖNDÜRMEDEN ÖNCE EKSİK VERİLERİ TEKRAR DOLDURUYORUZ
            ContactMessageViewModel.contactDto = await _contactApiService.GetContactForEditAsync();
            ContactMessageViewModel.SocialMediaDtos = await _publicSocialMediaApiService.GetAllAsync();

            return View(ContactMessageViewModel);
        }

        try
        {
            
            if (ContactMessageViewModel.createMessageDto != null)
            {
                ContactMessageViewModel.createMessageDto.ReceiverEmail = _adminSettings.NotificationEmail;
                ContactMessageViewModel.createMessageDto.Folder = SharedKernel.Enums.MessageFolder.Inbox;
            }
            var result = await _publicMessageApiService.SendContactMessageAsync(ContactMessageViewModel.createMessageDto!);
            if (result)
            {
                TempData["Success"] = "Mesajınız başarıyla gönderildi!";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Mesaj gönderilemedi, lütfen tekrar deneyin.";
            return View(ContactMessageViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "createMessageDto");
            ContactMessageViewModel.contactDto = await _contactApiService.GetContactForEditAsync();
            ContactMessageViewModel.SocialMediaDtos = await _publicSocialMediaApiService.GetAllAsync();
            return View(ContactMessageViewModel);
        }
    }


}
//socialmedia da ekle ve resim sorununu çöz