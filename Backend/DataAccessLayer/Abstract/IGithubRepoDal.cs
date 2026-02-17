using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IGithubRepoDal:IGenericRepository<GithubRepo>
    {
        Task<(List<GithubRepo> Items, int TotalCount)> GetUserListPagesAsync(int page, int size, CancellationToken cancellationToken = default);
    }
}
