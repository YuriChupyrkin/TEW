using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using EnglishLearnBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TewCloud.Helpers
{
    internal sealed class SyncHelper
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IUnitOfWork _unitOfWork;

        public SyncHelper(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _unitOfWork = (IUnitOfWork)repositoryFactory;
        }

        public WordsCloudModel GetUserWords(UserUpdateDateModel updateModel)
        {
            var user = GetUser(updateModel.UserName);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            IEnumerable<EnRuWord> enRuWords = new List<EnRuWord>();

            if (updateModel.UpdateDate == 0)
            {
                enRuWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
                 .Where(r => r.UserId == user.Id);
            }
            else
            {
                enRuWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
                  .Where(r => r.UserId == user.Id && r.UpdateDate >= new DateTime(updateModel.UpdateDate));

            }

            var wordsCloudModel = CreateWordsCloudModel(updateModel.UserName, enRuWords);

            return wordsCloudModel;
        }

        public User GetUser(string userName)
        {
            var user = _repositoryFactory.UserRepository
              .All().FirstOrDefault(r => r.Email == userName);

            if (user == null)
            {
                user = CreateNewUser(userName);
            }
            return user;
        }

        public int GetUserId(string userName)
        {
            var user = GetUser(userName);

            if (user == null)
            {
                user = CreateNewUser(userName);
            }
            return user.Id;

        }

        public User CreateNewUser(string userName)
        {
            var newUser = new User
            {
                Email = userName,
                Password = "password",
                RoleId = 2
            };

            _repositoryFactory.UserRepository.Create(newUser);
            _unitOfWork.Commit();

            return newUser;
        }

        public WordsCloudModel CreateWordsCloudModel(string userName, IEnumerable<EnRuWord> enRuWords)
        {
            var wordsCloudModel = new WordsCloudModel
            {
                UserName = userName
            };

            var words = new List<WordJsonModel>();

            foreach (var word in enRuWords)
            {
                var viewModel = new WordJsonModel
                {
                    English = word.EnglishWord.EnWord,
                    Russian = word.RussianWord.RuWord,
                    Level = word.WordLevel,
                    Example = word.Example,
                    UpdateDate = word.UpdateDate,
                    IsDeleted = word.IsDeleted
                };

                words.Add(viewModel);
            }

            wordsCloudModel.Words = words;
            wordsCloudModel.TotalWords = GetWordCount(userName);

            return wordsCloudModel;
        }

        public CheckUpdateModel CheckUpdates(string userName)
        {
            var user = GetUser(userName);

            var words = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == user.Id).ToList();

            var maxDate = DateTime.MinValue;

            if(words != null && words.Any())
            { 
                maxDate = words.Max(r => r.UpdateDate);
            }

            var checkUpdateModel = new CheckUpdateModel
            {
                LastUpdate = maxDate,
                UserName = userName
            };

            return checkUpdateModel;
        }

        public int GetWordCount(string userName)
        {
            return _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Count(r => r.User.Email == userName);
        }

    }
}
