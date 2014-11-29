using System;
using System.Text;
using System.Web.Helpers;
using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;

namespace WpfUI.Auth
{
  internal class UserProvider
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public UserProvider(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _unitOfWork = (IUnitOfWork)_repositoryFactory;
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
          user.Password = Crypto.HashPassword(password);

          var role = _repositoryFactory
            .RoleRepository.Find(x => x.RoleName == "user");

          if (role != null)
          {
            user.RoleId = role.Id;
            _repositoryFactory.UserRepository.Create(user);
            _unitOfWork.Commit();

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

        if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
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

    public string ResetPassword(string username)
    {
      try
      {
        var user = _repositoryFactory.UserRepository
            .Find(x => x.Email.ToLower() == username.ToLower());

        if (user != null)
        {
          var newPassword = RandomString(6);
          user.Password = Crypto.HashPassword(newPassword);
          _unitOfWork.Commit();
          return newPassword;
        }
        else
        {
          throw new NullReferenceException("User not exist");
        }
      }
      catch (NullReferenceException ex)
      {
        throw new NullReferenceException(ex.Message);
      }
    }

    public bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      var isChanged = false;
      try
      {
        var user = _repositoryFactory.UserRepository.
            Find(x => x.Email.ToLower() == username.ToLower());
        if (user != null && Crypto.VerifyHashedPassword(user.Password, oldPassword))
        {
          user.Password = Crypto.HashPassword(newPassword);
          _unitOfWork.Commit();
          isChanged = true;
        }
      }
      catch
      {
        isChanged = false;
      }
      return isChanged;
    }

    private string RandomString(int length)
    {
      var random = new Random(Environment.TickCount);
      var chars = "0123456789abcdefghijklmnopqrstuvwxyz";
      var builder = new StringBuilder(length);

      for (int i = 0; i < length; ++i)
        builder.Append(chars[random.Next(chars.Length)]);
      return builder.ToString();
    }
  }
}
