using DtoLayer.ContactDtos;
using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class ContactController : Controller
{

    private readonly IPublicMessageApiService _publicMessageApiService;
    private readonly IContactApiService _contactApiService;

    public ContactController(IPublicMessageApiService publicMessageApiService, IContactApiService contactApiService)
    {
        _publicMessageApiService = publicMessageApiService;
        _contactApiService = contactApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var contact = await _contactApiService.GetContactForEditAsync();
        var viewModel = new ContactMessageViewModel
        {
            contactDto = contact,
            createMessageDto = new CreateMessageDto() // Form için boş bir nesne yarat
        };
        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Index(ContactMessageViewModel ContactMessageViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(ContactMessageViewModel);
        }
        ContactMessageViewModel.createMessageDto?.ReceiverEmail = "barandasdemir.bd@gmail.com";

        ContactMessageViewModel.createMessageDto?.Folder = SharedKernel.Enums.MessageFolder.Inbox;

        var result = await _publicMessageApiService.SendContactMessageAsync(ContactMessageViewModel.createMessageDto!);
        if (result)
        {
            TempData["Success"] = "Mesajınız başarıyla gönderildi!";
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = "Mesaj gönderilemedi, lütfen tekrar deneyin.";
        return View(ContactMessageViewModel.createMessageDto);
    }



}
