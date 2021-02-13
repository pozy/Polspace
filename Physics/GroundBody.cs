namespace Physics
{
    public class GroundBody : StaticBody
    {
        public override Vector GetShortestVectorToOutside(in Vector point)
        {
            return point.Y < 0 ? Vector.New(0, -point.Y) : Vector.Zero;
        }

        public GroundBody(
            double bounceForceCoefficient,
            double coefficientOfRestitution)
            : base(bounceForceCoefficient, coefficientOfRestitution)
        {
        }
    }
}