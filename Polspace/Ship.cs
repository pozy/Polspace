using Math;

namespace Polspace
{
    public class Ship : PhysicalPart
    {
        public override double Mass => FuelContainer.Mass + MainEngine.Type.Mass + LeftEngine.Type.Mass + RightEngine.Type.Mass + 1000; // [kg]
        public FuelContainer FuelContainer { get; }
        public Engine MainEngine { get; }
        public Engine LeftEngine { get; }
        public Engine RightEngine { get; }

        public Ship(Vector position)
        {
            Position = position;
            Velocity = Vector.Zero;
            Size = Vector.New(2, 5);
            FuelContainer = new FuelContainer(FuelContainerType.Standard);
            MainEngine = new Engine(EngineType.Main);
            LeftEngine = new Engine(EngineType.Side);
            RightEngine = new Engine(EngineType.Side);
        }
    }
}