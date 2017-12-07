using System;
using System.Collections.Generic;

namespace Doering.OneWayMail.Model
{
    public class Session
    {
        public Guid Id { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
