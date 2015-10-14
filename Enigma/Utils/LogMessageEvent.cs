using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enigma.Utils
{
    public class LogMessageEvent
    {

        public string InstanceId { get; set; }

        public string MachineName { get; set; }

        public string SiteName { get; set; }

        public string Value { get; set; }
    }
}
