using System;

namespace EnglishLearnBLL.Models
{
    public sealed class CheckUpdateModel
    {
        public string UserName { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool? IsError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
