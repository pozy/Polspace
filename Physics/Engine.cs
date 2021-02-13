namespace Physics
{
    public class Engine
    {
        public DynamicBody AttachedTo { get; }
        public Vector AttachmentPoint { get; }
        public virtual double Force { get; } // [m*kg/s^2]

        public Engine(DynamicBody attachedTo, Vector attachmentPoint)
        {
            AttachedTo = attachedTo;
            AttachmentPoint = attachmentPoint;
        }
        public void Apply()
        {
            var rotated = AttachedTo.AngleVector;
            var rotatedPoint = Vector.New(
                AttachmentPoint.X * rotated.Y + AttachmentPoint.Y * rotated.X,
                -AttachmentPoint.X * rotated.X + AttachmentPoint.Y * rotated.Y);
            AttachedTo.ApplyForce(Force * rotated, rotatedPoint);
        }
    }
}