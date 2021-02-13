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
            var lastRender = gameTime.LastFrameTime;
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
                gameState.Ship.Engines[0].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.W);
                gameState.Ship.Engines[1].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.D);
                gameState.Ship.Engines[2].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.A);


                if (!isPaused)
                    gameState.Update(frameDuration);
                if (gameTime.LastFrameTime - (lastRender ?? 0) > 0.01)
                {
                    lastRender = gameTime.LastFrameTime;
                    renderer.Render(gameState, window, camera);
                }

                window.Display();
            }
        }
    }
}
