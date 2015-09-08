using System;
//using System.Web.Helpers;
using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;

namespace ConsoleUI.Authorize
{
  internal class UserProvider
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public UserProvider(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _unitOfWork = (IUnitOfWork) _repositoryFactory;
    }

    public User GetUser(string login)
    {
      return _repositoryFactory.UserRepository.Find(r => r.Email.Equals(login, StringComparison.OrdinalIgnoreCase));
    }

    public User CreateUser(string login, string password)
    {
      var user = GetUser(login);
      if (user == null)
      {
        try
        {
          user = new User();
          user.Email = login;
          user.Password = "password";// Crypto.HashPassword(password);

          var role = _repositoryFactory
            .RoleRepository.Find(x => x.RoleName == "user");

          if (role != null)
          {
            user.RoleId = role.Id;
            _repositoryFactory.UserRepository.Create(user);
            _unitOfWork.Commit();

            user = GetUser(login);
            return user;
          }
          throw new NullReferenceException("Role not found!");
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
      }
      return null;
    }

    public User ValidateUser(string username, string password)
    {
      try
      {
        User user = _repositoryFactory.UserRepository
          .Find(x => x.Email.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (user != null)// && Crypto.VerifyHashedPassword(user.Password, password))
        {
          return user;
        }
      }
      catch
      {
        return null;
      }
      return null;
    }
  }
}
