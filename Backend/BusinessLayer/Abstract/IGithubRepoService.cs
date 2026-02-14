using CV.EntityLayer.Entities;
using DtoLayer.BlogpostDtos;
using DtoLayer.GithubRepoDtos;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGithubRepoService:IGenericService<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto>
    {
        Task<PagedResult<GithubRepoDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    }
}
