using App.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicate, string sort, params Expression<Func<T, object>>[] includeProperties);
        Task<(IQueryable<T> Result, int TotalItems)> GetAll(Expression<Func<T, bool>> predicate, string sort, int? page = null, int? limit = null, params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetAllIncludeString(Expression<Func<T, bool>> predicate, string sort, params string[] includeProperties);
        Task<(IQueryable<T> Result, int TotalItems)> GetAllIncludeString(Expression<Func<T, bool>> predicate, string sort, int? page = null, int? limit = null, params string[] includeProperties);
        Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindSingleIncludeString(Expression<Func<T, bool>> predicate, params string[] includeProperties);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task AddMultipleEntities(IEnumerable<T> entities);
        Task Add(T entity);
        void Delete(T entity);
        void DeleteMultipleEntities(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateMultipleEntities(IEnumerable<T> entities);
        void UpdatePartial(T entity, params string[] updateProperties);
        Task<int> Save();
        Task<DateTime> GetCurrentDBDateTime();
    }
}
