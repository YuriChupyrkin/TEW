using System;
using Domain.Entities;
using Domain.Repositories;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using EntityFrameworkDAL.Context;
using EntityFrameworkDAL.Repositories;

namespace EntityFrameworkDAL.RepositoryFactories
{
  public class EFRepositoryFactory : IRepositoryFactory, IUnitOfWork
  {
    private readonly EnglishLearnContext _context;
    private bool _isDisposed;

    private IGenericRepository<Role, int> _roleRepository;
    private IGenericRepository<User, int> _userRepository;
    private IEnRuWordsRepository _enRuWordRepositor;

    public EFRepositoryFactory()
    {
      _context = new EnglishLearnContext();
      _isDisposed = false;
      _enRuWordRepositor = new EnRuWordsRepository(_context);
    }

    public bool IsDisposed
    {
      get { return _isDisposed; }
    }

    public IGenericRepository<User, int> UserRepository
    {
      get
      {
        _userRepository = _userRepository ?? new GenericRepository<User, int>(_context);
        return _userRepository;
      }
    }

    public IGenericRepository<Role, int> RoleRepository
    {
      get
      {
        _roleRepository = _roleRepository ?? new GenericRepository<Role, int>(_context);
        return _roleRepository;
      }
    }

    public void Commit()
    {
      _context.SaveChanges();
    }

    public void RollBack()
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing)
    {
      if (!_isDisposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
      }
      _isDisposed = true;
    }


    public IEnRuWordsRepository EnRuWordsRepository
    {
      get { return _enRuWordRepositor; }
    }
  }
}