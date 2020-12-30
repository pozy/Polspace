using System.Diagnostics;

namespace Polspace
{
    public class GameTimeTicker
    {
        private long? _lastFrameTime;

        public double Tick()
        {
            var currentFrameTime = Stopwatch.GetTimestamp();
            var frameDuration = (double) (currentFrameTime - _lastFrameTime ?? currentFrameTime) / Stopwatch.Frequency;
            _lastFrameTime = currentFrameTime;
            return frameDuration;
        }
    }
}