using System;
using Doering.OneWayMail.DataAccess;

namespace Doering.OneWayMail.Web
{
    public partial class Remove : System.Web.UI.Page
    {
        public string UrlParam => Request.Params["Id"];

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid subscriptionId;
            if (UrlParam == null || !Guid.TryParse(UrlParam, out subscriptionId))
            {
                NotFound();
                return;
            }
            using (var db = new OneWayMailContext())
            {
                var subscription = db.Subscriptions.Find(subscriptionId);
                if (subscription == null)
                {
                    NotFound();
                    return;
                }
                if (subscription.Enabled)
                {
                    subscription.Enabled = false;
                    subscription.DisabledAt = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }

        private void NotFound()
        {
            Response.StatusCode = 404;
            Response.StatusDescription = "There's no subscription with this id.";
            Response.End();
        }
    }
}