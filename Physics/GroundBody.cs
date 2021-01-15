using System;

namespace Physics
{
    public class GroundBody : StaticBody
    {
        public override ReadOnlySpan<Vector> Points => Array.Empty<Vector>();
        
        public override Vector GetShortestVectorToOutside(in Vector point)
        {
            return point.Y < 0 ? Vector.New(0, -point.Y) : Vector.Zero;
        }
    }
}