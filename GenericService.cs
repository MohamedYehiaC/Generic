using App.Common.Interfaces.Logger;
using App.Common.Resources;
using App.Common.Services.ExceptionHandler;
using App.Core.Entities.Base;
using App.Core.Interfaces.Repository;
using App.Core.Interfaces.Services;
using AutoMapper;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        protected readonly Ilogger mLogger;
        protected readonly IMapper mapper;

        protected readonly IGenericRepository<T> mRepository;

        public GenericService(IGenericRepository<T> oRepository, Ilogger logger, IMapper _mapper)
        {
            mRepository = oRepository;
            mLogger = logger;
            mapper = _mapper;

        }
        public virtual async Task<ReturnType> GetById<ReturnType>(int id)
        {
            var entity = await mRepository.FindSingle(a => a.Id == id);
            var model = mapper.Map<T, ReturnType>(entity);
            return model;
        }
        public virtual async Task<T> GetById(int id)
        {
            var entity = await mRepository.FindSingle(a => a.Id == id);
            return entity;
        }
        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            var found = await mRepository.Exists(predicate);
            return found;
        }
        public virtual async Task<ReturnType> FindSingle<ReturnType>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var entity = await mRepository.FindSingle(predicate,includeProperties);
            var model = mapper.Map<T, ReturnType>(entity);
            return model;
        }
        public virtual async Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var entity = await mRepository.FindSingle(predicate, includeProperties);
            return entity;
        }
        public virtual async Task<(List<ReturnType> Result, int TotalItems)> GetAll<ReturnType>(
            string sort=null,
            Expression<Func<T, bool>> predicate = null,
            int? page = null, int? limit = null,
            params Expression<Func<T, object>>[] includeProperties
            )
        {
            var entities = await mRepository.GetAll(predicate,sort, page, limit, includeProperties);
            var models = mapper.Map<List<T>, List<ReturnType>>(entities.Result.ToList());
            return (models, entities.TotalItems);
        }
        public virtual async Task<(List<ReturnType> Result, int TotalItems)> GetAllIncludeString<ReturnType>(
            string sort,
            Expression<Func<T, bool>> predicate = null,
            int? page = null, int? limit = null,
            params string[] includeProperties
            )
        {
            var entities = await mRepository.GetAllIncludeString(predicate, sort, page, limit, includeProperties);
            var models = mapper.Map<List<T>, List<ReturnType>>(entities.Result.ToList());
            return (models, entities.TotalItems);
        }
        public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null,string sort = null,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var entities = await mRepository.GetAll(predicate, sort, includeProperties);
            return entities;
        }
        public virtual async Task<ReturnType> Add<ReturnType>(ReturnType model)
        {
            var entity = mapper.Map<ReturnType, T>(model);
            await mRepository.Add(entity);
            await Save();
            return mapper.Map<T, ReturnType>(entity);
        }
        public virtual async Task<T> Add(T entity)
        {
            await mRepository.Add(entity);
            await Save();
            return entity;
        }
        public virtual async Task<List<ReturnType>> AddMultipleEntities<ReturnType>(List<ReturnType> models)
        {
            var entities = mapper.Map<List<ReturnType>, List<T>>(models);
            await mRepository.AddMultipleEntities(entities);
            await Save();
            return mapper.Map<List<T>, List<ReturnType>>(entities);
        }
        public virtual async Task<IEnumerable<T>> AddMultipleEntities(IEnumerable<T> entities)
        {
            await mRepository.AddMultipleEntities(entities);
            await Save();
            return entities;
        }
        public virtual async Task<ReturnType> Update<ReturnType>(ReturnType model)
        {
            var entity = mapper.Map<ReturnType, T>(model);
            mRepository.Update(entity);
            await Save();
            return mapper.Map<T, ReturnType>(entity);
        }
        public virtual async Task<T> Update(T entity)
        {
            mRepository.Update(entity);
            await Save();
            return entity;
        }
        public virtual async Task<List<ReturnType>> UpdateMultipleEntities<ReturnType>(List<ReturnType> models)
        {
            var entities = mapper.Map<List<ReturnType>, List<T>>(models);
            mRepository.UpdateMultipleEntities(entities);
            await Save();
            return mapper.Map<List<T>, List<ReturnType>>(entities);
        }
        public virtual async Task<IEnumerable<T>> UpdateMultipleEntities(IEnumerable<T> entities)
        {
            mRepository.UpdateMultipleEntities(entities);
            await Save();
            return entities;
        }
        public virtual async Task<ReturnType> UpdatePartial<ReturnType>(ReturnType model, params string[] updateProperties)
        {
            var entity = mapper.Map<ReturnType, T>(model);
            mRepository.UpdatePartial(entity, updateProperties);
            await Save();
            return mapper.Map<T, ReturnType>(entity);
        }
        public virtual async Task<T> UpdatePartial(T entity, params string[] updateProperties)
        {
            mRepository.UpdatePartial(entity, updateProperties);
            await Save();
            return entity;
        }
        public virtual async Task<bool> Delete(int id)
        {
            var entity = await mRepository.FindSingle(a => a.Id == id);
            mRepository.Delete(entity);
            await Save();
            return true;
        }
        public virtual async Task<bool> DeleteMultipleEntities<ReturnType>(List<ReturnType> models)
        {
            var entities = mapper.Map<List<ReturnType>, List<T>>(models);
            mRepository.DeleteMultipleEntities(entities);
            await Save();
            return true;
        }
        public virtual async Task<bool> DeleteMultipleEntities(IEnumerable<T> entities)
        {
            mRepository.DeleteMultipleEntities(entities);
            await Save();
            return true;
        }
        public async Task<int> Save()
        {
            int affectedRecoreds = await mRepository.Save();
            if (affectedRecoreds == 0)
                throw new ExceptionService(ExceptionResource.NoRecordsSaved);
            return affectedRecoreds;
        }
    }

}
