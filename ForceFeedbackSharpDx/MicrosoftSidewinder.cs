using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;
using System.Linq;

namespace ForceFeedbackSharpDx
{
    public class MicrosoftSidewinder
    {
        // Microsoft Force Feedback 2
        //private static Guid product = new Guid("001b045e-0000-0000-0000-504944564944");

        private Joystick joystick = null;
        private readonly Dictionary<string, EffectInfo> knownEffects = new Dictionary<string, EffectInfo>();
        private readonly Dictionary<string, List<EffectFile>> fileEffects = new Dictionary<string, List<EffectFile>>();

        public bool ForceFeedback2(string productGuid, string productName, bool AutoCenter, int ForceFeedbackGain)
        {
            // Initialize DirectInput
            var directInput = new DirectInput();

            var product = new Guid(productGuid);

            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;
            var joystickName = String.Empty;

            var directInputDevices = new List<DeviceInstance>();

            directInputDevices.AddRange(directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices));
            directInputDevices.AddRange(directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices));
            directInputDevices.AddRange(directInput.GetDevices(DeviceType.Driving, DeviceEnumerationFlags.AllDevices));

            foreach (var deviceInstance in directInputDevices)
            {
                Console.WriteLine($"DeviceName: {deviceInstance.ProductName}: ProductGuid {deviceInstance.ProductGuid}");

                if (deviceInstance.ProductGuid == product || deviceInstance.ProductName == productName)
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                    joystickName = deviceInstance.ProductName;
                    break;
                }
            }

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No matching Joystick/Gamepad/Wheel found. {deviceInstance.ProductName} {productGuid}");
                return false;
            }

            // Instantiate the joystick
            joystick = new Joystick(directInput, joystickGuid);

            Console.WriteLine("Found Joystick/Gamepad {0}", joystickName);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
            {
                knownEffects.Add(effectInfo.Name, effectInfo);
                Console.WriteLine("Effect available {0}", effectInfo.Name);
            }

            // Load all of the effect files
            var forcesFolder = new DirectoryInfo(@".\Forces");

            foreach (var file in forcesFolder.GetFiles("*.ffe"))
            {
                var effectsFromFile = joystick.GetEffectsInFile(file.FullName, EffectFileFlags.ModidyIfNeeded);
                fileEffects.Add(file.Name, new List<EffectFile>(effectsFromFile));
                Console.WriteLine($"File Effect available {file.Name}");
            }

            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // DirectX requires a window handle to set the CooperativeLevel
            var handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

            joystick.SetCooperativeLevel(handle, CooperativeLevel.Exclusive | CooperativeLevel.Background);

            // Autocenter on
            joystick.Properties.AutoCenter = AutoCenter;

            // Acquire the joystick
            joystick.Acquire();

            //var test = joystick.Properties.ForceFeedbackGain;

            joystick.Properties.ForceFeedbackGain = ForceFeedbackGain;

            return true;
        }

        private void PlayEffect(Effect effect)
        {
            // See if our effects are playing.  If not play them
            if (effect.Status != EffectStatus.Playing)
            {
                Console.WriteLine($"Effect {effect.Guid} starting");
                effect.Start(1, EffectPlayFlags.NoDownload);
            }
        }

        private void PlayEffects(IEnumerable<Effect> effects)
        {
            foreach (var effect in effects)
                PlayEffect(effect);
        }

        private void StopEffects(IEnumerable<Effect> effects)
        {
            foreach (var effect in effects)
                effect.Stop();

            foreach (var effect in effects)
                effect.Dispose();

            Reset();
        }

        private void Reset()
        {
            joystick.SendForceFeedbackCommand(ForceFeedbackCommand.Reset);
        }

        public void PlayFileEffect(string name, int duration = 250)
        {
            try
            {
                var fileEffect = fileEffects[name];

                _ = Task.Run(async () =>
                {
                    // Create a new list of effects
                    var forceEffects = fileEffect.Select(x => new Effect(joystick, x.Guid, x.Parameters)).ToList();
                    PlayEffects(forceEffects);
                    await Task.Delay(duration).ConfigureAwait(false);
                    StopEffects(forceEffects);
                }).ContinueWith(t =>
                {
                    if (t.IsCanceled) Console.WriteLine($"Effect {name} cancelled");
                    else if (t.IsFaulted) Console.WriteLine($"Effect {name} Exception {t.Exception.InnerException?.Message}");
                    else Console.WriteLine($"Effect {name} complete");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.Message}");
            }
        }
    }
}
