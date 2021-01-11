using System.Collections.Generic;

namespace Physics
{
    public abstract class Body
    {
        public abstract IReadOnlyList<Vector> Points { get; }

        public abstract Vector GetShortestVectorToOutside(in Vector point);

        public virtual void Update(in double time)
        {

        }
    }
}