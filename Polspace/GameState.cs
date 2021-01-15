using System;
using Physics;

namespace Polspace
{
    public class GameState
    {
        public int Frames { get; private set; }
        public static Vector Gravity => Vector.New(0, -1);

        public Ship Ship { get; }
        public GroundBody Ground { get; }

        private readonly Engine[] _engines;

        public GameState()
        {
            Ship = new Ship(Vector.New(0, 80));
            Ground = new GroundBody();
            _engines = new[] {Ship.MainEngine, Ship.RightEngine, Ship.LeftEngine};
        }

        private void UpdateFrame(double time)
        {
            Frames++;
            Ship.ApplyForce(Gravity * Ship.Mass);
            ApplyEngines(time, _engines);
            foreach (var point in Ship.Points)
            {
                var toOutside = Ground.GetShortestVectorToOutside(point + Ship.Position);
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

        private void ApplyEngines(double time, ReadOnlySpan<Engine> engines)
        {
            var engineForceMultiplier = 1.0;
            var fuelLoss = 0.0;
            foreach (var engine in engines)
            {
                if (engine.IsOn)
                    fuelLoss += Ship.MainEngine.Type.MaxThrust / Ship.MainEngine.Type.SpecificImpulse;
            }

            if (fuelLoss == 0)
                return;
            fuelLoss *= time;
            if (Ship.FuelContainer.Fuel >= fuelLoss)
            {
                Ship.FuelContainer.Fuel -= fuelLoss;
                Ship.Mass -= fuelLoss;
            }
            else
            {
                Ship.Mass -= Ship.FuelContainer.Fuel;
                Ship.FuelContainer.Fuel = 0;
                engineForceMultiplier = Ship.FuelContainer.Fuel / fuelLoss;
            }

            var rotated = Vector.NewRotated(Ship.Angle);
            if (Ship.MainEngine.IsOn)
                Ship.ApplyForce(Ship.MainEngine.Type.MaxThrust * engineForceMultiplier * rotated,
                    Vector.Zero);
            if (Ship.RightEngine.IsOn)
                Ship.ApplyForce(
                    Ship.RightEngine.Type.MaxThrust * engineForceMultiplier * Vector.New(rotated.Y, -rotated.X),
                    Ship.Points[0] - rotated);
            if (Ship.LeftEngine.IsOn)
                Ship.ApplyForce(
                    Ship.LeftEngine.Type.MaxThrust * engineForceMultiplier * Vector.New(-rotated.Y, -rotated.X),
                    Ship.Points[3] - rotated);

            if (Ship.FuelContainer.Fuel == 0)
                foreach (var engine in engines)
                    engine.IsOn = false;
        }

        private double _destTime;
        private double _currentTime;

        public void Update(double time)
        {
            _destTime += time;
            var velocity = Ship.Velocity.GetLength();
            var distance = Math.Abs(Ship.Position.Y);
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
            var frameDuration = 1.0 / Math.Pow(2.0, frameTimePrecision);
            return frameDuration;
        }
    }
}