using System;
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

        public void UpdateFrame(double time)
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

            var depth = Ship.Position.Y - Ship.Size.Y / 2;
            if (depth < 0)
            {
                var bounceForceCoefficient = 10e6;// [kg/s^2]
                var rebounceForceCoefficient = 10e5; // 20000; // [kg/s^2]
                var coefficient = Ship.Velocity.Y < 0 ? bounceForceCoefficient : rebounceForceCoefficient;
                Ship.ApplyForce(Vector.New(0, -coefficient * depth), Vector.Zero);
            }
            Ship.Update(time);
        }

        private double _destTime = 0;
        private double _currentTime = 0;

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