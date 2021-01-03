using System.Globalization;
using Math;
using SFML.Graphics;
using SFML.System;

namespace Polspace
{
    public class GameRenderer
    {
        private readonly Font _font;
        public GameRenderer()
        {
            _font = new Font("Content/arial.ttf");
        }
        public void Render(GameState gameState, RenderWindow window, Camera camera)
        {
            window.Clear();

            var groundPositionOnScreen = camera.ToScreenPosition(Vector.Zero);
            var groundShape = new RectangleShape
            {
                Position = new Vector2f(0, groundPositionOnScreen.Y),
                Size = new Vector2f(window.Size.X, window.Size.Y - groundPositionOnScreen.Y),
                FillColor = new Color(127,127,127)
            };
            window.Draw(groundShape);

            var shipSizeOnScreen = camera.ToScreenSize(gameState.Ship.Size);
            var shipPositionOnScreen = camera.ToScreenPosition(gameState.Ship.Position);
            var shipShape = new RectangleShape
            {
                Size = shipSizeOnScreen,
                Origin = shipSizeOnScreen / 2,
                FillColor = gameState.Ship.IsDestroyed ? new Color(255, 127, 0) : Color.White,
                Position = shipPositionOnScreen
            };
            window.Draw(shipShape);

            if (gameState.Ship.MainEngine.IsOn)
            {
                var engineShape = new ConvexShape(3)
                {
                    Position = shipPositionOnScreen + new Vector2f(0, shipSizeOnScreen.Y / 2),
                    FillColor = Color.Red
                };
                engineShape.SetPoint(0, new Vector2f(-shipSizeOnScreen.X / 2, 0));
                engineShape.SetPoint(1, new Vector2f(shipSizeOnScreen.X / 2, 0));
                engineShape.SetPoint(2, new Vector2f(0, shipSizeOnScreen.X));
                window.Draw(engineShape);
            }

            var accelerationString = gameState.Ship.Acceleration.Y.ToString(CultureInfo.InvariantCulture);
            var statsTextShape = new Text(accelerationString, _font)
            {
                Position = new Vector2f(10,10),
                FillColor = Color.White
            };
            window.Draw(statsTextShape);
        }
    }
}