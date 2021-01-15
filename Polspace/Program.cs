using System;
using Physics;
using SFML.Graphics;
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
            var camera = new Camera(window, Vector.New(0, 40));
            camera.Zoom = 7f;
            var cameraSpeed = 10; // units per second
            var gameTime = new GameTimeTicker();
            window.Closed += (_, _) => window.Close();
            var isPaused = true;
            window.KeyPressed += (_, args) =>
            {
                if (args.Code == Keyboard.Key.P)
                    isPaused = !isPaused;
            };
            while (window.IsOpen)
            {
                var frameDuration = gameTime.Tick();

                window.DispatchEvents();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    camera.Move(Vector.New(1, 0) * cameraSpeed * frameDuration);
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    camera.Move(Vector.New(-1, 0) * cameraSpeed * frameDuration);
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                    camera.Move(Vector.New(0, 1) * cameraSpeed * frameDuration);
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                    camera.Move(Vector.New(0, -1) * cameraSpeed * frameDuration);
                if (!gameState.Ship.IsDestroyed && gameState.Ship.FuelContainer.Fuel > 0)
                {
                    gameState.Ship.MainEngine.IsOn = Keyboard.IsKeyPressed(Keyboard.Key.W);
                    gameState.Ship.RightEngine.IsOn = Keyboard.IsKeyPressed(Keyboard.Key.D);
                    gameState.Ship.LeftEngine.IsOn = Keyboard.IsKeyPressed(Keyboard.Key.A);
                }
                else
                {
                    gameState.Ship.MainEngine.IsOn = false;
                    gameState.Ship.RightEngine.IsOn = false;
                    gameState.Ship.LeftEngine.IsOn = false;
                }
                

                if (!isPaused)
                    gameState.Update(frameDuration);
                renderer.Render(gameState, window, camera);

                window.Display();
            }
        }
    }
}
