namespace Polspace
{
    public class EngineType
    {
        public double Mass { get; } // [kg]
        public double MaxThrust { get;  } // [kg*m/s^2]
        public double SpecificImpulse { get;  } // [m/s]

        public EngineType(double mass, double maxThrust, double specificImpulse)
        {
            Mass = mass;
            MaxThrust = maxThrust;
            SpecificImpulse = specificImpulse;
        }

        public static EngineType Main => new(200, 2000, 50);
        public static EngineType Side => new(50, 300, 150);
    }
}