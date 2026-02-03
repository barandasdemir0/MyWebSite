using CV.EntityLayer.Entities;
using DtoLayer.GithubRepoDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGithubRepoService:IGenericService<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto>
    {
    }
}
