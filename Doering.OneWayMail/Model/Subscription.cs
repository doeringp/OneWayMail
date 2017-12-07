using System;
using System.Collections.Generic;

namespace Doering.OneWayMail.Model
{
    public class Subscription
    {
        public Guid Id { get; set; }

        public string EmailAddress { get; set; }

        public string ForwardTo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ValidUntil { get; set; }

        public bool Enabled { get; set; }

        public DateTime? DisabledAt { get; set; }

        public string IpAddress { get; set; }

        public string UserAgent { get; set; }

        public Guid? SessionId { get; set; }

        public virtual ICollection<SubscriptionUsage> Usages { get; set; } = new List<SubscriptionUsage>();
    }
}
