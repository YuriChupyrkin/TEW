using System.Collections.Generic;

namespace TewWinPhone.Models
{
    public sealed class SyncWordsModel
    {
        public SyncWordsModel()
        {
            Words = new List<WordModel>();
        }

        public string UserName { get; set; }

        public int TotalWords { get; set; }

        public IList<WordModel> Words { get; set; }
    }
}
