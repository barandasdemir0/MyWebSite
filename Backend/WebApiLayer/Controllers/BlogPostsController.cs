using BusinessLayer.Abstract;
using DtoLayer.AboutDto;
using DtoLayer.BlogpostDto;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll()
        {
            var query = await _blogPostService.GetAllAsync();
            return Ok(query);
        }

        //[HttpGet("{id}")] //idye göre getirme 
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var query = await _blogPostService.GetByIdAsync(id);
        //    if (query == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(query);
        //}

        [HttpGet("slug/{slug}")] //burası kullanıcıya gösterilecek yazan yazı
        public async Task<IActionResult> GetDetail(string slug)
        {
            var query = await _blogPostService.GetBySlugAsync(slug);
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);
        }

        [HttpGet("{id}")]//burasıda admine gösterilecek yazan yazı id ile çekiliyorki tüm veri gelsin
        public async Task<IActionResult> GetDetailById(Guid id)
        {
            var query = await _blogPostService.GetDetailsByIdAsync(id);
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlogPostDto createBlogPostDto)
        {
            var query = await _blogPostService.AddAsync(createBlogPostDto);
            return CreatedAtAction(nameof(GetDetailById), new { id = query.Id }, query);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBlogPostDto updateBlogPostDto)
        {
            //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
            var query = await _blogPostService.UpdateAsync(id, updateBlogPostDto);
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = await _blogPostService.GetByIdAsync(id);
            if (query == null)
            {
                return NotFound();
            }
            await _blogPostService.DeleteAsync(id);
            return Ok(query);
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var entity = await _blogPostService.RestoreAsync(id);
            return Ok(entity);
        }



    }
}
