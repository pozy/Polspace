namespace Physics
{
    public class Engine
    {
        public DynamicBody AttachedTo { get; }
        public Vector AttachmentPoint { get; }
        public double AttachmentAngle { get; }
        public virtual double Force { get; } // [m*kg/s^2]
        public Vector RotatedAttachmentPoint { get; private set; }

        public Engine(DynamicBody attachedTo, Vector attachmentPoint, double attachmentAngle)
        {
            AttachedTo = attachedTo;
            AttachmentPoint = attachmentPoint;
            AttachmentAngle = attachmentAngle;
            CalculateRotatedAttachmentPoint();
        }

        private void CalculateRotatedAttachmentPoint()
        {
            var rotated = AttachedTo.AngleVector;
            RotatedAttachmentPoint = Vector.New(
                AttachmentPoint.X * rotated.Y + AttachmentPoint.Y * rotated.X,
                -AttachmentPoint.X * rotated.X + AttachmentPoint.Y * rotated.Y);
        }

        public void Apply()
        {
            CalculateRotatedAttachmentPoint();
            var angle = Vector.NewRotated(AttachedTo.Angle + AttachmentAngle);
            AttachedTo.ApplyForce(Force * angle, RotatedAttachmentPoint);
        }
    }
}