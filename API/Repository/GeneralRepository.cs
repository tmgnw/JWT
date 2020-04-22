using API.Base;
using API.Repository.Interface;
using API.RepositoryContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class GeneralRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : myContext
    {
        private readonly myContext _myContext;

        public GeneralRepository(myContext myContexts)
        {
            _myContext = myContexts;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await Get(id);
            if(entity == null)
            {
                return entity;
            }
            entity.DeleteDate = DateTimeOffset.Now;
            entity.IsDelete = true;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        //public async Task<List<TEntity>> Get()
        //{
        //    return await _myContext.Set<TEntity>().ToListAsync();
        //}

        public async Task<List<TEntity>> Get()
        {
            return await _myContext.Set<TEntity>().Where(x => x.IsDelete == false).ToListAsync();
        }

        public async Task<TEntity> Get(int Id)
        {
            return await _myContext.Set<TEntity>().FindAsync(Id);
        }

        public async Task<TEntity> Post(TEntity entity)
        {
            entity.CreateDate = DateTimeOffset.Now;
            entity.IsDelete = false;
            await _myContext.Set<TEntity>().AddAsync(entity);
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Put(TEntity entity)
        {
            entity.UpdateDate = DateTimeOffset.Now;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }
    }
}
