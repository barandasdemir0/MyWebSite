using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AboutDtos;
using DtoLayer.BlogpostDtos;
using DtoLayer.Shared;
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

        public async Task<BlogPostDto> AddAsync(CreateBlogPostDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<BlogPost>(dto);
            entity.Slug = await UniqueSlugAsync(dto.Title, cancellationToken);
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
            await _repository.AddAsync(entity, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
            return _mapper.Map<BlogPostDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(guid,cancellationToken:cancellationToken);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity, cancellationToken);
                await _repository.SaveAsync(cancellationToken);
            }
        }

        public async Task<List<BlogPostDto>> GetAllAsync( CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAllAsync(
                tracking: false,
                includes: source => source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic),cancellationToken:cancellationToken);


            return _mapper.Map<List<BlogPostDto>>(entity);
        }

        public async Task<PagedResult<BlogPostListDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default)
        {
            // DAL'daki özel metodunu çağırıyorsun.
            // Include ve Tracking işlemleri ZATEN O METODUN İÇİNDE YAPILDI.
            var (items, totalCount) = await _repository.GetAdminListPagesAsync(query.PageNumber, query.PageSize ,query.TopicId, cancellationToken);
           return _mapper.Map<List<BlogPostListDto>>(items).ToPagedResult(query.PageNumber, query.PageSize, totalCount);
          
        }

        public async Task<BlogPostDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(guid, tracking: false,includes:source=>source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic),cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<BlogPostDto>(entity);
        }

        public async Task<BlogPostDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAsync(x => x.Slug == slug, 
                tracking: false,
                includes: source => source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic),cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<BlogPostDto>(entity);
        }


        //bu yöntem ID ile çekmektir mantığı ne kadar doğru olsada işlem slugla çekmek daha profesyonelce yaklaşmaktır
        public async Task<BlogPostDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(guid, 
                tracking: false, includes: source => source.Include(x => x.BlogTopics).ThenInclude(y => y.Topic),cancellationToken:cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<BlogPostDto>(entity);
        }

        public async Task<BlogPostListDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.RestoreDeletedByIdAsync(guid,cancellationToken);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            await _repository.UpdateAsync(entity, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
            return _mapper.Map<BlogPostListDto>(entity);
        }

        public async Task<BlogPostDto?> UpdateAsync(Guid id,UpdateBlogPostDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAsync(x=>x.Id == id,tracking:true,includes: source => source.Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic),cancellationToken:cancellationToken);

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
            await _repository.UpdateAsync(entity, cancellationToken);
            await _repository.SaveAsync( cancellationToken);
            return _mapper.Map<BlogPostDto>(entity);
        }





        //slug için benzersiz slug yapan metot

        private async Task<string> UniqueSlugAsync(string title, CancellationToken cancellationToken = default) //sadece bu manager içinde çalışacak  dışarıdan çapırılamaz 
        {

            string slug = title.AutoSlug(); //burada slugumuzu ilk defa oluşturuyoruz

            string originalSlug = slug; //burada slug değerimizi originalslug adı altında bir değere atıyoruz bunun nedeni slug = C ise original slugda c-1 olması için
            int counter = 1; //sayaç atadık bu da sayacımızın ilk değeri 1

            while (await _repository.GetAsync(x=>x.Slug == slug, cancellationToken: cancellationToken) != null) //bu slug değeri veritabanında varmı 
            {
                counter++; //eğer varsa ve aynıysa counterı 1 arttır
                slug = $"{originalSlug}-{counter}"; //slug değerimize original slugu yaz ve tire koy ve counter adımızı yazdır
            }
            return slug; //ve slugumuzu döndür


        }



    }
}
