using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.BlogpostDtos;
using DtoLayer.GithubRepoDtos;
using MapsterMapper;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class GithubRepoManager : IGithubRepoService
{

    private readonly IGithubRepoDal _githubRepoDal;
    private readonly IMapper _mapper;

    public GithubRepoManager(IGithubRepoDal githubRepoDal, IMapper mapper)
    {
        _githubRepoDal = githubRepoDal;
        _mapper = mapper;
    }

    public async Task<GithubRepoDto> AddAsync(CreateGithubRepoDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GithubRepo>(dto);
        await _githubRepoDal.AddAsync(entity,cancellationToken);
        await _githubRepoDal.SaveAsync(cancellationToken);
        return _mapper.Map<GithubRepoDto>(entity);
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _githubRepoDal.GetByIdAsync(guid,cancellationToken:cancellationToken);
        if (entity != null)
        {
            await _githubRepoDal.DeleteAsync(entity,cancellationToken);
            await _githubRepoDal.SaveAsync(cancellationToken);
        }
    }

 

    public async Task<List<GithubRepoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _githubRepoDal.GetAllAsync(tracking: false, cancellationToken:cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<PagedResult<GithubRepoDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _githubRepoDal.GetUserListPagesAsync(query.PageNumber, query.PageSize, cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(items).ToPagedResult(query.PageNumber, query.PageSize, totalCount);
    }

    public async Task<GithubRepoDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _githubRepoDal.GetByIdAsync(guid, tracking: false,cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<GithubRepoDto>(entity);
    }

    public async Task<GithubRepoDto?> UpdateAsync(Guid guid, UpdateGithubRepoDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _githubRepoDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        _mapper.Map(dto, entity);
        await _githubRepoDal.UpdateAsync(entity, cancellationToken);
        await _githubRepoDal.SaveAsync(cancellationToken);
        return _mapper.Map<GithubRepoDto>(entity);

    }
}
