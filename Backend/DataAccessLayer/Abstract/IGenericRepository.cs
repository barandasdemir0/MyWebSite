using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Query;

namespace DataAccessLayer.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity //bu repositori sadece baseentityden tüeryen entityler kullanabilir
    {
        //filter ile  // Opsiyonel filtre işlemi yapıyoruz WHERE koşulu. null ise tüm kayıtlar gelir
        //tracking ile //trackinci kapalı yapıyoruz bu da performans oluyor EF Core entity'yi takip etsin mi? Sadece okuma için false
        //includes ile // ilişkili verileri dahil ediyoruz JOIN yapılacak ilişkileri belirtir

        #region getirme komutları

        #region tümünü getirme
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null); //projeleri getir kategorilerini dahil et ama ek olarak yazarlarınıda dahil et gibi bir anlam

        Task<List<T>> GetAllAdminAsync(bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null); // hepsini getir ama ignorefiltersi geç yani admin kısmında silinenleride göstermek için bu

        #endregion

        #region idye göre getirme

        Task<T?> GetByIdAsync(Guid guid, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null); //idye göre getir 

        #endregion

        #region getirmek

        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null); //idye göre tek kayıt gelecek yine ama detaylarıyla gelecek include olanları getir hani mesela project ve blog benzerse beraber getir

        #endregion

        #endregion

        #region temel crud

        Task AddAsync(T entity);   // Yeni kayıt ekle (INSERT)
        Task UpdateAsync(T entity); // Kayıt güncelle (UPDATE)
        Task DeleteAsync(T entity);// Kayıt sil (soft delete — SaveChanges'te yakalanıyor)
        Task<int> SaveAsync();  // Değişiklikleri veritabanına kaydet neden int
                                // 1 kayıt eklendi → 1
                                //1 kayıt güncellendi → 1
                                //2 kayıt silindi → 2
                                //Hiçbir şey değişmedi → 0

        #endregion








        #region eski gereksiz kodlar


        //Task<List<T>> GetAllAsync(bool tracking = true); // hepsini getir

        //Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, bool tracking = true); //filtreli olarak getir

        //Task<List<T>> GetAllAsync(bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null); //projeleri getir ama include olanları getir hani mesela project ve blog benzerse beraber getir






        //Task<T?> GetByIdAsync(Guid guid, bool tracking = true); //idye göre getir 


        //Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true); //idye göre yine ama tek kayıt olacak ve filtreli olacak


        #endregion
    }
}
