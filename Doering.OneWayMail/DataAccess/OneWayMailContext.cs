using System.Data.Entity;
using Doering.OneWayMail.Model;

namespace Doering.OneWayMail.DataAccess
{
    public class OneWayMailContext : DbContext
    {
        public OneWayMailContext() : base("OneWayMailContext")
        {
        }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionUsage> SubscriptionUsages { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Domain> AllowedDomains { get; set; }
        public DbSet<EmailAddress> AllowedForwardAddresses { get; set; }
        public DbSet<LogEntry> Log { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain>().ToTable("AllowedDomains");
            modelBuilder.Entity<EmailAddress>().ToTable("AllowedForwardAddresses");
            modelBuilder.Entity<LogEntry>().ToTable("Log");
        }
    }
}
