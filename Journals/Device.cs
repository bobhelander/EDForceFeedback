using System;
using System.Collections.Generic;
using System.Text;

namespace Journals
{
    public class Device
    {
        public string ProductGuid { get; set; }
        public string ProductName { get; set; }
        public bool AutoCenter { get; set; } = true;
        public int ForceFeedbackGain { get; set; } = 10000;
        public List<EventConfiguration> StatusEvents { get; set; } = new List<EventConfiguration>();
    }
}
