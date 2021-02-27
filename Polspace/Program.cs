using System;
using Physics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Polspace
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var window = new RenderWindow(new VideoMode(800, 600), "Polspace", Styles.Close);
            var gameState = new GameState();
            var renderer = new GameRenderer();

            var realTimeTextShape = new Text("real time:", renderer.Font)
            {
                Position = new Vector2f(10,70),
                FillColor = Color.White
            };

            var gameTimeTextShape = new Text("game time:", renderer.Font)
            {
                Position = new Vector2f(10,100),
                FillColor = Color.White
            };

            var diffTimeTextShape = new Text("diff:", renderer.Font)
            {
                Position = new Vector2f(10,130),
                FillColor = Color.White
            };

            var camera = new Camera(window, Vector.New(0, 40))
            {
                Zoom = 7f
            };
            var cameraSpeed = 10.0; // units per second
            var realTimeTicker = new GameTimeTicker();
            window.Closed += (_, _) => window.Close();
            var isPaused = true;
            window.KeyPressed += (_, args) =>
            {
                if (args.Code == Keyboard.Key.P)
                    isPaused = !isPaused;
            };
            var realTime = 0.0;
            while (window.IsOpen)
            {
                var realTimeFrameDuration = UpdateGameState(realTimeTicker, gameState);
                realTime += realTimeFrameDuration;

                window.DispatchEvents();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    camera.Move(Vector.New(1, 0) * cameraSpeed * realTimeFrameDuration);
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    camera.Move(Vector.New(-1, 0) * cameraSpeed * realTimeFrameDuration);
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                    camera.Move(Vector.New(0, 1) * cameraSpeed * realTimeFrameDuration);
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                    camera.Move(Vector.New(0, -1) * cameraSpeed * realTimeFrameDuration);
                gameState.Ship.Engines[0].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.W);
                gameState.Ship.Engines[1].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.D);
                gameState.Ship.Engines[2].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.A);

                renderer.Render(gameState, window, camera);

                realTimeTextShape.DisplayedString = $"real time: {realTime:F2}";
                window.Draw(realTimeTextShape);

                gameTimeTextShape.DisplayedString = $"game time: {gameState.GameTime:F2}";
                window.Draw(gameTimeTextShape);

                diffTimeTextShape.DisplayedString = $"diff: {realTime - gameState.GameTime:F2}";
                window.Draw(diffTimeTextShape);

                window.Display();
            }
        }

        private static double UpdateGameState(GameTimeTicker realTimeTicker, GameState gameState)
        {
            var realTime = 0.0;
            while (realTime < 1.0 / 60)
            {
                var realTimeFrameDuration = realTimeTicker.Tick();
                realTime += realTimeFrameDuration;

                var gameTimeFrameDuration = Math.Min(realTimeFrameDuration, 1.0 / 180);

                gameState.Update(gameTimeFrameDuration);
            }
            
            return realTime;
        }
    }
}
