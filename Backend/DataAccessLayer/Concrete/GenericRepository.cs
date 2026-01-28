using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region tanımlama ve constructor kısımları
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        #endregion

        #region CRUD İŞLEMLERİ

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }



        #endregion

        #region komple getirme

        public async Task<List<T>> GetAllAsync(bool tracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, bool tracking = true)
        {
            var query = _dbSet.Where(filter);
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(bool tracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, bool tracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.Where(filter);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        #endregion

        #region  getirme
        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(filter);


        }

        #endregion

        #region idye göre getirme


        public async Task<T?> GetByIdAsync(Guid guid, bool tracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == guid);
        }


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
