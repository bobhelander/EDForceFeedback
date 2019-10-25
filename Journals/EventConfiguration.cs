using System;
using System.Collections.Generic;
using System.Text;

namespace Journals
{
    public class EventConfiguration
    {
        public string Event { get; set; }
        public string ForceFile { get; set; }
        public int Duration { get; set; } = 250;
    }
}
