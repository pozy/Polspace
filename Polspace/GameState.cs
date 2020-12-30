using Math;

namespace Polspace
{
    public class GameState
    {
        public Ship Ship { get; }

        public GameState()
        {
            Ship = new Ship(Vector.New(0, 80));
        }

        public void Update(double time)
        {
            var acceleration = Vector.New(0, -1);
            if (Ship.MainEngine.IsOn)
            {
                var fuelLoss = Ship.MainEngine.Type.MaxThrust / Ship.MainEngine.Type.SpecificImpulse * time;
                var shipAcceleration = Vector.New(0, Ship.MainEngine.Type.MaxThrust) / Ship.Mass;
                if (Ship.FuelContainer.Fuel >= fuelLoss)
                    Ship.FuelContainer.Fuel -= fuelLoss;
                else
                {
                    Ship.FuelContainer.Fuel = 0;
                    shipAcceleration *= Ship.FuelContainer.Fuel / fuelLoss;
                    Ship.MainEngine.IsOn = false;
                }
                acceleration += shipAcceleration;
            }

            Ship.Velocity += acceleration * time;
            Ship.Position += Ship.Velocity * time;
            if (Ship.Position.Y - Ship.Size.Y / 2 < 0)
            {
                Ship.Position = Vector.New(Ship.Position.X, Ship.Size.Y / 2);
                if (System.Math.Abs(Ship.Velocity.Y) > 2)
                    Ship.IsDestroyed = true;
                Ship.Velocity = Vector.Zero;
            }
        }
    }
}