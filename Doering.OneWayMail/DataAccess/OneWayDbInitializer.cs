using System.Data.Entity;

namespace Doering.OneWayMail.DataAccess
{
    public class OneWayDbInitializer : CreateDatabaseIfNotExists<OneWayMailContext>
    {
        protected override void Seed(OneWayMailContext context)
        {
            // TODO: Add default configuration here.
            //context.AllowedDomains.Add(new Model.Domain("gmx.de"));
            //context.AllowedForwardAddresses.Add(new Model.EmailAddress("test@gmx.de"));
            //context.SaveChanges();
        }
    }
}
