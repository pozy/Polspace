using System;
using Math;

namespace Polspace
{
    public class PhysicalPart
    {
        public double MaxAcceleration { get; protected set; } // [m/s^2]
        public bool IsDestroyed { get; set; }
        public virtual double Mass { get; } // [kg]
        public Vector Size { get; protected set; } // [m]
        public Vector Position { get; protected set; } // [m]
        public Vector Velocity { get; private set; } // [m/s]
        public Vector Acceleration { get; private set; } // [m/s^2]
        private Vector _currentAcceleration; // [m/s^2]
        public double Rotation { get; private set; } // [r]
        public double RotationVelocity { get; private set; } // [r/s]
        public double RotationAcceleration { get; private set; } // [r/s^2]

        public void ApplyForce(Vector force, Vector targetPosition)
        {
            if (force.X == 0 && targetPosition.X == 0)
            {
                _currentAcceleration += force / Mass;
            }
            else
                throw new ArgumentException();
        }

        public void Update(double time)
        {
            Velocity += _currentAcceleration * time;
            Position += Velocity * time;
            if (_currentAcceleration.GetLength() > MaxAcceleration)
                IsDestroyed = true;
            Acceleration = _currentAcceleration;
            _currentAcceleration = Vector.Zero;
            RotationAcceleration = 0;
        }
    }
}