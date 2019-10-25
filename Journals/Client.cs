using EliteAPI;
using ForceFeedbackSharpDx;
using Somfic.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq;

namespace Journals
{
    public class Client
    {
        private MicrosoftSidewinder msffb2;
        private EliteDangerousAPI eliteAPI;

        private Dictionary<string, EventConfiguration> eventSettings = new Dictionary<string, EventConfiguration>();

        public void Initialize(Settings settings)
        {
            this.eventSettings = settings.StatusEvents.ToDictionary(v => v.Event, v => v);

            eliteAPI = new EliteDangerousAPI();
            eliteAPI.Logger.UseConsole(Severity.Info);
            eliteAPI.Start();

            msffb2 = new MicrosoftSidewinder();
            msffb2.ForceFeedback2();

            eliteAPI.Events.AllEvent += Events_AllEvent;
        }

        private void Events_AllEvent(object sender, dynamic e)
        {
            if (e is EliteAPI.Events.StatusEvent)
            {
                var statusEvent = e as EliteAPI.Events.StatusEvent;
                var key = $"{statusEvent.Event}:{statusEvent.Value}";
                Console.WriteLine($"StatusEvent {key}");
                if (eventSettings.ContainsKey(key))
                {
                    var eventConfig = eventSettings[key];
                    msffb2?.PlayFileEffect(eventConfig.ForceFile, eventConfig.Duration);
                }
            }
            else
            {
                Console.WriteLine($"Event {e}");
            }
        }
    }
}
