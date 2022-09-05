using EliteAPI;
using ForceFeedbackSharpDx;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using EliteAPI.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Journals
{
    public class Client
    {
        private IEliteDangerousApi eliteAPI;
        private EliteAPI.Status.Ship.Abstractions.IShip ship;
        private ILogger logger;
        private IHost host;

        private readonly List<DeviceEvents> Devices = new List<DeviceEvents>();

        public async Task Initialize(Settings settings)
        {
            // Inject the logger and EliteApi services into host
            host = Host.CreateDefaultBuilder()
                 .ConfigureServices((context, service) =>
                 {
                     service.AddEliteAPI();
                     service.AddTransient<Client>();
                     service.AddLogging(builder => 
                        builder.AddSimpleConsole(options => { options.SingleLine = true; options.TimestampFormat = "hh:mm:ss "; }).SetMinimumLevel(LogLevel.Debug));
                 })
                 .Build();

            // Get the services from the Host object
            eliteAPI = host.Services.GetService<IEliteDangerousApi>();
            ship = host.Services.GetService<EliteAPI.Status.Ship.Abstractions.IShip>();
            logger = host.Services.GetRequiredService<ILogger<Client>>();

            foreach (var device in settings.Devices)
            {
                var ffDevice = new ForceFeedbackController() { Logger = logger };
                if (ffDevice.Initialize(
                        device.ProductGuid,
                        device.ProductName,
                        @".\Forces",
                        device.AutoCenter,
                        device.ForceFeedbackGain) == false)
                {
                    logger.LogError($"Device Initialization failed: {device.ProductGuid}: {device.ProductName}");
                    continue;
                }

                var deviceEvents = new DeviceEvents
                {
                    EventSettings = device.StatusEvents.ToDictionary(v => v.Event, v => v),
                    Device = ffDevice
                };

                Devices.Add(deviceEvents);
            }

            // Start the api
            await eliteAPI.StartAsync().ConfigureAwait(false);

            eliteAPI.Events.AllEvent += Events_AllEvent;

            ship.Docked.OnChange += (obj, eventArgs) => FindEffect($"Status.Docked:{eventArgs}");
            ship.Landed.OnChange += (obj, eventArgs) => FindEffect($"Status.Landed:{eventArgs}");
            ship.Gear.OnChange += (obj, eventArgs) => FindEffect($"Status.Gear:{eventArgs}");
            ship.Shields.OnChange += (obj, eventArgs) => FindEffect($"Status.Shields:{eventArgs}");
            ship.Supercruise.OnChange += (obj, eventArgs) => FindEffect($"Status.Supercruise:{eventArgs}");
            ship.FlightAssist.OnChange += (obj, eventArgs) => FindEffect($"Status.FlightAssist:{eventArgs}");
            ship.Hardpoints.OnChange += (obj, eventArgs) => FindEffect($"Status.Hardpoints:{eventArgs}");
            ship.Winging.OnChange += (obj, eventArgs) => FindEffect($"Status.Winging:{eventArgs}");
            ship.Lights.OnChange += (obj, eventArgs) => FindEffect($"Status.Lights:{eventArgs}");
            ship.CargoScoop.OnChange += (obj, eventArgs) => FindEffect($"Status.CargoScoop:{eventArgs}");
            ship.SilentRunning.OnChange += (obj, eventArgs) => FindEffect($"Status.SilentRunning:{eventArgs}");
            ship.Scooping.OnChange += (obj, eventArgs) => FindEffect($"Status.Scooping:{eventArgs}");
            ship.SrvHandbreak.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvHandbreak:{eventArgs}");
            ship.SrvTurrent.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvTurrent:{eventArgs}");
            ship.SrvNearShip.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvNearShip:{eventArgs}");
            ship.SrvDriveAssist.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvDriveAssist:{eventArgs}");
            ship.MassLocked.OnChange += (obj, eventArgs) => FindEffect($"Status.MassLocked:{eventArgs}");
            ship.FsdCharging.OnChange += (obj, eventArgs) => FindEffect($"Status.FsdCharging:{eventArgs}");
            ship.FsdCooldown.OnChange += (obj, eventArgs) => FindEffect($"Status.FsdCooldown:{eventArgs}");
            ship.LowFuel.OnChange += (obj, eventArgs) => FindEffect($"Status.LowFuel:{eventArgs}");
            ship.Overheating.OnChange += (obj, eventArgs) => FindEffect($"Status.Overheating:{eventArgs}");
            ship.HasLatLong.OnChange += (obj, eventArgs) => FindEffect($"Status.HasLatLong:{eventArgs}");
            ship.InDanger.OnChange += (obj, eventArgs) => FindEffect($"Status.InDanger:{eventArgs}");
            ship.InInterdiction.OnChange += (obj, eventArgs) => FindEffect($"Status.InInterdiction:{eventArgs}");
            ship.InMothership.OnChange += (obj, eventArgs) => FindEffect($"Status.InMothership:{eventArgs}");
            ship.InFighter.OnChange += (obj, eventArgs) => FindEffect($"Status.InFighter:{eventArgs}");
            ship.InSrv.OnChange += (obj, eventArgs) => FindEffect($"Status.InSrv:{eventArgs}");
            ship.AnalysisMode.OnChange += (obj, eventArgs) => FindEffect($"Status.AnalysisMode:{eventArgs}");
            ship.NightVision.OnChange += (obj, eventArgs) => FindEffect($"Status.NightVision:{eventArgs}");
            ship.AltitudeFromAverageRadius.OnChange += (obj, eventArgs) => FindEffect($"Status.AltitudeFromAverageRadius:{eventArgs}");
            ship.FsdJump.OnChange += (obj, eventArgs) => FindEffect($"Status.FsdJump:{eventArgs}");
            ship.SrvHighBeam.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvHighBeam:{eventArgs}");
        }

        private void Events_AllEvent(object sender, dynamic e)
        {
            var eventKey = e.ToString();
            FindEffect(eventKey);
        }

        private void FindEffect(string eventKey)
        {
            logger.LogDebug($"Event {eventKey}");

            foreach (var device in Devices)
            {
                if (device.EventSettings.ContainsKey(eventKey))
                {
                    var eventConfig = device.EventSettings[eventKey];
                    var result = device.Device?.PlayFileEffect(eventConfig.ForceFile, eventConfig.Duration);
                }
            }
        }
    }
}
