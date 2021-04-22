using ForceFeedbackSharpDx;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journals
{
    public class DeviceEvents
    {
        public ForceFeedbackController Device { get; set; }
        public Dictionary<string, EventConfiguration> EventSettings { get; set; } = new Dictionary<string, EventConfiguration>();
    }
}
