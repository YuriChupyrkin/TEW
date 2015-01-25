using System;

namespace EnglishLearnBLL.Models
{
    public sealed class UserUpdateDateModel
    {
        public string UserName { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
