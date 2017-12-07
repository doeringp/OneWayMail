using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doering.OneWayMail.Model
{
    public class LogEntry
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string CallSite { get; set; }

        public string Exception { get; set; }
    }
}
