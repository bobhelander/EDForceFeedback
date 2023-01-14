using EliteAPI.Abstractions;
using EliteAPI.Events.Status.Ship.Events;
using ForceFeedbackSharpDx;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpDX.DirectInput;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Journals
{
    public class Client
    {
        private IEliteDangerousApi eliteDangerousApi;
        private ILogger logger;
        private IHost host;

        private readonly List<DeviceEvents> Devices = new List<DeviceEvents>();

        public string StatusText { get; set; } = "Not Initialized";

        public async Task Initialize(Settings settings)
        {
            // Inject the logger and EliteApi services into host
            host = Host.CreateDefaultBuilder()
                 .ConfigureServices((context, service) =>
                 {
                     service.AddEliteApi();
                     service.AddTransient<Client>();
                     service.AddLogging(builder => 
                        builder.AddSimpleConsole(options => { options.SingleLine = true; options.TimestampFormat = "hh:mm:ss "; }).SetMinimumLevel(LogLevel.Debug));
                 })
                 .Build();

            // Get the services from the Host object
            eliteDangerousApi = host.Services.GetRequiredService<IEliteDangerousApi>();
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
            await eliteDangerousApi.StartAsync().ConfigureAwait(false);

            eliteDangerousApi.Events.OnAny(e => Events_AllEvent(this, e));

            eliteDangerousApi.Events.On<DockedStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<LandedStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<GearStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<ShieldsStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<SupercruiseStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<FlightAssistStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<HardpointsStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<WingingStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<LightsStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<CargoScoopStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<SilentRunningStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<ScoopingStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<SrvHandbrakeStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<SrvTurretStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<SrvNearShipStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<SrvDriveAssistStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<MassLockedStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<FsdChargingStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<FsdCooldownStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<LowFuelStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<OverheatingStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<HasLatLongStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<InDangerStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<InInterdictionStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<InMothershipStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<InFighterStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<InSrvStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<AnalysisModeStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<NightVisionStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<AltitudeFromAverageRadiusStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<FsdJumpStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));
            eliteDangerousApi.Events.On<SrvHighBeamStatusEvent>(e => FindEffect($"Status.{e.Event}:{e.Value}"));

            StatusText = $"{Devices.FirstOrDefault()?.Device?.StatusText ?? "Not Connected"}";
        }

        public void TestEffect()
        {
            var device = Devices.FirstOrDefault();
            var effect = device?.EventSettings.FirstOrDefault();

            if (device != null && effect != null && effect.HasValue)
            {
                var result = device.Device?.PlayFileEffect(effect.Value.Value.ForceFile, effect.Value.Value.Duration);
            }
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
