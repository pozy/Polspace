using Physics;

namespace Polspace
{
    public class Engine : Physics.Engine
    {
        private bool _isOn;
        public EngineType Type { get; }

        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                if (ConnectedShip.FuelContainer.Fuel == 0 || ConnectedShip.IsDestroyed)
                    _isOn = false;
            }
        }

        public Ship ConnectedShip { get; }

        /// <inheritdoc />
        public override double Force => IsOn ? Type.MaxThrust : 0;

        public Engine(EngineType type,
            DynamicBody attachedTo,
            Vector attachmentPoint,
            double attachmentAngle,
            Ship connectedShip)
            : base(attachedTo, attachmentPoint, attachmentAngle)
        {
            Type = type;
            ConnectedShip = connectedShip;
            IsOn = false;
        }
    }
}