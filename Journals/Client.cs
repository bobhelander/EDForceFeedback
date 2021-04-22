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

        private EliteDangerousAPI eliteAPI;

        private readonly List<DeviceEvents> Devices = new List<DeviceEvents>();

        public void Initialize(Settings settings)
        {
            // Initialize first. We are outputing to the console set up in this API
            eliteAPI = new EliteDangerousAPI();
            //eliteAPI.Logger.UseConsole(Severity.Info);

            foreach (var device in settings.Devices)
            {
                var ffDevice = new ForceFeedbackController();
                if (ffDevice.Initialize(
                    device.ProductGuid,
                    device.ProductName,
                    @".\Forces",
                    device.AutoCenter,
                    device.ForceFeedbackGain) == false)
                    continue;

                var deviceEvents = new DeviceEvents
                {
                    EventSettings = device.StatusEvents.ToDictionary(v => v.Event, v => v),
                    Device = ffDevice
                };

                Devices.Add(deviceEvents);
            }

            eliteAPI.Start();

            eliteAPI.Events.AllEvent += Events_AllEvent;
        }

        private void Events_AllEvent(object sender, dynamic e)
        {
            if (e is EliteAPI.Events.StatusEvent)
            {
                var statusEvent = e as EliteAPI.Events.StatusEvent;
                var key = $"{statusEvent.Event}:{statusEvent.Value}";
                Console.WriteLine($"StatusEvent {key}");
                foreach (var device in Devices)
                {
                    if (device.EventSettings.ContainsKey(key))
                    {
                        var eventConfig = device.EventSettings[key];
                        var test = device.Device?.PlayFileEffect(eventConfig.ForceFile, eventConfig.Duration);
                    }
                }
            }
            else
            {
                Console.WriteLine($"Event {e}");
            }
        }
    }
}
