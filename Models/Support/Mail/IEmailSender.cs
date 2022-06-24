namespace kairosApp.Models.Support.Mail
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
