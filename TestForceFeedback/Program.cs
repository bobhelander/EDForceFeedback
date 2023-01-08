using EliteAPI.Abstractions;
using ForceFeedbackSharpDx;
using Journals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForceFeedback
{
    internal static class Program
    {
        //static string feedbackGuid = "001b045e-0000-0000-0000-504944564944";  // MS ForceFeedBack 2
        //static string feedbackProduct = "SideWinder Force Feedback 2 Joystick"; // MS ForceFeedBack 2

        static string feedbackGuid = "02dd045e-0000-0000-0000-504944564944";  // XBOX One
        static string feedbackProduct = "Controller (Xbox One For Windows)";  // XBOX One

        static void Main_bak(string[] args)
        {
            var msffb2 = new ForceFeedbackController();
            msffb2.Initialize(feedbackGuid, feedbackProduct, @".\Forces", false, 10000);

            Console.ReadKey();

            Console.WriteLine("Damper.ffe");
            var effects = msffb2.PlayFileEffect("Damper.ffe", -1);

            Console.ReadKey();

            Console.WriteLine("SetActuators(false)");
            msffb2.SetActuators(false);

            Console.ReadKey();

            Console.WriteLine("SetActuators(true)");
            msffb2.SetActuators(true);

            Console.ReadKey();

            Console.WriteLine("VibrateSide.ffe");
            var vibrate = msffb2.PlayFileEffect("VibrateSide.ffe", -1);

            Console.ReadKey();

            Console.WriteLine("StopEffects");
            msffb2.StopEffects(effects);
            msffb2.StopEffects(vibrate);

            Console.ReadKey();

            Console.WriteLine("CenterSpringXY.ffe");
            var centerEffects = msffb2.PlayFileEffect("CenterSpringXY.ffe", -1);

            Console.ReadKey();

            if (centerEffects != null)
            {
                Console.WriteLine("Gain = 5000");
                var effectParams = centerEffects[0].GetParameters(SharpDX.DirectInput.EffectParameterFlags.Gain);

                effectParams.Gain = 5000;

                centerEffects[0].SetParameters(effectParams, SharpDX.DirectInput.EffectParameterFlags.Gain | SharpDX.DirectInput.EffectParameterFlags.NoRestart);

                Console.ReadKey();

                Console.WriteLine("Gain = 10000");
                effectParams.Gain = 10000;

                centerEffects[0].SetParameters(effectParams, SharpDX.DirectInput.EffectParameterFlags.Gain | SharpDX.DirectInput.EffectParameterFlags.NoRestart);

                Console.ReadKey();
            }

            Console.WriteLine("StopEffects(centerEffects)");
            msffb2.StopEffects(centerEffects);

            Console.ReadKey();
            /*
            msffb2.PlayFileEffect("VibrateSide.ffe");
            //msffb2.PlayFileEffect("VibrateSide.ffe");
           // msffb2.PlayFileEffect("VibrateSide.ffe");

            Console.ReadKey();

            msffb2.PlayFileEffect("Landed.ffe");

            Console.ReadKey();

            msffb2.PlayFileEffect("Gear.ffe");

            //msffb2.CenterOff();

            Console.ReadKey();
            */
        }

        public static IServiceCollection AddEliteAPI(this IServiceCollection services)
        {
            // EliteAPI
            services.AddSingleton<IEliteDangerousApi, EliteAPIMock>();
            services.AddSingleton<EliteAPI.Status.Ship.Abstractions.IShip, EliteShipMock>();
            return services;
        }


        static void Main(string[] args)
        {
            var fileName = $"{Directory.GetCurrentDirectory()}\\settings.json";

            // Check if a settings file was specified
            if (args?.Length == 1)
            {
                if (args[0].CompareTo("-h") == 0 || args[0].CompareTo("help") == 0)
                {
                    Console.WriteLine("EDForceFeedBack: EDForceFeedback.exe is a console program that runs during a Elite Dangerous session.");
                    Console.WriteLine("It watches the ED log files and responds to game events by playing a force feedback editor (.ffe) file.");
                    Console.WriteLine();
                    Console.WriteLine("Usage:");
                    Console.WriteLine("EDForceFeedback.exe -h                   Output this help.");
                    Console.WriteLine(@"EDForceFeedback.exe c:\settings.json    Override the default settings file and use this instead.");
                    Console.WriteLine($"EDForceFeedback.exe                     Will default to the settings file {Directory.GetCurrentDirectory()}\\settings.json");
                    return;
                }
                else
                {
                    fileName = args[0];
                }
            }

            Console.WriteLine($"Using setting file: {fileName}");

            var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName));

            IEliteDangerousApi eliteAPI;
            EliteAPI.Status.Ship.Abstractions.IShip ship;
            ILogger logger;
            IHost host;

            List<DeviceEvents> Devices = new List<DeviceEvents>();

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
            //await eliteAPI.StartAsync().ConfigureAwait(false);

            //eliteAPI.Events.AllEvent += Events_AllEvent;

            ship.Docked.OnChange += (obj, eventArgs) => FindEffect($"Status.Docked:{eventArgs}", Devices);
            ship.Landed.OnChange += (obj, eventArgs) => FindEffect($"Status.Landed:{eventArgs}", Devices);
            ship.Gear.OnChange += (obj, eventArgs) => FindEffect($"Status.Gear:{eventArgs}", Devices);
            ship.Shields.OnChange += (obj, eventArgs) => FindEffect($"Status.Shields:{eventArgs}", Devices);
            ship.Supercruise.OnChange += (obj, eventArgs) => FindEffect($"Status.Supercruise:{eventArgs}", Devices);
            ship.FlightAssist.OnChange += (obj, eventArgs) => FindEffect($"Status.FlightAssist:{eventArgs}", Devices);
            ship.Hardpoints.OnChange += (obj, eventArgs) => FindEffect($"Status.Hardpoints:{eventArgs}", Devices);
            ship.Winging.OnChange += (obj, eventArgs) => FindEffect($"Status.Winging:{eventArgs}", Devices);
            ship.Lights.OnChange += (obj, eventArgs) => FindEffect($"Status.Lights:{eventArgs}", Devices);
            ship.CargoScoop.OnChange += (obj, eventArgs) => FindEffect($"Status.CargoScoop:{eventArgs}", Devices);
            ship.SilentRunning.OnChange += (obj, eventArgs) => FindEffect($"Status.SilentRunning:{eventArgs}", Devices);
            ship.Scooping.OnChange += (obj, eventArgs) => FindEffect($"Status.Scooping:{eventArgs}", Devices);
            ship.SrvHandbreak.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvHandbreak:{eventArgs}", Devices);
            ship.SrvTurrent.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvTurrent:{eventArgs}", Devices);
            ship.SrvNearShip.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvNearShip:{eventArgs}", Devices);
            ship.SrvDriveAssist.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvDriveAssist:{eventArgs}", Devices);
            ship.MassLocked.OnChange += (obj, eventArgs) => FindEffect($"Status.MassLocked:{eventArgs}", Devices);
            ship.FsdCharging.OnChange += (obj, eventArgs) => FindEffect($"Status.FsdCharging:{eventArgs}", Devices);
            ship.FsdCooldown.OnChange += (obj, eventArgs) => FindEffect($"Status.FsdCooldown:{eventArgs}", Devices);
            ship.LowFuel.OnChange += (obj, eventArgs) => FindEffect($"Status.LowFuel:{eventArgs}", Devices);
            ship.Overheating.OnChange += (obj, eventArgs) => FindEffect($"Status.Overheating:{eventArgs}", Devices);
            ship.HasLatLong.OnChange += (obj, eventArgs) => FindEffect($"Status.HasLatLong:{eventArgs}", Devices);
            ship.InDanger.OnChange += (obj, eventArgs) => FindEffect($"Status.InDanger:{eventArgs}", Devices);
            ship.InInterdiction.OnChange += (obj, eventArgs) => FindEffect($"Status.InInterdiction:{eventArgs}", Devices);
            ship.InMothership.OnChange += (obj, eventArgs) => FindEffect($"Status.InMothership:{eventArgs}", Devices);
            ship.InFighter.OnChange += (obj, eventArgs) => FindEffect($"Status.InFighter:{eventArgs}", Devices);
            ship.InSrv.OnChange += (obj, eventArgs) => FindEffect($"Status.InSrv:{eventArgs}", Devices);
            ship.AnalysisMode.OnChange += (obj, eventArgs) => FindEffect($"Status.AnalysisMode:{eventArgs}", Devices);
            ship.NightVision.OnChange += (obj, eventArgs) => FindEffect($"Status.NightVision:{eventArgs}", Devices);
            ship.AltitudeFromAverageRadius.OnChange += (obj, eventArgs) => FindEffect($"Status.AltitudeFromAverageRadius:{eventArgs}", Devices);
            ship.FsdJump.OnChange += (obj, eventArgs) => FindEffect($"Status.FsdJump:{eventArgs}", Devices);
            ship.SrvHighBeam.OnChange += (obj, eventArgs) => FindEffect($"Status.SrvHighBeam:{eventArgs}", Devices);

            while (true)
            {
                Console.WriteLine("0: Quit");
                Console.WriteLine("1: Docked (true)");
                Console.WriteLine("2: Landed (true)");
                Console.WriteLine("3: Gear (deployed)");
                Console.WriteLine("4: Gear (retracted)");
                Console.WriteLine("5: Hardpoints (deployed)");
                Console.WriteLine("6: Hardpoints (retracted)");
                Console.WriteLine("7: Overheating");

                var key = Console.ReadKey();
                if (key.KeyChar == '0')
                    break;
                switch (key.KeyChar)
                {
                    case '1':
                        ship.Docked.OnChange.Invoke(null, true);
                        break;
                    case '2':
                        ship.Landed.OnChange.Invoke(null, true);
                        break;
                    case '3':
                        ship.Gear.OnChange.Invoke(null, true);
                        break;
                    case '4':
                        ship.Gear.OnChange.Invoke(null, false);
                        break;
                    case '5':
                        ship.Hardpoints.OnChange.Invoke(null, true);
                        break;
                    case '6':
                        ship.Hardpoints.OnChange.Invoke(null, false);
                        break;
                    case '7':
                        ship.Overheating.OnChange.Invoke(null, true);
                        break;
                }
            }
        }

        //private static void Events_AllEvent(object sender, dynamic e)
        //{
        //    var eventKey = e.ToString();
        //    FindEffect(eventKey);
        //}

        private static void FindEffect(string eventKey, IEnumerable<DeviceEvents> Devices)
        {
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
