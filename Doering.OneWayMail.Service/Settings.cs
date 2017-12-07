using System;
using System.Configuration;

namespace Doering.OneWayMail.Service
{
    public static class Settings
    {
        public static int CheckInterval
        {
            get
            {
                var stringValue = ConfigurationManager.AppSettings[nameof(CheckInterval)];
                int intValue;
                if (!int.TryParse(stringValue, out intValue))
                {
                    return 5000;
                }
                return intValue * 1000;
            }
        }

        public static string WebsiteUrl => ConfigurationManager.AppSettings[nameof(WebsiteUrl)];
        public static string ImapServer => ConfigurationManager.AppSettings[nameof(ImapServer)];
        public static int ImapPort => Convert.ToInt32(ConfigurationManager.AppSettings[nameof(ImapPort)]);
        public static string ImapUser => ConfigurationManager.AppSettings[nameof(ImapUser)];
        public static string ImapPassword => ConfigurationManager.AppSettings[nameof(ImapPassword)];
        public static string SmtpServer => ConfigurationManager.AppSettings[nameof(SmtpServer)];
        public static int SmtpPort => Convert.ToInt32(ConfigurationManager.AppSettings[nameof(SmtpPort)]);
        public static string SmtpUser => ConfigurationManager.AppSettings[nameof(SmtpUser)];
        public static string SmtpPassword => ConfigurationManager.AppSettings[nameof(SmtpPassword)];
        public static string EmailSenderDisplayName => ConfigurationManager.AppSettings[nameof(EmailSenderDisplayName)];
        public static string EmailSenderSmtpAddress => ConfigurationManager.AppSettings[nameof(EmailSenderSmtpAddress)];
    }
}
