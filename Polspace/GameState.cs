using Math;

namespace Polspace
{
    public class GameState
    {
        public static Vector Gravity => Vector.New(0, -1);

        public Ship Ship { get; }

        public GameState()
        {
            Ship = new Ship(Vector.New(0, 80));
        }

        public void Update(double time)
        {
            Ship.ApplyForce(Gravity * Ship.Mass, Vector.Zero);
            if (Ship.MainEngine.IsOn)
            {
                var fuelLoss = Ship.MainEngine.Type.MaxThrust / Ship.MainEngine.Type.SpecificImpulse * time;
                var engineForce = Ship.MainEngine.Type.MaxThrust;
                if (Ship.FuelContainer.Fuel >= fuelLoss)
                    Ship.FuelContainer.Fuel -= fuelLoss;
                else
                {
                    Ship.FuelContainer.Fuel = 0;
                    engineForce *= Ship.FuelContainer.Fuel / fuelLoss;
                    Ship.MainEngine.IsOn = false;
                }

                Ship.ApplyForce(new Vector(0, engineForce), Vector.Zero);
            }

            Ship.Update(time);
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