using DtoLayer.BlogPostDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class BlogPostApiService : GenericApiService<BlogPostDto, CreateBlogPostDto, UpdateBlogPostDto>, IBlogPostApiService
    {
        public BlogPostApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "blogposts")
        {
        }

        public async Task<PagedResult<BlogPostDto>> GetAllAdminAsync(PaginationQuery query)
        {
            var url = $"{_endpoint}/admin-all?PageNumber={query.PageNumber}&PageSize={query.PageSize}";
            if (query.TopicId.HasValue)
            {
                url += $"&TopicId={query.TopicId}";
            }

            var result = await _httpClient.GetFromJsonAsync<PagedResult<BlogPostDto>>(url);
            if (result == null)
            {
                return new PagedResult<BlogPostDto>();
            }
            return result;

        }

        public async Task<BlogPostListDto?> GetDetailById(Guid guid)
        {
            return await _httpClient.GetFromJsonAsync<BlogPostListDto>($"{_endpoint}/{guid}");
            
        }

        public async Task<BlogPostListDto?> GetDetailBySlug(string slug)
        {
            return await _httpClient.GetFromJsonAsync<BlogPostListDto>($"{_endpoint}/{slug}");
        }

        public async Task<List<BlogPostDto>> GetLatestAsync(int count)
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
            if (!response.IsSuccessStatusCode)
            {
                return new List<BlogPostDto>();
            }
            var result = await response.Content.ReadFromJsonAsync<List<BlogPostDto>>();
            if (result == null)
            {
                return new List<BlogPostDto>();
            }
            return result;
        }

        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PutAsync($"{_endpoint}/restore/{id}",null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

        }

        
    }
}
