using App.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.Services
{
    public interface IGenericService<T> where T : BaseEntity
    {
        Task<ReturnType> GetById<ReturnType>(int id);
        Task<T> GetById(int id);
        Task<(List<ReturnType> Result, int TotalItems)> GetAll<ReturnType>(string sort=null, Expression<Func<T, bool>> predicate=null,
            int? page = null, int? limit = null,params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null, string sort = null,
            params Expression<Func<T, object>>[] includeProperties);
        Task<(List<ReturnType> Result, int TotalItems)> GetAllIncludeString<ReturnType>(string sort,Expression<Func<T, bool>> predicate,
            int? page = null, int? limit = null,params string[] includeProperties);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task<ReturnType> FindSingle<ReturnType>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<ReturnType> Add<ReturnType>(ReturnType model);
        Task<T> Add(T entity);
        Task<List<ReturnType>> AddMultipleEntities<ReturnType>(List<ReturnType> models);
        Task<IEnumerable<T>> AddMultipleEntities(IEnumerable<T> entities);
        Task<bool> DeleteMultipleEntities<ReturnType>(List<ReturnType> models);
        Task<bool> DeleteMultipleEntities(IEnumerable<T> entities);
        Task<ReturnType> Update<ReturnType>(ReturnType model);
        Task<T> Update(T entity);
        Task<List<ReturnType>> UpdateMultipleEntities<ReturnType>(List<ReturnType> models);
        Task<IEnumerable<T>> UpdateMultipleEntities(IEnumerable<T> entities);
        Task<ReturnType> UpdatePartial<ReturnType>(ReturnType model, params string[] updateProperties);
        Task<T> UpdatePartial(T entity, params string[] updateProperties);
        Task<bool> Delete(int id);
        Task<int> Save();
    }
}
