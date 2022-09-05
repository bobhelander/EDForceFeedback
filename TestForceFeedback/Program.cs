using ForceFeedbackSharpDx;
using System;
using System.Collections.Generic;
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
        
        static void Main(string[] args)
        {
            var msffb2 = new ForceFeedbackController();
            msffb2.Initialize( feedbackGuid, feedbackProduct, @".\Forces", false, 10000);

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
            var vibrate = msffb2.PlayFileEffect("VibrateSide.ffe" ,-1);

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
    }
}
