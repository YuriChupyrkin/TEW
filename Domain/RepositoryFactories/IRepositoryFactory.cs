using Domain.Entities;
using Domain.Repositories;

namespace Domain.RepositoryFactories
{
  public interface IRepositoryFactory
  {
    IGenericRepository<User, int> UserRepository { get; }
    IGenericRepository<Role, int> RoleRepository { get; }
    IEnRuWordsRepository EnRuWordsRepository { get; }
    bool IsDisposed { get; }
  }
}