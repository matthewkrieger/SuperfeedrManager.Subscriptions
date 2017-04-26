using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperfeedrManager.Models
{
    public class Feed
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Subscription
    {
        public string format { get; set; }
        public string endpoint { get; set; }
        public object secret { get; set; }
        public Feed feed { get; set; }
    }

    public class SuperfeedrSubscription
    {
        public Subscription subscription { get; set; }
    }
}
