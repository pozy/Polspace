using Physics;

namespace Polspace
{
    public class GameState
    {
        private readonly World _world;
        public Ship Ship { get; }

        public int Frames => _world.Frames;
        public double GameTime => _world.CurrentTime;

        public GameState()
        {
            _world = new World();
            Ship = new Ship(Vector.New(0, 80));
            Ship.AddToWorld(_world);
            _world.AddBody(new GroundBody(10e6, 0.3));
            foreach (var shipEngine in Ship.Engines)
                _world.AddEngine(shipEngine);
        }

        public void Update(double time)
        {
            Ship.Update(time);
            _world.Update(time);
        }
    }
}