using System;
using System.Collections.Generic;
using Physics;
using SFML.Graphics;
using SFML.System;

namespace Polspace
{
    public class GameRenderer
    {
        private readonly List<ConvexShape> _engineShape;
        private readonly RectangleShape _groundShape;
        private readonly RectangleShape _shipShape;
        private readonly Text _statsTextShape;

        public string StatsText
        {
            get => _statsTextShape.DisplayedString;
            set => _statsTextShape.DisplayedString = value;
        }
        private readonly GameState _gameState;

        public GameRenderer(GameState gameState)
        {
            _gameState = gameState;

            var font = new Font("Content/arial.ttf");
            _statsTextShape = new Text("frames:", font)
            {
                Position = new Vector2f(10,10),
                FillColor = Color.White
            };

            _groundShape = new RectangleShape
            {
                FillColor = new Color(127,127,127)
            };

            _shipShape = new RectangleShape();

            _engineShape = new List<ConvexShape>();
            for (var i = 0; i < 3; i++)
            {
                var engineShape = new ConvexShape(3)
                {
                    FillColor = Color.Red
                };
                engineShape.SetPoint(0, new Vector2f(-1, 0));
                engineShape.SetPoint(1, new Vector2f(1, 0));
                engineShape.SetPoint(2, new Vector2f(0, 2));
                _engineShape.Add(engineShape);
            }
        }
        
        public void Render(RenderWindow window, Camera camera)
        {
            window.Clear();

            var groundPositionOnScreen = camera.ToScreenPosition(Vector.Zero);
            _groundShape.Position = new Vector2f(0, groundPositionOnScreen.Y);
            _groundShape.Size = new Vector2f(window.Size.X, window.Size.Y - groundPositionOnScreen.Y);
            window.Draw(_groundShape);
            
            var shipPositionOnScreen = camera.ToScreenPosition(_gameState.Ship.Position);
            _shipShape.Size = camera.ToScreenSize(Vector.New(2, 5));
            _shipShape.Position = shipPositionOnScreen;
            _shipShape.Origin = camera.ToScreenSize(Vector.New(2, 5) / 2);
            _shipShape.FillColor = _gameState.Ship.IsDestroyed ? new Color(255, 127, 0) : Color.White;
            _shipShape.Rotation = (float) (_gameState.Ship.Angle * 180 / Math.PI);
            window.Draw(_shipShape);

            for (var i = 0; i < _gameState.Ship.Engines.Count; i++)
            {
                var shipEngine = _gameState.Ship.Engines[i];
                if (!shipEngine.IsOn)
                    continue;
                var engineShape = _engineShape[i];
                engineShape.Position = camera.ToScreenPosition(shipEngine.RotatedAttachmentPoint + _gameState.Ship.Position);
                engineShape.Rotation = (float) ((shipEngine.AttachmentAngle + _gameState.Ship.Angle) * 180 / Math.PI);
                engineShape.Scale = new Vector2f(camera.Zoom, camera.Zoom);
                window.Draw(engineShape);
            }
            
            window.Draw(_statsTextShape);
        }
    }
}