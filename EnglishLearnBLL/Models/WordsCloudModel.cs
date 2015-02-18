using System.Collections.Generic;

namespace EnglishLearnBLL.Models
{
    public sealed class WordsCloudModel
    {
        public WordsCloudModel()
        {
            Words = new List<WordJsonModel>();
        }

        public string UserName { get; set; }

        public int TotalWords { get; set; }
        
        public IList<WordJsonModel> Words { get; set; }
    }
}
