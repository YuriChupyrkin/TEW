using SQLite;

namespace TewWinPhone.Entities
{
    public sealed class EnglishRussianWordEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string English { get; set; }

        public string Russian { get; set; }

        public string ExampleOfUse { get; set; }

        public int WordLevel { get; set; }

        public bool IsDeleted { get; set; }

        public string GetViewString()
        {
            return string.Format("{0} - {1} - {2}", this.English, this.Russian, this.WordLevel);
        }
    }
}
