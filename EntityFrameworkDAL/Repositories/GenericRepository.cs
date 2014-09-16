using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repositories;
using EntityFrameworkDAL.Context;

namespace EntityFrameworkDAL.Repositories
{
  public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : Entity
  {
    protected EnglishLearnContext Context;

    public GenericRepository(EnglishLearnContext context)
    {
      Context = context;
    }


    public IQueryable<TEntity> All()
    {
      return Context.Set<TEntity>();
    }

    public void Create(TEntity entity)
    {
      Context.Set<TEntity>().Add(entity);
    }

    public void Delete(TEntity entity)
    {
      Context.Set<TEntity>().Remove(entity);
    }

    public void Delete(TKey key)
    {
      TEntity entity = Find(key);
      Delete(entity);
    }

    public void Update(TEntity entity)
    {
    }

    public TEntity Find(TKey key)
    {
      return Context.Set<TEntity>().Find(key);
    }

    public TEntity Find(Expression<Func<TEntity, bool>> func)
    {
      return Context.Set<TEntity>().FirstOrDefault(func);
    }
  }
}