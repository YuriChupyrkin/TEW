using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IGenericRepository { }


    public interface IGenericRepository<TEntity, TKey>: IGenericRepository
        where TEntity:  Entity 
    {
        IQueryable<TEntity> All();
        void Create(TEntity entity);
        void Delete(TEntity entity);
        void Delete(TKey key);
        void Update(TEntity entity);
        TEntity Find(TKey key);
        TEntity Find(Expression<Func<TEntity, bool>> func);
    }
}