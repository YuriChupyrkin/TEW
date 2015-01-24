using System;

namespace TewWinPhone.Models
{
    public sealed class WordModel
    {
        public bool IsDeleted { get; set; }
        public DateTime UpdateDate { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }
        public int Level { get; set; }
        public string Example { get; set; }
    }
}
