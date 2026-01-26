using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IGenericRepository<T> where T : class
    {

        #region getirme komutları

        #region tümünü getirme

        Task<List<T>> GetAllAsync(bool tracking = true); // hepsini getir
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, bool tracking = true); //filtreli olarak getir

        Task<List<T>> GetAllAsync(bool tracking = true, params Expression<Func<T, object>>[] includes); //projeleri getir ama include olanları getir hani mesela project ve blog benzerse beraber getir
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, bool tracking = true, params Expression<Func<T, object>>[] includes); //projeleri getir kategorilerini dahil et ama ek olarak yazarlarınıda dahil et gibi bir anlam

        #endregion

        #region idye göre getirme

        Task<T?> GetByIdAsync(Guid guid, bool tracking = true); //idye göre getir 

        #endregion

        #region getirmek

        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true); //idye göre yine ama tek kayıt olacak ve filtreli olacak

        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true, params Expression<Func<T, object>>[] includes); //idye göre tek kayıt gelecek yine ama detaylarıyla gelecek include olanları getir hani mesela project ve blog benzerse beraber getir

        #endregion

        #endregion

        #region temel crud

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveAsync();

        #endregion
    }
}
