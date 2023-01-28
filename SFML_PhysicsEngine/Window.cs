using System;
using System.Collections.Generic;
using System.Text;
using SFML.Window;
using SFML.Graphics;
using SFMLTest.Shapes;
using System.Linq;

namespace SFMLTest
{
    class Window
    {
        private VideoMode mode;
        public RenderWindow window;
        private Font font = new Font("ChakraPetch-Regular.ttf");

        public Window(string title, uint width, uint height)
        {
            mode = new VideoMode(width, height);
            window = new RenderWindow(mode, title);
            window.SetVerticalSyncEnabled(true);
        }

        public void Render(World world, double FPS)
        {
            window.Clear();

            /*
            foreach(Primitive2D pr2d in renderBuffer)
            {
                window.Draw(pr2d.sp);
            }
            */

            world.Render(window);
            window.Draw(new Text("FPS: " + FPS, font, 22));
            window.Draw(new Text("Objects: " + world.ObjectAmount, font, 22)
            {
                Position = new SFML.System.Vector2f(0, 25),
            });

            window.Display();
            window.DispatchEvents();
        }
    }
}