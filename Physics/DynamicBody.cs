using System;

namespace Physics
{
    public abstract class DynamicBody : Body
    {
        public Vector Acceleration { get; private set; } // [m/s^2]

        public double Mass // [kg]
        {
            get => _mass;
            set
            {
                _mass = value;
                MomentOfInertia = CalculateMomentOfInertia();
            }
        }

        public Vector Position { get; private set; } // [m]
        public Vector Velocity { get; private set; } // [m/s]
        public Vector Force { get; private set; } // [N, kg*m/s^2]
        private Vector _currentForce; // [m/s^2]

        public double MomentOfInertia { get; private set; } // [N*s, kg*m^2]
        public double Angle { get; private set; } // [rad]
        public Vector AngleVector { get; private set; }
        public double AngularMomentum { get; private set; } // [N*m, kg*m^2/s]
        public double Torque { get; private set; } // [N*m/s, kg*m^2/s^2]
        private double _currentTorque;
        private double _mass;

        protected DynamicBody(
            Vector position,
            double angle,
            double mass,
            double momentOfInertia,
            double bounceForceCoefficient,
            double coefficientOfRestitution)
            : base(bounceForceCoefficient, coefficientOfRestitution)
        {
            if (angle < 0 || angle >= Math.PI * 2)
                throw new ArgumentOutOfRangeException(nameof(angle), angle, @"Should be in range [0, 2*pi)");
            if (momentOfInertia < 0.1)
                throw new ArgumentOutOfRangeException(nameof(momentOfInertia), momentOfInertia, @"Should be no less than 0.1");
            Position = position;
            Angle = angle;
            Mass = mass;
            MomentOfInertia = momentOfInertia;
        }

        protected abstract double CalculateMomentOfInertia();

        public void ApplyForce(in Vector force, in Vector targetLocalPoint)
        {
            _currentForce += force;
            _currentTorque += Vector.Cross(force, targetLocalPoint);
        }

        public void ApplyForce(Vector force)
        {
            _currentForce += force;
        }

        public override void Update(in double time)
        {
            Acceleration = _currentForce / Mass;
            Velocity += Acceleration * time;
            Position += Velocity * time;
            Force = _currentForce;
            _currentForce = Vector.Zero;

            AngularMomentum += _currentTorque * time;
            Angle += AngularMomentum * time / MomentOfInertia;
            AngleVector = Vector.NewRotated(Angle);
            Torque = _currentTorque;
            _currentTorque = 0;
            Torque = 0;
        }
    }
}