using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(); // hepsini getir
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter); //filtreli getir

        Task<T?> GetByIdAsync(Guid guid); //idye göre getir 
        Task<T?> GetAsync(Expression<Func<T, bool>> filter); //tek kayıt filtreli

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveAsync();
    }
}
