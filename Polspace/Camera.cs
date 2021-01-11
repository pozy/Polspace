using Physics;
using SFML.System;
using SFML.Window;

namespace Polspace
{
    public class Camera
    {
        public Window Screen { get; }
        public Vector Position { get; private set; }
        public float Zoom { get; set; }

        public Camera(Window screen, Vector position)
        {
            Screen = screen;
            Position = position;
            Zoom = 1;
        }

        public Vector2f ToScreenPosition(Vector position)
        {
            var relativePosition = position - Position;
            var screenCenter = (Vector2f)Screen.Size / 2;
            return screenCenter + new Vector2f((float)relativePosition.X * Screen.Size.X, -(float)relativePosition.Y * Screen.Size.Y) * Zoom;
        }

        public Vector2f ToScreenSize(Vector size)
        {
            return new Vector2f((float) size.X * Screen.Size.X, (float) size.Y * Screen.Size.Y) * Zoom;
        }

        public void Move(Vector v)
        {
            Position += v;
        }
    }
}