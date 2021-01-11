using Physics;

namespace Polspace
{
    public class GameState
    {
        public int Frames { get; private set; }
        public static Vector Gravity => Vector.New(0, -1);

        public Ship Ship { get; }
        public GroundBody Ground { get; }

        public GameState()
        {
            Ship = new Ship(Vector.New(0, 80));
            Ground = new GroundBody();
        }

        public void UpdateFrame(double time)
        {
            Frames++;
            Ship.ApplyForce(Gravity * Ship.Mass);
            if (Ship.MainEngine.IsOn)
            {
                var fuelLoss = Ship.MainEngine.Type.MaxThrust / Ship.MainEngine.Type.SpecificImpulse * time;
                var engineForce = Ship.MainEngine.Type.MaxThrust;
                if (Ship.FuelContainer.Fuel >= fuelLoss)
                {
                    Ship.FuelContainer.Fuel -= fuelLoss;
                    Ship.Mass -= fuelLoss;
                }
                else
                {
                    Ship.Mass -= Ship.FuelContainer.Fuel;
                    Ship.FuelContainer.Fuel = 0;
                    engineForce *= Ship.FuelContainer.Fuel / fuelLoss;
                    Ship.MainEngine.IsOn = false;
                }

                Ship.ApplyForce(new Vector(0, engineForce), Vector.Zero);
            }

            foreach (var point in Ship.Points)
            {
                var toOutside = Ground.GetShortestVectorToOutside(point);
                if (toOutside != Vector.Zero)
                {
                    var bounceForceCoefficient = 10e6; // [kg/s^2]
                    var rebounceForceCoefficient = 10e5; // [kg/s^2]
                    var isPointGoingInside = Vector.Dot(toOutside, Ship.Velocity) < 0;
                    var coefficient = isPointGoingInside ? bounceForceCoefficient : rebounceForceCoefficient;
                    Ship.ApplyForce(toOutside * coefficient, point);
                }
            }

            Ship.Update(time);
        }

        private double _destTime;
        private double _currentTime;

        public void Update(double time)
        {
            _destTime += time;
            var velocity = Ship.Velocity.GetLength();
            var distance = System.Math.Abs(Ship.Position.Y);
            var frameDuration = CalculateFrameDuration(distance, velocity);
            while (_currentTime < _destTime)
            {
                UpdateFrame(frameDuration);
                _currentTime += frameDuration;
            }
        }

        private static double CalculateFrameDuration(double distance, double velocity)
        {
            int frameTimePrecision;
            if (distance > 1_000_000)
                frameTimePrecision = 0;
            else if (distance > 1000)
                frameTimePrecision = 10;
            else if (velocity < 1000)
                frameTimePrecision = 20;
            else
                frameTimePrecision = 27;
            var frameDuration = 1.0 / System.Math.Pow(2.0, frameTimePrecision);
            return frameDuration;
        }
    }
}