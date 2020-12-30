namespace Polspace
{
    public class FuelContainer
    {
        private FuelContainerType Type { get; }
        public double Fuel { get; set; } // [kg]
        public double Mass => Fuel + Type.EmptyMass; // [kg]

        public FuelContainer(FuelContainerType type)
        {
            Type = type;
            Fuel = type.MaxFuel;
        }
    }
}