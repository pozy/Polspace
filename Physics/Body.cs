using System;

namespace Physics
{
    public abstract class Body
    {
        public abstract ReadOnlySpan<Vector> Points { get; }

        public abstract Vector GetShortestVectorToOutside(in Vector point);

        public virtual void Update(in double time)
        {

        }
    }
}