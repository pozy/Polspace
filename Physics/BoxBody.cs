using System;

namespace Physics
{
    public class BoxBody : DynamicBody
    {
        public Vector Size { get; }

        private readonly Vector[] _points;

        public ReadOnlySpan<Vector> Points => _points;

        /// <inheritdoc />
        public BoxBody(
            Vector position,
            Vector size,
            double angle,
            double mass,
            double bounceForceCoefficient,
            double coefficientOfRestitution,
            double maxAcceleration = double.PositiveInfinity)
            : base(
                position,
                angle,
                mass,
                CalculateMomentOfInertiaInternal(mass, size),
                bounceForceCoefficient,
                coefficientOfRestitution,
                maxAcceleration)
        {
            Size = size;
            _points = new Vector[4];
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            var halvedSize = Size / 2;
            var rotated1 = AngleVector * halvedSize.Y;
            var rotated2 = Vector.New(AngleVector.Y, -AngleVector.X) * halvedSize.X;
            _points[0] = rotated1 + rotated2;
            _points[1] = -rotated1 + rotated2;
            _points[2] = -rotated1 - rotated2;
            _points[3] = rotated1 - rotated2;
        }

        public override Vector GetShortestVectorToOutside(in Vector globalPoint)
        {
            var p = globalPoint - Position;
            var angleVector90 = Vector.New(AngleVector.Y, -AngleVector.X);
            var distance = Vector.New(Vector.Dot(p, AngleVector), Vector.Dot(p, angleVector90));
            var depth = Size / 2 - distance;
            if (depth.X > 0 || depth.Y > 0)
                return Vector.Zero;
            // inside box
            if (depth.X < depth.Y)
                return distance.Y * AngleVector;
            else
                return distance.X * angleVector90;
        }

        protected override double CalculateMomentOfInertia() => CalculateMomentOfInertiaInternal(Mass, Size);

        private static double CalculateMomentOfInertiaInternal(double mass, Vector size)
        {
            return (size.X * size.X + size.Y * size.Y) * mass / 12;
        }

        /// <inheritdoc />
        public override void Update(in double time)
        {
            base.Update(in time);
            UpdatePoints();
        }
    }
}