using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(bool tracking = true); // hepsini getir
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, bool tracking = true); //filtreli getir

        Task<T?> GetByIdAsync(Guid guid,bool tracking = true); //idye göre getir 
        Task<T?> GetAsync(Expression<Func<T, bool>> filter , bool tracking = true); //tek kayıt filtreli

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveAsync();
    }
}
