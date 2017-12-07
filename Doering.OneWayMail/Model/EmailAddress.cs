using System.ComponentModel.DataAnnotations;

namespace Doering.OneWayMail.Model
{
    public class EmailAddress
    {
        [Key]
        public string SmtpAddress { get; set; }

        public EmailAddress()
        {
        }

        public EmailAddress(string smtpAddress)
        {
            SmtpAddress = smtpAddress;
        }
    }
}
