using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.GithubRepoDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class GithubRepoManager : IGithubRepoService
    {

        private readonly IGithubRepoDal _githubRepoDal;
        private readonly IMapper _mapper;

        public GithubRepoManager(IGithubRepoDal githubRepoDal, IMapper mapper)
        {
            _githubRepoDal = githubRepoDal;
            _mapper = mapper;
        }

        public async Task<GithubRepoDto> AddAsync(CreateGithubRepoDto dto)
        {
            var entity = _mapper.Map<GithubRepo>(dto);
            await _githubRepoDal.AddAsync(entity);
            await _githubRepoDal.SaveAsync();
            return _mapper.Map<GithubRepoDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _githubRepoDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _githubRepoDal.DeleteAsync(entity);
                await _githubRepoDal.SaveAsync();
            }
        }

        public async Task<List<GithubRepoDto>> GetAllAsync()
        {
            var entity = await _githubRepoDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<GithubRepoDto>>(entity);
        }

        public async Task<GithubRepoDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _githubRepoDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<GithubRepoDto>(entity);
        }

        public async Task<GithubRepoDto?> UpdateAsync(UpdateGithubRepoDto dto)
        {
            var entity = await _githubRepoDal.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _githubRepoDal.UpdateAsync(entity);
            await _githubRepoDal.SaveAsync();
            return _mapper.Map<GithubRepoDto>(entity);

        }
    }
}
