using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AboutDto;
using DtoLayer.BlogpostDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class BlogPostManager : IBlogPostService
    {
        private readonly IGenericRepository<BlogPost> _repository;
        private readonly IMapper _mapper;

        public BlogPostManager(IGenericRepository<BlogPost> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

       

        public async Task<BlogPostDto> AddAsync(BlogPostDto dto)
        {
            var entity =  _mapper.Map<BlogPost>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            return _mapper.Map<BlogPostDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _repository.GetByIdAsync(guid);
            if (entity!=null)
            {
                await _repository.DeleteAsync(entity);
                await _repository.SaveAsync();
            }

        }

        public async Task<List<BlogPostDto>> GetAllAsync()
        {
            var entity = await _repository.GetAllAsync(tracking: false);
            return _mapper.Map<List<BlogPostDto>>(entity);
        }

        public async Task<BlogPostDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _repository.GetByIdAsync(guid,tracking:false);
            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<BlogPostDto>(entity);
        }

        public async Task<BlogPostDto> UpdateAsync(BlogPostDto dto)
        {
            var entity = _mapper.Map<BlogPost>(dto);
            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();
            return _mapper.Map<BlogPostDto>(entity);
        }
    }
}
