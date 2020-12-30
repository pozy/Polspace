using Math;

namespace Polspace
{
    public class Ship
    {
        public bool IsDestroyed { get; set; }
        public double Mass => FuelContainer.Mass + MainEngine.Type.Mass + 1000; // [kg]
        public Vector Size { get; } // [m]
        public Vector Position { get; internal set; }
        public Vector Velocity { get; internal set;} // [m/s^2]
        public double Rotation { get; internal set;} // [r]
        public double RotationVelocity { get; internal set;} // [r/s]
        public FuelContainer FuelContainer { get; }
        public Engine MainEngine { get; }

        public Ship(Vector position)
        {
            Position = position;
            Velocity = Vector.Zero;
            Size = Vector.New(2, 5);
            FuelContainer = new FuelContainer(FuelContainerType.Standard);
            MainEngine = new Engine(EngineType.Main);
        }
    }
}