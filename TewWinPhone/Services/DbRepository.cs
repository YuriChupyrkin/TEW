using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TewWinPhone.Entities;
using Windows.Storage;

namespace TewWinPhone.Services
{
    public sealed class DbRepository : IDisposable
    {
        private readonly SQLiteConnection _dbConnection;

        public DbRepository(string dbName)
        {
            var connectionString = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, dbName));
            _dbConnection = new SQLiteConnection(connectionString);

            _dbConnection.CreateTable<EnglishRussianWordEntity>();
            _dbConnection.CreateTable<UserEntity>();
        }

        public IEnumerable<EnglishRussianWordEntity> GetEnRuWords()
        {
            return _dbConnection.Table<EnglishRussianWordEntity>().ToList<EnglishRussianWordEntity>();
        }

        public void AddWord(EnglishRussianWordEntity word)
        {
            var englishWord = GetEnRuWords()
                .FirstOrDefault(r => r.English.Equals(word.English, StringComparison.OrdinalIgnoreCase) && r.IsDeleted == false);

            if (englishWord == null)
            {
                _dbConnection.Insert(word);
            }
            else
            {
                englishWord.Russian = word.Russian;
                englishWord.WordLevel = 0;
                _dbConnection.Update(englishWord);
            }
        }

        [Obsolete]
        public IEnumerable<UserEntity> GetAllUsers()
        {
            return _dbConnection.Table<UserEntity>().ToList<UserEntity>();
        }

        public string GetUserEmail()
        {
            var user = _dbConnection.Table<UserEntity>().FirstOrDefault();
            return user != null ? user.Email : string.Empty;
        }

        public void SetUserEmail(string email)
        {
            var user = _dbConnection.Table<UserEntity>().FirstOrDefault();
           
            if(user != null)
            {
                user.Email = email;
                _dbConnection.Update(user);
            }
            else
            {
                _dbConnection.CreateTable<UserEntity>();
                _dbConnection.Insert(new UserEntity { Email = email });
            }

        }

        public bool IsUserHaveEmail()
        {
            var email = GetUserEmail();
            return ApplicationValidator.IsValidatEmail(email);
        }

        public void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
            }
        }
    }
}
