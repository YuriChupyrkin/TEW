using System;

namespace Domain.UnitOfWork
{
  public interface IUnitOfWork : IDisposable
  {
    void Commit();
    void RollBack();
  }
}