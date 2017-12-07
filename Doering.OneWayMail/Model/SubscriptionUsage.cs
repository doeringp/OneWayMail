using System;

namespace Doering.OneWayMail.Model
{
    public class SubscriptionUsage
    {
        public Guid Id { get; set; }

        public Guid SubscriptionId { get; set; }

        public DateTime Time { get; set; }

        public virtual Subscription Subscription { get; set; }
    }
}
