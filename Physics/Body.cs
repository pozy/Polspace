namespace Physics
{
    public abstract class Body
    {
        public abstract Vector GetShortestVectorToOutside(in Vector point);

        public readonly double BounceForceCoefficient;
        public readonly double CoefficientOfRestitution;

        protected Body(double bounceForceCoefficient, double coefficientOfRestitution)
        {
            BounceForceCoefficient = bounceForceCoefficient;
            CoefficientOfRestitution = coefficientOfRestitution;
        }

        public virtual void Update(in double time)
        {

        }
    }
}