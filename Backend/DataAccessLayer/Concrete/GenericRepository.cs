using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Shareds;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region tanımlama ve constructor kısımları
        protected readonly AppDbContext _context;   // Veritabanı bağlantısı
        private readonly DbSet<T> _dbSet;           // Çalışılan tablo

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();  // T hangi entity ise onun tablosunu al
        }
        #endregion

        #region CRUD İŞLEMLERİ

        public async Task AddAsync(T entity,
    CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteAsync(T entity,
    CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;  // async uyumluluğu için
        }

        public async Task<int> SaveAsync(
    CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }



        #endregion

        #region komple getirme



        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, QueryOptions<T>? options = null,
    CancellationToken cancellationToken = default)
        {

            //  Başlangıç sorgusu oluştur
            var query = _dbSet.AsQueryable();
            // SQL: SELECT * FROM JobSkills (henüz çalışmadı, sadece sorgu oluşturuluyor)
            // + Global Filter otomatik: WHERE IsDeleted = 0



            //  Filtre varsa ekle
            if (filter != null)
            {
                query = query.Where(filter);
                // SQL: ... WHERE IsDeleted = 0 AND [filter koşulu]
            }

            //  Include varsa ekle
            if (includes != null)
            {
                query = includes(query);
                // SQL: ... LEFT JOIN [ilişkili tablo] ON ...

            }

            if (options!=null)// ayar varmı herhangi bir sıralama veya balka bir şey istenmişmi
            {
                if (options.OrderBy != null) //sana nasıl olacağı söylenmişmi
                {
                    IOrderedQueryable<T> orderedQuery; //bununla sorgumuzu açıyoruz var query = context.Products; bunun mantığında çalışır
                    if (options.Descending)
                    {
                        orderedQuery = query.OrderByDescending(options.OrderBy); //büyükten küçüğe sırala
                    }
                    else
                    {
                        orderedQuery = query.OrderBy(options.OrderBy); //küçükten büyüğe sırala
                    }
                    if (options.ThenBy!= null) //thenbylar ise 2.ci sıralamayı yapar örnek a ile başlayan kişileri sıraladık ismi a ile başlayıp soyadı b ile başlayan kişileri sırala gibi
                    {
                        if (options.ThenByDescending)
                        {
                            orderedQuery = orderedQuery.ThenByDescending(options.ThenBy); 
                        }
                        else
                        {
                            orderedQuery = orderedQuery.ThenBy(options.ThenBy);
                        }
                    }

                    query = orderedQuery;
                }
                if (options.Skip.HasValue) //skip atlama komutudr hasvalue varmı anlamı taşır yani null değilse zaten su satırdan başla su satırdan sonrasını ver yani o satırdan öncesini atla örnek : 1 2 3 4 5 biz skip(2) dersek 3 4 5
                {
                    query = query.Skip(options.Skip.Value);
                }
                if (options.Take.HasValue) //aynı mantık skip ile belirtilen kayda kadar al take(2) dersek 1 2 alır sadece
                {
                    query = query.Take(options.Take.Value);
                }

            }
            //  Tracking kapalıysa devre dışı bırak
            if (!tracking)
            {
                query = query.AsNoTrackingWithIdentityResolution();
                // EF Core bu entity'leri takip ETMEZ → RAM tasarrufu
                // WithIdentityResolution → aynı Id'li entity'ler aynı referansı paylaşır
            }

            //  Sorguyu çalıştır ve sonuçları listele
            return await query.ToListAsync(cancellationToken);
            // BURADA SQL gerçekten veritabanına gider ve çalışır


            //1. _dbSet.AsQueryable()       →SQL henüz çalışmadı, sorgu BUILD ediliyor
            //2. .Where(filter)             →SQL henüz çalışmadı, WHERE eklendi
            //3. includes(query)            →SQL henüz çalışmadı, JOIN eklendi
            //4. .AsNoTracking()            →SQL henüz çalışmadı, tracking kapatıldı
            //5. .ToListAsync()             →SQL ŞİMDİ ÇALIŞTI! Veritabanına gitti ve sonuç döndü

        }


        public async Task<List<T>> GetAllAdminAsync(bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
    CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            //// ❗ Global Filter'ı DEVRE DIŞI birak
            // Normalde: WHERE IsDeleted = 0 (otomatik eklenir)
            // Şimdi:    (hiç filtre yok, silinmişler de gelir)
            query = query.IgnoreQueryFilters();
            if (!tracking)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            if (includes != null)
            {
                query = includes(query);

            }

            return await query.ToListAsync(cancellationToken);
        }


        #endregion

        #region  getirme


        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
    CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();
            if (includes != null)
            {
                query = includes(query);
            }

            if (!tracking)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            return await query.FirstOrDefaultAsync(filter, cancellationToken);


        }

        #endregion

        #region idye göre getirme




        public async Task<T?> GetByIdAsync(Guid guid, bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null,
    CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();
            if (includes != null)
            {
                query = includes(query);
            }
            if (!tracking)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            return await query.FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
            // SQL: SELECT TOP 1 * FROM JobSkills WHERE Id = 'guid' AND IsDeleted = 0
            // Bulunamazsa NULL döner

        }



        #endregion

        #region eski kodlar


        //public async Task<List<T>> GetAllAsync(bool tracking = true)
        //{
        //    var query = _dbSet.AsQueryable();
        //    if (!tracking)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    return await query.ToListAsync();
        //}

        //public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, bool tracking = true)
        //{
        //    var query = _dbSet.Where(filter);
        //    if (!tracking)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    return await query.ToListAsync();
        //}

        //public async Task<List<T>> GetAllAsync(bool tracking = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
        //{
        //    var query = _dbSet.AsQueryable();
        //    if (includes != null)
        //    {

        //        query = includes(query);

        //    }

        //    if (!tracking)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    return await query.ToListAsync();
        //}
        //public async Task<T?> GetByIdAsync(Guid guid, bool tracking = true)
        //{
        //    var query = _dbSet.AsQueryable();
        //    if (!tracking)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    return await query.FirstOrDefaultAsync(x => x.Id == guid);
        //}


        //public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true)
        //{
        //    var query = _dbSet.AsQueryable();
        //    if (!tracking)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    return await query.FirstOrDefaultAsync(filter);
        //}
        #endregion

        #region trackingsiz hali eski kodlar



        //trackingsiz yapı bu şekildeydi
        //public async Task AddAsync(T entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //}

        //public async Task DeleteAsync(T entity)
        //{
        //    _dbSet.Remove(entity);
        //    await Task.CompletedTask;
        //}

        //public async Task<List<T>> GetAllAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}

        //public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await _dbSet.Where(filter).ToListAsync();
        //}

        //public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await _dbSet.FirstOrDefaultAsync(filter);
        //}

        //public async Task<T?> GetByIdAsync(Guid guid)
        //{
        //    return await _dbSet.FindAsync(guid);
        //}

        //public async Task<int> SaveAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}

        //public async Task UpdateAsync(T entity)
        //{
        //    _dbSet.Update(entity);
        //    await Task.CompletedTask;
        //}

        #endregion
    }
}
