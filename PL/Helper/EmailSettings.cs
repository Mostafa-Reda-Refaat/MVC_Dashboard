using System.Net;
using System.Net.Mail;

namespace PL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("mostafa.reda.10897@gmail.com", "jexymbyfvzhwvajq");

            client.Send("mostafa.reda.10897@gmail.com", email.To, email.Title, email.Body);
        }
    }
}
