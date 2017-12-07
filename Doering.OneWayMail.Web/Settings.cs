using System.Configuration;

namespace Doering.OneWayMail.Web
{
    public static class Settings
    {
        public static string MailDomain => ConfigurationManager.AppSettings[nameof(MailDomain)];
        public static string OneWayServiceEmailAddress => ConfigurationManager.AppSettings[nameof(OneWayServiceEmailAddress)];
    }
}