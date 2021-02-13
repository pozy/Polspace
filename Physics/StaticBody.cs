namespace Physics
{
    public abstract class StaticBody : Body
    {
        protected StaticBody(
            double bounceForceCoefficient,
            double coefficientOfRestitution)
            : base(bounceForceCoefficient, coefficientOfRestitution)
        {
        }
    }
}