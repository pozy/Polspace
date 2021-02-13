namespace Physics
{
    public static class Collider
    {
        public static void Interact(Body body1, Body body2)
        {
            switch (body1, body2)
            {
                case (BoxBody box1, BoxBody box2):
                    Interact(box1, box2);
                    break;
                case (GroundBody ground, BoxBody box):
                    Interact(ground, box);
                    break;
                case (BoxBody box, GroundBody ground):
                    Interact(ground, box);
                    break;
            }
        }

        public static void Interact(BoxBody body1, BoxBody body2)
        {
            InteractFrom(body1, body2);
            InteractFrom(body2, body1);
        }

        private static void InteractFrom(BoxBody body1, BoxBody body2)
        {
            foreach (var point in body1.Points)
            {
                var toOutside = body2.GetShortestVectorToOutside(point + body1.Position);
                if (toOutside == Vector.Zero)
                    continue;
                var isPointGoingInside = Vector.Dot(toOutside, body1.Velocity) < 0;
                var coefficient = isPointGoingInside ? 1 : body1.CoefficientOfRestitution*body2.CoefficientOfRestitution;
                var force = toOutside * coefficient * body1.BounceForceCoefficient*body2.BounceForceCoefficient;
                body1.ApplyForce(force, point);
                body2.ApplyForce(-force, body1.Position + point - body2.Position);
            }
        }

        public static void Interact(GroundBody ground, BoxBody box)
        {
            if (box.Position.Y > box.Size.Y + box.Size.X)
                return;
            foreach (var point in box.Points)
            {
                var toOutside = ground.GetShortestVectorToOutside(point + box.Position);
                if (toOutside == Vector.Zero)
                    continue;
                var isPointGoingInside = Vector.Dot(toOutside, box.Velocity) < 0;
                var coefficient = isPointGoingInside ? 1 : ground.CoefficientOfRestitution * box.CoefficientOfRestitution;
                var force = toOutside * coefficient * (ground.BounceForceCoefficient + box.BounceForceCoefficient) / 2;
                box.ApplyForce(force, point);
            }
        }
    }
}