using System.Collections.Generic;
using Physics;

namespace Polspace
{
    public class Ship : BoxBody
    {
        public FuelContainer FuelContainer { get; }
        public List<Engine> Engines { get; }

        public Ship(Vector position)
            : base(
                position,
                Vector.New(2, 5),
                0.5,
                1200,
                10e6,
                0.3,
                100)
        {
            FuelContainer = new FuelContainer(FuelContainerType.Standard);
            Engines = new List<Engine>
            {
                new(EngineType.Main, this, Vector.New(0, 2), this),
                new(EngineType.Side, this, Vector.New(1, 1), this),
                new(EngineType.Side, this, Vector.New(-1, 1), this)
            };
        }
    }
}