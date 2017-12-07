using System.Web;

namespace Doering.OneWayMail.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetIpAddress(this HttpRequest request)
        {
            string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return request.ServerVariables["REMOTE_ADDR"];
        }
    }
}