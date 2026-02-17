using BusinessLayer.Abstract;
using DtoLayer.BlogPostDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
public sealed class BlogPostsController:CrudController<BlogPostDto,CreateBlogPostDto,UpdateBlogPostDto>
{

    private readonly IBlogPostService _blogPostService;

    public BlogPostsController(IBlogPostService blogPostService) : base(blogPostService)
    {
        _blogPostService = blogPostService;
    }

    [HttpGet("admin-all")] // hepsini getirme 
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var result = await _blogPostService.GetAllAdminAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("user-all")] // hepsini getirme 
    public async Task<IActionResult> GetAllUser([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var result = await _blogPostService.GetAllUserAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("slug/{slug}")] //burası kullanıcıya gösterilecek yazan yazı
    public async Task<IActionResult> GetDetail(string slug, CancellationToken cancellationToken)
    {
        var query = await _blogPostService.GetBySlugAsync(slug, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpGet("{id}")]//burasıda admine gösterilecek yazan yazı id ile çekiliyorki tüm veri gelsin
    public async Task<IActionResult> GetDetailById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _blogPostService.GetDetailsByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken )
    {
        var entity = await _blogPostService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }


    [HttpGet("latest/{count}")]
    public async Task<IActionResult> GetLatest(int count,CancellationToken cancellationToken)
    {
        var values = await _blogPostService.GetLatestAsync(count, cancellationToken);
        if (values==null)
        {
            return NotFound();
        }
        return Ok(values);
    }
}
