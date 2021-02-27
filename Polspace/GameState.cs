using Physics;

namespace Polspace
{
    public class GameState
    {
        private readonly World _world;
        public Ship Ship { get; }

        public int Frames => _world.Frames;
        public double GameTime => _world.CurrentTime;

        public GameState()
        {
            _world = new World();
            Ship = new Ship(Vector.New(0, 80));
            _world.AddBody(Ship);
            _world.AddBody(new GroundBody(10e6, 0.3));
            foreach (var shipEngine in Ship.Engines)
                _world.AddEngine(shipEngine);
        }

        private void ApplyEngines(double time)
        {
            var fuelLoss = 0.0;
            foreach (var engine in Ship.Engines)
            {
                if (engine.IsOn)
                    fuelLoss += engine.Type.MaxThrust / engine.Type.SpecificImpulse;
            }

            if (fuelLoss == 0)
                return;
            fuelLoss *= time;
            if (Ship.FuelContainer.Fuel >= fuelLoss)
            {
                Ship.Mass -= fuelLoss;
                Ship.FuelContainer.Fuel -= fuelLoss;
            }
            else
            {
                Ship.Mass -= Ship.FuelContainer.Fuel;
                Ship.FuelContainer.Fuel = 0;
            }

            if (Ship.FuelContainer.Fuel == 0)
            {
                foreach (var engine in Ship.Engines)
                    engine.IsOn = false;
            }
        }

        public void Update(double time)
        {
            ApplyEngines(time);
            _world.Update(time);
        }
    }
}