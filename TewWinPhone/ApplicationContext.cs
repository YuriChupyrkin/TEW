using SQLite;
using TewWinPhone.Models;
using TewWinPhone.Services;

namespace TewWinPhone
{
    internal sealed class ApplicationContext
    {
        //public static string ConnectionString { get; set; }

        //public static SQLiteConnection DbConnection { get; set; }

        public static DbRepository DbRepository { get; set; }

        public static NavigationService NavigationService { get; set; }

        public static PickerTest CurrentPickerTest { get; set; }

        public static string UserEmail { get; set; }
    }
}
