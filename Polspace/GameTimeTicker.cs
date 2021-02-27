using System.Diagnostics;

namespace Polspace
{
    public class GameTimeTicker
    {
        private double? _lastFrameTime;
        private static readonly double Frequency = Stopwatch.Frequency;

        public double Tick()
        {
            var currentFrameTime = Stopwatch.GetTimestamp() / Frequency;
            var frameDuration = currentFrameTime - (_lastFrameTime ?? currentFrameTime);
            _lastFrameTime = currentFrameTime;
            return frameDuration;
        }
    }
}