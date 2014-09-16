using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mail
{
    public interface IEmailSender
    {
        void Send(string from, string password, string mailto, string subject, string message);
    }
}
