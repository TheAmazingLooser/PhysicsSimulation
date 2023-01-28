using System;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLTest.Objects;
using SFMLTest.Shapes;

namespace SFMLTest
{
    class Program
    {
        const int WIDTH = 1200;
        const int HEIGHT = 800;
        const string TITLE = "Physics Engine";

        private static Window window;
        private static World world;

        public static int TICK_SPEED = 50;
        private static Clock clock = new Clock();

        /*
        private static void CreateObject()
        {
            Texture ground_texture = new Texture("ground.png");
            Primitive2D ground = new Primitive2D(false, "ground", new SFML.System.Vector2f(0.0f, HEIGHT - 32), 0.0f, ground_texture);
            window.AddObject(ground);
            physics.AddObject(ground);
            ground.sp.Color = Color.White;
        }
        */

        static void Main(string[] args)
        {
            window = new Window(TITLE, WIDTH, HEIGHT);
            window.window.Closed += (sender, args) => window.window.Close();

            world = new World(9.8f)
            {
                Bounds = new Vector2f(WIDTH, HEIGHT)
            };

            // ahh lol, höhere auflösungen
            //window.window.Position = new SFML.System.Vector2i(-2200, 1200); // wenn das nicht geht koords ändern xD (lol, sorry) sieht so aus xD probier mal

            // Rects

            /*
            Random rand = new Random(1);
            for(int i = 0; i < 5000; i++)
            {
                world.AddObject(new Rectangle(64, 64)
                {
                    Texture = new Texture("texture.jpg"),
                    Position = new Vector2f(rand.Next() % (WIDTH - 64) + 32, rand.Next() % (HEIGHT - 64) + 32),
                    VelY = 4,
                    Bounciness = 0.01f
                });
            }
            */

            
            world.AddObject(new Rectangle(64, 64)
            {
                Texture = new Texture("texture.jpg"),
                Position = new Vector2f(250,250),
                VelY = 5,
                VelX = -40,
                Bounciness = 0.5f
            });
            world.AddObject(new Rectangle(64, 64)
            {
                Texture = new Texture("texture.jpg"),
                Position = new Vector2f(250,100),
                VelY = 0,
                VelX = 4,
                Bounciness = 0.5f
            });
            

            Color WallColor = new Color(12, 12, 12, 255);
            // Ground and Walls
            // Left Wall
            world.AddObject(new Rectangle(32, HEIGHT)
            {
                Texture = new Texture("ground.png"),
                Color = WallColor,
                Position = new Vector2f(0, 0),
                IsSolid = true
            });
            // Right Wall
            world.AddObject(new Rectangle(32, HEIGHT)
            {
                Texture = new Texture("ground.png"),
                Color = WallColor,
                Position = new Vector2f(WIDTH - 32, 0),
                IsSolid = true
            });
            // Top Wall
            world.AddObject(new Rectangle(WIDTH, 32)
            {
                Texture = new Texture("ground.png"),
                Color = WallColor,
                Position = new Vector2f(0, 0),
                IsSolid = true
            });
            // Bot Wall (Ground)
            world.AddObject(new Rectangle(WIDTH, 32)
            {
                Texture = new Texture("ground.png"),
                Color = WallColor,
                Position = new Vector2f(0, HEIGHT - 32),
                IsSolid = true
            });



            /*
            Physics physics = new Physics();

            Texture ground_texture = new Texture("ground.png");
            Primitive2D ground = new Primitive2D(false, "ground", new SFML.System.Vector2f(0.0f, HEIGHT - 32), 0.0f, ground_texture);
            window.AddObject(ground);
            physics.AddObject(ground);
            ground.sp.Color= Color.White;

            Primitive2D top = new Primitive2D(false, "top", new SFML.System.Vector2f(0.0f, 0.0f), 0.0f, ground_texture);
            window.AddObject(top);
            physics.AddObject(top);
            top.sp.Color= Color.White;

            Texture side_texture = new Texture(64, HEIGHT);
            Primitive2D left = new Primitive2D(false, "left", new SFML.System.Vector2f(0.0f, HEIGHT), 0.0f, side_texture);
            window.AddObject(left);
            physics.AddObject(left);
            left.sp.Color= Color.White;

            Primitive2D right = new Primitive2D(false, "right", new SFML.System.Vector2f(WIDTH - 64, HEIGHT), 0.0f, side_texture);
            window.AddObject(right);
            physics.AddObject(right);
            right.sp.Color = Color.White;


            Primitive2D square = new Primitive2D(true, "test", new SFML.System.Vector2f(WIDTH / 2 + 32, HEIGHT /2 - 128), 0.3f);
            window.AddObject(square);
            physics.AddObject(square);
            square.velocity.X = -0.2f;
            square.velocity.Y = 3f;

            Primitive2D square2 = new Primitive2D(true, "test2", new SFML.System.Vector2f(WIDTH / 2 + 32, HEIGHT / 2 + 32), 0.3f);
            window.AddObject(square2);
            physics.AddObject(square2);
            square2.velocity.X = -0.1f;



            Primitive2D ground2 = new Primitive2D(false, "ground2", new SFML.System.Vector2f(9999, 9999), 0.0f, new Texture(1,1));
            window.AddObject(ground2);
            physics.AddObject(ground2);
            */

            new Thread(UpdateThread).Start();

            while (window.window.IsOpen)
            {
                long time = clock.Restart().AsMicroseconds();

                // Rendern!
                window.Update(world, 1000000 / time);
            }
        }

        private static void UpdateThread()
        {
            int TicksPerUpdate = 1000 * 1000 / TICK_SPEED;
            long ticks = 0;

            Clock c = new Clock();
            while (window.window.IsOpen)
            {
                long time = c.Restart().AsMicroseconds();
                ticks += time;
                while (ticks > TicksPerUpdate)
                {
                    // Update
                    world.Update();
                    ticks -= TicksPerUpdate;
                }
            }
        }
    }
}