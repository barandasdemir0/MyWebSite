using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class SitemapController : Controller
{
    private readonly IPublicBlogPostApiService _publicBlogPostApiService;
    private readonly IPublicProjectApiService _publicProjectApiService;

    public SitemapController(IPublicBlogPostApiService publicBlogPostApiService, IPublicProjectApiService publicProjectApiService)
    {
        _publicBlogPostApiService = publicBlogPostApiService;
        _publicProjectApiService = publicProjectApiService;
    }

    [Route("sitemap.xml")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> Index()
    {
        var host = Request.Scheme + "://" + Request.Host;
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
        // 1. Statik Sayfalar
        var staticPages = new[] { "", "/About", "/Resume", "/Project", "/Blog", "/GuestBook", "/Contact" };
        foreach (var page in staticPages)
        {
            sb.AppendLine("<url>");
            sb.AppendLine($"  <loc>{host}{page}</loc>");
            sb.AppendLine($"  <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("  <changefreq>weekly</changefreq>");
            sb.AppendLine("  <priority>0.8</priority>");
            sb.AppendLine("</url>");
        }
        // 2. Dinamik Blog Yazıları
        try
        {
            var blogs = await _publicBlogPostApiService.GetAllAsync();
            if (blogs != null)
            {
                foreach (var blog in blogs)
                {
                    sb.AppendLine("<url>");
                    sb.AppendLine($"  <loc>{host}/Blog/BlogDetails/{blog.Slug}</loc>");
                    sb.AppendLine($"  <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
                    sb.AppendLine("  <changefreq>monthly</changefreq>");
                    sb.AppendLine("  <priority>0.6</priority>");
                    sb.AppendLine("</url>");
                }
            }
        }
        catch { /* API hatası olursa sitemap yine de çalışsın */ }
        // 3. Dinamik Projeler
        try
        {
            var projects = await _publicProjectApiService.GetAllAsync();
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    sb.AppendLine("<url>");
                    sb.AppendLine($"  <loc>{host}/Project/ProjectDetails/{project.Slug}</loc>");
                    sb.AppendLine($"  <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
                    sb.AppendLine("  <changefreq>monthly</changefreq>");
                    sb.AppendLine("  <priority>0.6</priority>");
                    sb.AppendLine("</url>");
                }
            }
        }
        catch { /* API hatası olursa sitemap yine de çalışsın */ }
        sb.AppendLine("</urlset>");
        return Content(sb.ToString(), "text/xml", Encoding.UTF8);
    }
}
