using System;
using System.Collections.Generic;

namespace Physics
{
    public class World
    {
        public int Frames { get; private set; }

        private readonly List<Body> _bodies;

        private readonly List<Engine> _engines;

        public static Vector Gravity => Vector.New(0, -1);

        public IReadOnlyList<Body> Bodies => _bodies;
        public IReadOnlyList<Engine> Engines => _engines;

        public World()
        {
            _bodies = new List<Body>();
            _engines = new List<Engine>();
        }

        public void AddBody(Body body)
        {
            _bodies.Add(body);
        }

        public void AddEngine(Engine engine)
        {
            _engines.Add(engine);
        }

        private void UpdateFrame(double time)
        {
            Frames++;

            for (var i = 0; i < _bodies.Count; i++)
                for (var j = i + 1; j < _bodies.Count; j++)
                    Collider.Interact(_bodies[i], _bodies[j]);

            foreach (var body in _bodies)
                if (body is DynamicBody dynamicBody)
                    dynamicBody.ApplyForce(Gravity * dynamicBody.Mass);

            foreach (var engine in _engines)
                engine.Apply();

            foreach (var body in _bodies)
                body.Update(time);
        }

        private double _destTime;
        public double CurrentTime;

        public void Update(double time)
        {
            _destTime += time;
            var ship = (DynamicBody) _bodies[0];
            var velocity = ship.Velocity.GetLength();
            var distance = Math.Abs(ship.Position.Y);
            var frameDuration = CalculateFrameDuration(distance, velocity);
            while (CurrentTime < _destTime)
            {
                UpdateFrame(frameDuration);
                CurrentTime += frameDuration;
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
            var frameDuration = 1.0 / Math.Pow(2.0, frameTimePrecision);
            return frameDuration;
        }
    }
}