using CV.EntityLayer.Entities;
using DtoLayer.GithubRepoDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGithubRepoService:IGenericService<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto>
    {
    }
}
