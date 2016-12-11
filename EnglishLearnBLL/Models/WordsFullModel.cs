using System.Collections.Generic;

namespace EnglishLearnBLL.Models
{
    public sealed class WordsFullModel
    {
        public WordsFullModel()
        {
            Words = new List<WordModelModel>();
        }

        public string UserName { get; set; }

        public int TotalWords { get; set; }
        
        public IList<WordModelModel> Words { get; set; }
    }
}
