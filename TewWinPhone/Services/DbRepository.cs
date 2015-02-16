using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using TewWinPhone.Entities;
using TewWinPhone.Models;
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
            return _dbConnection.Table<EnglishRussianWordEntity>();
        }

        public IEnumerable<EnglishRussianWordEntity> GetEnRuWords(Expression<Func<EnglishRussianWordEntity, bool>> expression)
        {
            return _dbConnection.Table<EnglishRussianWordEntity>().Where(expression);
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

        public EnglishRussianWordEntity DeleteWord(EnglishRussianWordEntity word)
        {
            var wordFromDb = _dbConnection.Table<EnglishRussianWordEntity>().FirstOrDefault(r => r.Id == word.Id);

            if(wordFromDb != null)
            {
                wordFromDb.IsDeleted = true;
                _dbConnection.Update(wordFromDb);
                return wordFromDb;
            }

            return null;
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

        public EnglishRussianWordEntity UpdateLevel(int wordId, bool isLevelUp, PickerTest pickerTest)
        {
            var word = _dbConnection.Table<EnglishRussianWordEntity>().FirstOrDefault(r => r.Id == wordId);

            if(wordId == null)
            {
                return null;
            }

            var maxLevel = 0;

            switch (pickerTest)
            {
                case PickerTest.EnRu:
                    maxLevel = 5;
                    break;
                case PickerTest.RuEn:
                    maxLevel = 10;
                    break;
                default:
                    maxLevel = 5;
                    break;
            }

            if (isLevelUp)
            {
                if(word.WordLevel < maxLevel)
                {
                    word.WordLevel += 1;
                    word.UpdateDate = DateTime.UtcNow;
                }
            }
            else
            {
                if (word.WordLevel > 0)
                {
                    word.WordLevel -= 1;
                    word.UpdateDate = DateTime.UtcNow;
                }
            }
            
            _dbConnection.Update(word);

            return word;
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
