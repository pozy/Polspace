﻿using System;
using Physics;
using Polspace;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

var window = new RenderWindow(new VideoMode(800, 600), "Polspace", Styles.Close);
var gameState = new GameState();
var renderer = new GameRenderer(gameState);

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
    var actualRealTimeFrameDuration = UpdateGameState(gameState, realTimeTicker, 1.0 / 60);
    realTime += actualRealTimeFrameDuration;

    window.DispatchEvents();
    if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        camera.Move(Vector.New(1, 0) * cameraSpeed * actualRealTimeFrameDuration);
    if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        camera.Move(Vector.New(-1, 0) * cameraSpeed * actualRealTimeFrameDuration);
    if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        camera.Move(Vector.New(0, 1) * cameraSpeed * actualRealTimeFrameDuration);
    if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
        camera.Move(Vector.New(0, -1) * cameraSpeed * actualRealTimeFrameDuration);
    gameState.Ship.Engines[0].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.W);
    gameState.Ship.Engines[1].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.D);
    gameState.Ship.Engines[2].IsOn = Keyboard.IsKeyPressed(Keyboard.Key.A);

    renderer.StatsText = @$"frames: {gameState.Frames}
real time: {realTime:F2}
game time: {gameState.GameTime:F2}
diff: {realTime - gameState.GameTime:F2}";

    renderer.Render(window, camera);
    window.Display();
}

static double UpdateGameState(GameState gameState, GameTimeTicker realTimeTicker, double requestedRealTimeFrameDuration)
{
    var realTime = 0.0;
    while (realTime < requestedRealTimeFrameDuration)
    {
        var realTimeFrameDuration = realTimeTicker.Tick();
        realTime += realTimeFrameDuration;
        var gameTimeFrameDuration = Math.Min(realTimeFrameDuration, requestedRealTimeFrameDuration / 3);
        gameState.Update(gameTimeFrameDuration);
    }

    return realTime;
}
