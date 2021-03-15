using System;
using System.Collections.Generic;
using Physics;

namespace Polspace
{
    public class Ship
    {
        private readonly BoxBody _body;
        public FuelContainer FuelContainer { get; }
        public List<Engine> Engines { get; }
        public Vector Position => _body.Position;
        public double Angle => _body.Angle;
        public bool IsDestroyed { get; private set; }

        public Ship(Vector position)
        {
            _body = new BoxBody(position, Vector.New(2, 5), 0.5, 1200, 10e6, 0.3);
            FuelContainer = new FuelContainer(FuelContainerType.Standard);
            Engines = new List<Engine>
            {
                new(EngineType.Main, _body, Vector.New(0, -2.5), 0, this),
                new(EngineType.Side, _body, Vector.New(1, 1), -Math.PI / 2, this),
                new(EngineType.Side, _body, Vector.New(-1, 1), Math.PI / 2, this)
            };
        }

        public void AddToWorld(World world)
        {
            world.AddBody(_body);
        }

        private void ApplyEngines(double time)
        {
            var fuelLoss = 0.0;
            foreach (var engine in Engines)
            {
                if (engine.IsOn)
                    fuelLoss += engine.Type.MaxThrust / engine.Type.SpecificImpulse;
            }

            if (fuelLoss == 0)
                return;
            fuelLoss *= time;
            if (FuelContainer.Fuel >= fuelLoss)
            {
                _body.Mass -= fuelLoss;
                FuelContainer.Fuel -= fuelLoss;
            }
            else
            {
                _body.Mass -= FuelContainer.Fuel;
                FuelContainer.Fuel = 0;
            }

            if (FuelContainer.Fuel == 0)
            {
                foreach (var engine in Engines)
                    engine.IsOn = false;
            }
        }

        public void Update(double time)
        {
            ApplyEngines(time);
            if (!IsDestroyed && _body.Acceleration.GetLength() > 100)
                IsDestroyed = true;
        }
    }
}