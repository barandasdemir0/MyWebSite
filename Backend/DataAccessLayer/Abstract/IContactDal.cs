using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract;

public interface IContactDal:IGenericRepository<Contact>
{
    Task<Contact?> GetSingleAsync(CancellationToken cancellationToken = default);
}
