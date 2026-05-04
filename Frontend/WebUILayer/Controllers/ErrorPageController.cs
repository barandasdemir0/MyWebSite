using Microsoft.AspNetCore.Mvc;

namespace WebUILayer.Controllers;

public class ErrorPageController : Controller
{
    [Route("ErrorPage/Index")]
    public IActionResult Index(int code)
    {
        switch (code)
        {
            case 404:
                ViewBag.Title = "404 - Sayfa Bulunamadı";
                ViewBag.Message = "Aradığın sayfa uzay boşluğunda kaybolmuş gibi görünüyor. Ana sayfaya dönerek yolculuğuna devam edebilirsin.";
                ViewBag.ErrorCode = 404;
                break;
            case 401:
            case 403:
                ViewBag.Title = "Yetkisiz Erişim";
                ViewBag.Message = "Bu sayfayı görüntülemek için yeterli yetkiniz bulunmuyor. Lütfen giriş yaptığınızdan emin olun.";
                ViewBag.ErrorCode = code;
                break;
            case 500:
                ViewBag.Title = "500 - Sunucu Hatası";
                ViewBag.Message = "Sistemimizde geçici bir teknik sorun oluştu. Teknik ekip bilgilendirildi. Lütfen daha sonra tekrar deneyin.";
                ViewBag.ErrorCode = 500;
                break;
            default:
                // Diğer tüm bilinmeyen hatalar için
                ViewBag.Title = "Bir Hata Oluştu";
                ViewBag.Message = "İşleminiz sırasında beklenmedik bir hata meydana geldi.";
                ViewBag.ErrorCode = code;
                break;
        }

        return View();
    }
}
