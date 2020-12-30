namespace Polspace
{
    public class FuelContainerType
    {
        public double EmptyMass { get;  } // [kg]
        public double MaxFuel { get; } // [kg]
        public FuelContainerType(double emptyMass, double maxFuel)
        {
            EmptyMass = emptyMass;
            MaxFuel = maxFuel;
        }

        public static FuelContainerType Standard = new(100, 800);
    }
}