using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IMessageDal:IGenericRepository<Message>
    {
        Task<(List<Message> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default);
      
    }
}
