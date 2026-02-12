using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DataAccessLayer.Abstract
{
    public interface ICertificateDal:IGenericRepository<Certificate>
    {
        Task<Certificate?> RestoreDeleteByIdAsync(Guid guid, CancellationToken cancellationToken = default);
    }
}
