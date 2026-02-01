using DtoLayer.BlogpostDto;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class BlogPostApiService : GenericApiService<BlogPostDto, CreateBlogPostDto, UpdateBlogPostDto>, IBlogPostApiService
    {
        public BlogPostApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "blogposts")
        {
        }

        public async Task<List<BlogPostDto>> GetAllAdminAsync()
        {
            var query = await _httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{_endpoint}/admin-all");
            if (query == null)
            {
                return new List<BlogPostDto>();
            }
            return query;

        }

        public async Task<BlogPostListDto?> GetDetailById(Guid guid)
        {
            return await _httpClient.GetFromJsonAsync<BlogPostListDto>($"{_endpoint}/{guid}");
            
        }

        public async Task<BlogPostListDto?> GetDetailBySlug(string slug)
        {
            return await _httpClient.GetFromJsonAsync<BlogPostListDto>($"{_endpoint}/{slug}");
        }

        public async Task RestoreAsync(Guid guid)
        {
            var response = await _httpClient.PostAsync($"{_endpoint}/restore/{guid}",null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

        }
    }
}
