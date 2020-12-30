namespace Polspace
{
    public class Engine
    {
        public EngineType Type { get; }
        public bool IsOn { get; set; }

        public Engine(EngineType type)
        {
            Type = type;
            IsOn = false;
        }
    }
}