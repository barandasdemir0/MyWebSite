using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Shareds;


namespace DataAccessLayer.Abstract;

//Cancelation token ne işe yarar  "Bu işlemi iptal et" sinyali.

//Kullanıcı bir sayfa isteği yaptı ama:

//Sayfayı kapattı
//Başka bir sayfaya geçti
//Tarayıcıyı kapattı
//İnterneti koptu
//Bu durumda sunucu hala çalışıyor — veritabanı sorgusu devam ediyor, boşuna kaynak harcıyor.CancellationToken bunu engeller
public interface IGenericRepository<T> where T : BaseEntity //bu repositori sadece baseentityden tüeryen entityler kullanabilir
{
    //filter ile  // Opsiyonel filtre işlemi yapıyoruz WHERE koşulu. null ise tüm kayıtlar gelir
    //tracking ile //trackinci kapalı yapıyoruz bu da performans oluyor EF Core entity'yi takip etsin mi? Sadece okuma için false
    //includes ile // ilişkili verileri dahil ediyoruz JOIN yapılacak ilişkileri belirtir

    #region getirme komutları

    #region tümünü getirme
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, QueryOptions<T>? options = null,
    CancellationToken cancellationToken = default); //projeleri getir kategorilerini dahil et ama ek olarak yazarlarınıda dahil et gibi bir anlam

    Task<List<T>> GetAllAdminAsync(bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
    CancellationToken cancellationToken = default); // hepsini getir ama ignorefiltersi geç yani admin kısmında silinenleride göstermek için bu

    #endregion

    #region idye göre getirme

    Task<T?> GetByIdAsync(Guid guid, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
    CancellationToken cancellationToken = default); //idye göre getir 

    #endregion

    #region getirmek

    Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
    CancellationToken cancellationToken = default); //idye göre tek kayıt gelecek yine ama detaylarıyla gelecek include olanları getir hani mesela project ve blog benzerse beraber getir

    #endregion

    #endregion

    #region temel crud

    Task AddAsync(T entity,
    CancellationToken cancellationToken = default);   // Yeni kayıt ekle (INSERT)
    Task UpdateAsync(T entity,
    CancellationToken cancellationToken = default); // Kayıt güncelle (UPDATE)
    Task DeleteAsync(T entity,
    CancellationToken cancellationToken = default);// Kayıt sil (soft delete — SaveChanges'te yakalanıyor)
    Task<int> SaveAsync(
    CancellationToken cancellationToken = default);  // Değişiklikleri veritabanına kaydet neden int
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
