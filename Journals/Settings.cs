using System;
using System.Collections.Generic;
using System.Text;

namespace Journals
{
    public class Settings
    {
        public string ProductGuid { get; set; }
        public List<EventConfiguration> StatusEvents { get; set; } = new List<EventConfiguration>();
    }
}
