using SQLite;
using TewWinPhone.Services;

namespace TewWinPhone
{
    internal sealed class ApplicationContext
    {
        public static string ConnectionString { get; set; }

        public static SQLiteConnection DbConnection { get; set; }

        public static NavigationService NavigationService { get; set; }
    }
}
