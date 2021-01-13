using Physics;

namespace Polspace
{
    public class Ship : BoxBody
    {
        public FuelContainer FuelContainer { get; }
        public Engine MainEngine { get; }
        public Engine LeftEngine { get; }
        public Engine RightEngine { get; }

        public Ship(Vector position)
            : base(position, Vector.New(2, 5), 0.5, 1200, 100)
        {
            FuelContainer = new FuelContainer(FuelContainerType.Standard);
            MainEngine = new Engine(EngineType.Main);
            LeftEngine = new Engine(EngineType.Side);
            RightEngine = new Engine(EngineType.Side);
        }
    }
}