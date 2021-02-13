using System.Diagnostics;

namespace Polspace
{
    public class GameTimeTicker
    {
        public double? LastFrameTime { get; private set; }
        private static readonly double Frequency = Stopwatch.Frequency;

        public double Tick()
        {
            var currentFrameTime = Stopwatch.GetTimestamp() / Frequency;
            var frameDuration = currentFrameTime - LastFrameTime ?? currentFrameTime;
            LastFrameTime = currentFrameTime;
            return frameDuration;
        }
    }
}