using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
using SharedKernel.Shared;
using System.Security.Cryptography.X509Certificates;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
public class MessagesController : Controller
{

    private readonly IMessageApiService _messageApiService;

    public MessagesController(IMessageApiService messageApiService)
    {
        _messageApiService = messageApiService;
    }


    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PaginationQuery query, string category = "inbox")
    {

       
        var pagedResult = await _messageApiService.GetAllAdminAsync(query);

        switch (category.ToLower())
        {

            case "starred":
                pagedResult = await _messageApiService.GetStarredAsync(query);
                break;
            case "read":
                pagedResult = await _messageApiService.GetReadAsync(query);
                break;
            case "sent":
                pagedResult = await _messageApiService.GetByFolderAsync(MessageFolder.Sent, query);
                break;
            case "draft":
                pagedResult = await _messageApiService.GetByFolderAsync(MessageFolder.Draft, query);
                break;
            case "trash":
                pagedResult = await _messageApiService.GetByFolderAsync(MessageFolder.Trash, query);
                break;
            default:
                pagedResult = await _messageApiService.GetByFolderAsync(MessageFolder.Inbox, query);
                category = "inbox";
                break;
        }

        var folderCounts = await _messageApiService.GetFolderCountsAsync();

        var model = new MessageIndexViewModel
        {
            messageDtos = pagedResult.Items,
            CurrentPage = pagedResult.PageNumber,
            TotalPages = pagedResult.TotalPages,
            ActiveCategory = category,
            FolderCounts = folderCounts
        };


        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> Compose(CreateMessageDto createMessageDto,string action = "send")
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Lütfen Tüm Alanları Doldurun";
            return RedirectToAction(nameof(Index));
        }
        try
        {
            createMessageDto.SenderName = "Baran Daşdemir";
            createMessageDto.SenderEmail = "barandasdemir.bd@gmail.com";
            if (action=="draft")
            {
                createMessageDto.Folder = MessageFolder.Draft;
                await _messageApiService.AddAsync(createMessageDto);
                TempData["Success"] = "Mesaj taslağa kaydedildi.";

            }
            else
            {
                createMessageDto.Folder = MessageFolder.Sent;
                await _messageApiService.AddAsync(createMessageDto);
                TempData["Success"] = "Mesaj başarıyla gönderildi.";
            }
        }
        catch (Exception ex)
        {

            TempData["Error"] = "Mesaj gönderilemedi: " + ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> ToggleStar(Guid id)
    {
        return await this.SafeAction(
            action: () => _messageApiService.ToggleStarAsync(id),
            successMessage: "Yıldız durumu güncellendi",
            ErrorMessage: "Yıldız güncellenemedi"
            );
        
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        return await this.SafeAction(
            action: () => _messageApiService.MarkAsReadAsync(id),
            successMessage: "Mesaj okundu olarak işaretlendi",
            ErrorMessage: "İşlem başarısız"
        );
    }
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await this.SafeAction(
            action: () => _messageApiService.DeleteAsync(id),
            successMessage: "Mesaj çöp kutusuna taşındı",
            ErrorMessage: "Silme işlemi başarısız"
        );
    }
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        return await this.SafeAction(
            action: () => _messageApiService.RestoreAsync(id),
            successMessage: "Mesaj geri yüklendi",
            ErrorMessage: "Geri yükleme başarısız"
        );
    }





}
