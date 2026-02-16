using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract;

public interface IHeroDal:IGenericRepository<Hero>
{
    Task<Hero?> GetSingleAsync(CancellationToken cancellationToken = default); 
}
