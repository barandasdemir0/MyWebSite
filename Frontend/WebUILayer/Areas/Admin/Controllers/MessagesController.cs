using DtoLayer.MessageDtos;
using Mapster;
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
    public async Task<IActionResult> Compose(CreateMessageDto createMessageDto,string action = "send", Guid? DraftId = null)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Lütfen Tüm Alanları Doldurun";
            return RedirectToAction(nameof(Index));
        }
        try
        {
            createMessageDto.SenderName = User.GetUserName();
            createMessageDto.SenderEmail = User.GetUserEmail();
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
            // Eski draft'ı sil (düzenleme + gönderme durumu)
            if (DraftId.HasValue && DraftId.Value != Guid.Empty)
            {
                createMessageDto.Folder = MessageFolder.Sent;
                await _messageApiService.AddAsync(createMessageDto);
                await _messageApiService.DeleteAsync(DraftId.Value);
            }
        }
        catch (Exception ex)
        {

            TempData["Error"] = "Mesaj gönderilemedi: " + ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> SendDraft(Guid guid)
    {
        try
        {
            var draft = await _messageApiService.GetDetailAsync(guid);
            if (draft==null)
            {
                TempData["Error"] = "Taslak bulunamadı";
                return RedirectToAction(nameof(Index), new
                {
                    category = "draft"
                });
            }

            var createDto = draft.Adapt<CreateMessageDto>();
            createDto.SenderName = User.GetUserName();
            createDto.SenderEmail = User.GetUserEmail();
            createDto.Folder = MessageFolder.Sent;


           
       




            await _messageApiService.AddAsync(createDto);
            await _messageApiService.DeleteAsync(guid);
            TempData["Success"] = "Taslak başarıyla gönderildi";
        }
        catch (Exception)
        {
            TempData["Error"] = "Taslak gönderilemedi";
        }
        return RedirectToAction(nameof(Index), new
        {
            category = "sent"
        });
    }




    [HttpPost]
    public async Task<IActionResult> ToggleStar(Guid id, string category = "inbox")
    {
        try
        {
            await _messageApiService.ToggleStarAsync(id);
            TempData["Success"] = "Yıldız durumu güncellendi";
        }
        catch (Exception)
        {
            TempData["Error"] = "Yıldız güncellenemedi";
        }
        return RedirectToAction(nameof(Index), new { category });
    }
    [HttpPost]
    public async Task<IActionResult> MarkAsRead(Guid id, string category = "inbox")
    {
        try
        {
            await _messageApiService.MarkAsReadAsync(id);
            TempData["Success"] = "Mesaj okundu olarak işaretlendi";
        }
        catch (Exception)
        {
            TempData["Error"] = "İşlem başarısız";
        }
        return RedirectToAction(nameof(Index), new { category });
    }
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, string category = "inbox")
    {
        try
        {
            await _messageApiService.DeleteAsync(id);
            TempData["Success"] = "Mesaj çöp kutusuna taşındı";
        }
        catch (Exception)
        {
            TempData["Error"] = "Silme işlemi başarısız";
        }
        return RedirectToAction(nameof(Index), new { category });
    }
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        try
        {
            await _messageApiService.RestoreAsync(id);
            TempData["Success"] = "Mesaj geri yüklendi";
        }
        catch (Exception)
        {
            TempData["Error"] = "Geri yükleme başarısız";
        }
        return RedirectToAction(nameof(Index), new { category = "trash" });
    }




}
