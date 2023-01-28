using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SFMLTest
{
    public class World
    {
        public Physics Physics { private set; get; }

        public Vector2f Bounds { get; set; }
        List<PhysicsObject> Objects = new List<PhysicsObject>();
        public int ObjectAmount => Objects.Count;

        public World(float Gravity)
        {
            Physics = new Physics(Gravity);
        }

        public void SetBounds(int X, int Y)
        {
            Bounds = new Vector2f(X, Y);
        }

        public void AddObject(PhysicsObject obj)
        {
            Objects.Add(obj);
        }

        public void Render(RenderWindow w)
        {
            // TODO:
            for(int i = 0; i < Objects.Count; i++)
            {
                w.Draw(Objects[i]);
            }
        }

        public void Update()
        {
            for(int i = Objects.Count - 1; i >= 0; i--)
            {
                if (Objects[i].Position.X + Objects[i].Width < 0 ||
                    Objects[i].Position.X > Bounds.X ||
                    Objects[i].Position.Y + Objects[i].Height < 0 ||
                    Objects[i].Position.Y > Bounds.Y)
                    Objects.RemoveAt(i);
            }
            Physics.Update(Objects);
        }
    }
}
