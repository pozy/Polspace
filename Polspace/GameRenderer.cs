using System;
using Physics;
using SFML.Graphics;
using SFML.System;

namespace Polspace
{
    public class GameRenderer
    {
        private readonly ConvexShape _engineShape;
        private readonly RectangleShape _groundShape;
        private readonly ConvexShape _shipShape;
        private readonly Text _accTextShape;
        private readonly Text _framesTextShape;

        public GameRenderer()
        {
            var font = new Font("Content/arial.ttf");

            _groundShape = new RectangleShape
            {
                FillColor = new Color(127,127,127)
            };

            _shipShape = new ConvexShape(4);

            _engineShape = new ConvexShape(3)
            {
                FillColor = Color.Red
            };

            _accTextShape = new Text("acc:", font)
            {
                Position = new Vector2f(10,10),
                FillColor = Color.White
            };

            _framesTextShape = new Text("frames:", font)
            {
                Position = new Vector2f(10,40),
                FillColor = Color.White
            };
        }
        public void Render(GameState gameState, RenderWindow window, Camera camera)
        {
            window.Clear();

            var groundPositionOnScreen = camera.ToScreenPosition(Vector.Zero);
            _groundShape.Position = new Vector2f(0, groundPositionOnScreen.Y);
            _groundShape.Size = new Vector2f(window.Size.X, window.Size.Y - groundPositionOnScreen.Y);
            window.Draw(_groundShape);

            var shipSizeOnScreen = camera.ToScreenSize(gameState.Ship.Size);
            var shipPositionOnScreen = camera.ToScreenPosition(gameState.Ship.Position);
            _shipShape.Position = shipPositionOnScreen;
            _shipShape.FillColor = gameState.Ship.IsDestroyed ? new Color(255, 127, 0) : Color.White;
            for (var i = 0; i < gameState.Ship.Points.Length; i++)
            {
                var point = camera.ToScreenSize(gameState.Ship.Points[i]);
                _shipShape.SetPoint((uint)i, point);
            }
            window.Draw(_shipShape);

            if (gameState.Ship.Engines[0].IsOn)
            {
                _engineShape.Position = shipPositionOnScreen;
                _engineShape.Rotation = (float) (gameState.Ship.Angle * 180 / Math.PI);
                _engineShape.Origin = new Vector2f(0, shipSizeOnScreen.Y / 2);
                _engineShape.SetPoint(0, new Vector2f(-shipSizeOnScreen.X / 2, 0));
                _engineShape.SetPoint(1, new Vector2f(shipSizeOnScreen.X / 2, 0));
                _engineShape.SetPoint(2, new Vector2f(0, shipSizeOnScreen.X));
                window.Draw(_engineShape);
            }
            
            _accTextShape.DisplayedString = "acc: " + gameState.Ship.Force.Y / gameState.Ship.Mass;
            window.Draw(_accTextShape);
            
            _framesTextShape.DisplayedString = "frames " + gameState.Frames;
            window.Draw(_framesTextShape);
        }
    }
}