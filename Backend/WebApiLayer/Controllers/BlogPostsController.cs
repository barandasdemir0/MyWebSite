using BusinessLayer.Abstract;
using DtoLayer.AboutDtos;
using DtoLayer.BlogpostDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using System.Threading;

namespace WebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class BlogPostsController:ControllerBase
    {

        private readonly IBlogPostService _blogPostService;

        public BlogPostsController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet] // hepsini getirme 
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            var query = await _blogPostService.GetAllAsync(cancellationToken);
            return Ok(query);
        }

        [HttpGet("admin-all")] // hepsini getirme 
        public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
        {
            var result = await _blogPostService.GetAllAdminAsync(query, cancellationToken);
            return Ok(result);
        }

        //[HttpGet("{id}")] //idye göre getirme 
        //public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        //{
        //    var query = await _blogPostService.GetByIdAsync(id,cancellationToken);
        //    if (query == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(query);
        //}

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


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlogPostDto createBlogPostDto, CancellationToken cancellationToken)
        {
            var query = await _blogPostService.AddAsync(createBlogPostDto, cancellationToken);
            return CreatedAtAction(  // HTTP 201 Created + body'de result + Location header
                nameof(GetDetailById)   //  Hangi action'a yönlendirilecek
                , new { id = query.Id } //  O action'ın parametreleri
                , query);   //  Response body'deki veri

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBlogPostDto updateBlogPostDto, CancellationToken cancellationToken)
        {
            //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
            var query = await _blogPostService.UpdateAsync(id, updateBlogPostDto, cancellationToken);
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var query = await _blogPostService.GetByIdAsync(id, cancellationToken);
            if (query == null)
            {
                return NotFound();
            }
            await _blogPostService.DeleteAsync(id, cancellationToken);
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
}
