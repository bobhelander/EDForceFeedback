using EliteAPI.Status;
using EliteAPI.Status.Abstractions;
using EliteAPI.Status.Ship;
using EliteAPI.Status.Ship.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForceFeedback
{
    public class EliteShipMock : EliteStatusMock, IShip
    {
        /// <inheritdoc />
        public StatusProperty<bool> Available { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Docked { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Landed { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Gear { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Shields { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Supercruise { get; } = new StatusProperty<bool>(false);
        /// <inheritdoc />
        public StatusProperty<bool> FlightAssist { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Hardpoints { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Winging { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Lights { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> CargoScoop { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> SilentRunning { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Scooping { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> SrvHandbreak { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> SrvTurrent { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> SrvNearShip { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> SrvDriveAssist { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> MassLocked { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> FsdCharging { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> FsdCooldown { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> LowFuel { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> Overheating { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> HasLatLong { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> InDanger { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> InInterdiction { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> InMothership { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> InFighter { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> InSrv { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> AnalysisMode { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> NightVision { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> AltitudeFromAverageRadius { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> FsdJump { get; } = new StatusProperty<bool>(false);

        /// <inheritdoc />
        public StatusProperty<bool> SrvHighBeam { get; } = new StatusProperty<bool>(false);

        public StatusProperty<ShipFlags> Flags => throw new NotImplementedException();

        public StatusProperty<ShipPips> Pips => throw new NotImplementedException();

        public StatusProperty<int> FireGroup => throw new NotImplementedException();

        public StatusProperty<ShipGuiFocus> GuiFocus => throw new NotImplementedException();

        public StatusProperty<ShipFuel> Fuel => throw new NotImplementedException();

        public StatusProperty<int> Cargo => throw new NotImplementedException();

        public StatusProperty<ShipLegalState> LegalState => throw new NotImplementedException();

        public StatusProperty<float> Latitude => throw new NotImplementedException();

        public StatusProperty<float> Altitude => throw new NotImplementedException();

        public StatusProperty<float> Longitude => throw new NotImplementedException();

        public StatusProperty<float> Heading => throw new NotImplementedException();

        public StatusProperty<string> Body => throw new NotImplementedException();

        public StatusProperty<float> BodyRadius => throw new NotImplementedException();

        /// <inheritdoc />
    }
}
