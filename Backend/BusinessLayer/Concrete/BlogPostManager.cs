using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AboutDto;
using DtoLayer.BlogpostDto;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class BlogPostManager : IBlogPostService
    {
        private readonly IBlogPostDal _repository;
        private readonly IMapper _mapper;

        public BlogPostManager(IBlogPostDal repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BlogPostListDto> AddAsync(CreateBlogPostDto dto)
        {
            var entity = _mapper.Map<BlogPost>(dto);
            entity.Slug = await UniqueSlugAsync(dto.Title);
            if (entity.IsPublished && entity.PublishedAt == null )
            {
                entity.PublishedAt = DateTime.UtcNow;
            }
           
            if (dto.TopicIds !=null)
            {
                foreach (var topicId in dto.TopicIds)
                {
                    entity.BlogTopics.Add(new BlogTopic { TopicId = topicId });
                }
            }
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            return _mapper.Map<BlogPostListDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _repository.GetByIdAsync(guid);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
                await _repository.SaveAsync();
            }
        }

        public async Task<List<BlogPostListDto>> GetAllAsync()
        {
            var entity = await _repository.GetAllAsync(
                tracking: false,
                includes: source => source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic));


            return _mapper.Map<List<BlogPostListDto>>(entity);
        }

        public async Task<List<BlogPostListDto>> GetAllAdminAsync()
        {
            var entity = await _repository.GetAllAdminAsync(
                tracking: false,
                includes: source => source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic));
            return _mapper.Map<List<BlogPostListDto>>(entity);


        }

        public async Task<BlogPostListDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _repository.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<BlogPostListDto>(entity);
        }

        public async Task<BlogPostDto?> GetBySlugAsync(string slug)
        {
            var entity = await _repository.GetAsync(x => x.Slug == slug, 
                tracking: false,
                includes: source => source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic));
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<BlogPostDto>(entity);
        }


        //bu yöntem ID ile çekmektir mantığı ne kadar doğru olsada işlem slugla çekmek daha profesyonelce yaklaşmaktır
        public async Task<BlogPostDto?> GetDetailsByIdAsync(Guid guid)
        {
            var entity = await _repository.GetByIdAsync(guid, 
                tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<BlogPostDto>(entity);
        }

        public async Task<BlogPostListDto?> RestoreAsync(Guid guid)
        {
            var entity = await _repository.RestoreDeletedByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();
            return _mapper.Map<BlogPostListDto>(entity);
        }

        public async Task<BlogPostListDto?> UpdateAsync(Guid id,UpdateBlogPostDto dto)
        {
            var entity = await _repository.GetAsync(x=>x.Id == id,tracking:true,includes: source => source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic));

            if (entity == null)
            {
                return null;
            }
           
            _mapper.Map(dto, entity);

            if (entity.IsPublished && entity.PublishedAt == null)
            {
                entity.PublishedAt = DateTime.UtcNow;
            }
            entity.BlogTopics.Clear();
            if (dto.TopicIds != null)
            {
                foreach (var topic in dto.TopicIds)
                {
                    entity.BlogTopics.Add(new BlogTopic { TopicId = topic });
                }
            }
            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();
            return _mapper.Map<BlogPostListDto>(entity);
        }





        //slug için benzersiz slug yapan metot

        private async Task<string> UniqueSlugAsync(string title) //sadece bu manager içinde çalışacak  dışarıdan çapırılamaz 
        {

            string slug = title.AutoSlug(); //burada slugumuzu ilk defa oluşturuyoruz

            string originalSlug = slug; //burada slug değerimizi originalslug adı altında bir değere atıyoruz bunun nedeni slug = C ise original slugda c-1 olması için
            int counter = 1; //sayaç atadık bu da sayacımızın ilk değeri 1

            while (await _repository.GetAsync(x=>x.Slug == slug) != null) //bu slug değeri veritabanında varmı 
            {
                counter++; //eğer varsa ve aynıysa counterı 1 arttır
                slug = $"{originalSlug}-{counter}"; //slug değerimize original slugu yaz ve tire koy ve counter adımızı yazdır
            }
            return slug; //ve slugumuzu döndür


        }



    }
}
