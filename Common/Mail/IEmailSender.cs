namespace Common.Mail
{
  public interface IEmailSender
  {
    void Send(string from, string password, string mailto, string subject, string message);
  }
}
