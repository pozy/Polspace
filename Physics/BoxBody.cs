using System.Collections.Generic;

namespace Physics
{
    public class BoxBody : DynamicBody
    {
        public Vector Size { get; }

        private readonly Vector[] _points;
        private Vector _angleVector;
        private Vector _angle90Vector;


        public override IReadOnlyList<Vector> Points => _points;

        /// <inheritdoc />
        public BoxBody(Vector position, Vector size, double angle, double mass, double maxAcceleration = double.PositiveInfinity)
            : base(position, angle, mass, CalculateMomentOfInertiaInternal(mass, size), maxAcceleration)
        {
            Size = size;
            _points = new Vector[4];
            UpdateAngleVectors();
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            var halvedSize = Size / 2;
            var rotated1 = _angleVector * halvedSize.Y;
            var rotated2 = _angle90Vector * halvedSize.X;
            _points[0] = rotated1 + rotated2;
            _points[1] = -rotated1 + rotated2;
            _points[2] = -rotated1 - rotated2;
            _points[3] = rotated1 - rotated2;
        }

        private void UpdateAngleVectors()
        {
            _angleVector = Vector.NewRotated(Angle);
            _angle90Vector = Vector.New(_angleVector.Y, -_angleVector.X);
        }

        public override Vector GetShortestVectorToOutside(in Vector point)
        {
            var p = point - Position;
            var distance = Vector.New(Vector.Dot(p, _angleVector), Vector.Dot(p, _angle90Vector));
            var depth = Size / 2 - distance;
            if (depth.X > 0 || depth.Y > 0)
                return Vector.Zero;
            // inside box
            if (depth.X < depth.Y)
                return distance.Y * _angleVector;
            else
                return distance.X * _angle90Vector;
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
            UpdateAngleVectors();
            UpdatePoints();
        }
    }
}