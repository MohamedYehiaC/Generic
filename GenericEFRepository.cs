using App.Core.Entities.Base;
using App.Core.Interfaces.Repository;
using App.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace App.Infrastructure.Data
{
    public class GenericEFRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDBContext mContext;

        public GenericEFRepository(AppDBContext context)
        {
            mContext = context;
        }


        public virtual Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicate, string sort, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = mContext.Set<T>().Where(predicate).ApplySort(sort).AsNoTracking();
            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return Task.FromResult(query);
        }

        public virtual Task<(IQueryable<T> Result, int TotalItems)> GetAll(Expression<Func<T, bool>> predicate, string sort, int? page = null, int? limit = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = null;
            if (predicate == null)
                query = mContext.Set<T>();
            else
                query = mContext.Set<T>().Where(predicate).Distinct().ApplySort(sort).AsNoTracking();


            var count = query.Count();
            if (page != null && limit != null)
            {
                query = query.Skip(page.Value * limit.Value);
                query = query.Take(limit.Value);
            }
            if (includeProperties != null && includeProperties.Count() > 0)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return Task.FromResult((query, count));
        }
        public virtual Task<IQueryable<T>> GetAllIncludeString(Expression<Func<T, bool>> predicate, string sort, params string[] includeProperties)
        {
            IQueryable<T> query = mContext.Set<T>().Where(predicate).ApplySort(sort);
            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return Task.FromResult(query);
        }

        public virtual Task<(IQueryable<T> Result, int TotalItems)> GetAllIncludeString(Expression<Func<T, bool>> predicate, string sort, int? page = null, int? limit = null, params string[] includeProperties)
        {
            IQueryable<T> query = mContext.Set<T>().Where(predicate).ApplySort(sort);

            var count = query.Count();

            if (page != null && limit != null)
            {
                query = query.Skip(page.Value * limit.Value);
                query = query.Take(limit.Value);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return Task.FromResult((query, count));
        }
        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            var found = await mContext.Set<T>().AnyAsync(predicate);
            return found;
        }
        public async Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = mContext.Set<T>().Where(predicate).AsNoTracking();

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task<T> FindSingleIncludeString(Expression<Func<T, bool>> predicate, params string[] includeProperties)
        {
            var query = mContext.Set<T>().Where(predicate);

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return await query.SingleOrDefaultAsync();
        }

        public virtual Task Add(T entity)
        {
            var result = mContext.Set<T>().AddAsync(entity);
            return result.AsTask();
        }
        public virtual Task AddMultipleEntities(IEnumerable<T> entities)
        {
            var result = mContext.Set<T>().AddRangeAsync(entities);
            return result;
        }
        public virtual void Delete(T entity)
        {
            mContext.Set<T>().Remove(entity);
        }
        public virtual void DeleteMultipleEntities(IEnumerable<T> entities)
        {
            mContext.Set<T>().RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            if (mContext.Entry(entity).State == EntityState.Detached)
                mContext.Attach(entity);
            mContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void UpdateMultipleEntities(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (mContext.Entry(entity).State == EntityState.Detached)
                    mContext.Attach(entity);
                mContext.Entry(entity).State = EntityState.Modified;
            }
        }
        public virtual void UpdatePartial(T entity, params string[] updateProperties)
        {
            foreach (string prop in updateProperties)
            {
                mContext.Entry(entity).Property(prop).IsModified = true;
            }
        }

        public virtual async Task<int> Save()
        {
            return await mContext.SaveChangesAsync();
        }

        public async Task<DateTime> GetCurrentDBDateTime()
        {
            DateTime result;

            string query = "SET @CurrentDateTime = GETUTCDATE()";
            SqlParameter outputParam = DBHelperContext.GetOutPutParameter("@CurrentDateTime", SqlDbType.DateTime);

            await mContext.Database.ExecuteSqlRawAsync(query, outputParam);

            result = (DateTime)outputParam.Value;

            return result;
		}
    }

}
