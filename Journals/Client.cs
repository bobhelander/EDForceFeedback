using EliteAPI;
using ForceFeedbackSharpDx;
using Somfic.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journals
{
    public class Client
    {
        private MicrosoftSidewinder msffb2;

        public void Initialize()
        {
            msffb2 = new MicrosoftSidewinder();
            msffb2.ForceFeedback2();

            var EliteAPI = new EliteDangerousAPI();
            EliteAPI.Logger.UseConsole(Severity.Info);
            EliteAPI.Start();

            EliteAPI.Events.StatusGearEvent += Events_StatusGearEvent;
            EliteAPI.Events.StatusDockedEvent += Events_StatusDockedEvent;
            EliteAPI.Events.StatusHardpointsEvent += Events_StatusHardpointsEvent;
            EliteAPI.Events.StatusLandedEvent += Events_StatusLandedEvent;
            EliteAPI.Events.StatusLightsEvent += Events_StatusLightsEvent;
            EliteAPI.Events.StatusLowFuelEvent += Events_StatusLowFuelEvent;
            EliteAPI.Events.StatusCargoScoopEvent += Events_StatusCargoScoopEvent;
            EliteAPI.Events.StatusOverheatingEvent += Events_StatusOverheatingEvent;
        }

        private void Events_StatusOverheatingEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("VibrateSide.ffe", 500);
        }

        private void Events_StatusCargoScoopEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("Cargo.ffe", 2000);
        }

        private void Events_StatusLowFuelEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("VibrateSide.ffe", 500);
        }

        private void Events_StatusLightsEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("Vibrate.ffe", 250);
        }

        private void Events_StatusLandedEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("Landed.ffe", 1500);
        }

        private void Events_StatusHardpointsEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("Hardpoints.ffe", 2000);
        }

        private void Events_StatusDockedEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("Dock.ffe", 2000);
        }

        private void Events_StatusGearEvent(object sender, EliteAPI.Events.StatusEvent e)
        {
            msffb2.PlayFileEffect("Gear.ffe", 3000);
        }
    }
}
