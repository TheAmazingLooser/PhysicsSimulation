using System;
using System.Collections.Generic;
using SFMLTest.Shapes;
using SFML.System;
using System.Diagnostics;
using System.Data;
using System.Runtime.InteropServices;
using SFML.Graphics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace SFMLTest
{
    public class Physics
    {
        /*
        List<Primitive2D> updateBuffer;

        Clock deltaTime;

        float gravity;

        int ticks = 0;

        */
        public float Gravity { private set; get; }
        public Physics(float Gravity)
        {
            this.Gravity = Gravity * 0.005f;
        }

        public void Update(List<PhysicsObject> Objects)
        {
            ConcurrentDictionary<PhysicsObject, Vector2f> FuturePosition = new ConcurrentDictionary<PhysicsObject, Vector2f>();

            Parallel.For(0, Objects.Count, i =>
            {
                if (Objects[i].IsSolid) return;

                PhysicsObject MainObj = Objects[i];

                Vector2f FutureVel = new Vector2f(MainObj.VelX, MainObj.VelY);

                FutureVel.Y += Gravity;

                for (int f = 0; f < Objects.Count; f++)
                {
                    if (f == i) continue;

                    PhysicsObject SecondObj = Objects[f];

                    // Wir kollidieren im nächsten Update!
                    // Jetzt schon Bouncen :D!
                    Vector2f CollisionPosition = new Vector2f();
                    Vector2f PreCollisionPosition = new Vector2f();
                    if (MainObj.IsInsideObjectInFuture(SecondObj, Gravity, 1, out CollisionPosition, out PreCollisionPosition) && 
                        !MainObj.IsInsideObject(SecondObj))
                    {
                        // TODO: Detect the origin of collision
                        // left, right, bottom OR Top-sided! It can either be one. In very RARE cases it can be a real Edge!
                        
                        Vector2f FixedPos = MainObj.Position;
                        FuturePosition.AddOrUpdate(MainObj, CollisionPosition, (key, value) => CollisionPosition);

                        Vector2f CollisionArea = new Vector2f();

                        if (MainObj.VelY > 0) // We're Falling
                        {
                            float overlappingArea = Math.Abs(MainObj.Position.Y + MainObj.Height - SecondObj.Position.Y);
                            CollisionArea.Y = overlappingArea;
                        } else if (MainObj.VelY < 0)
                        {
                            float overlappingArea = Math.Abs(MainObj.Position.Y - (SecondObj.Position.Y + SecondObj.Height));
                            CollisionArea.Y = overlappingArea;
                        }

                        // Calculate the Collision Area for X too
                        if (MainObj.VelX > 0) // We're going to the right side
                        {
                            float overlappingArea = Math.Abs(MainObj.Position.X + MainObj.Width - SecondObj.Position.X);
                            CollisionArea.X = overlappingArea;
                        }
                        else if (MainObj.VelX < 0)
                        {
                            float overlappingArea = Math.Abs(MainObj.Position.X - (SecondObj.Position.X + SecondObj.Width));
                            CollisionArea.X = overlappingArea;
                        }

                        if (CollisionArea.X >= CollisionArea.Y)
                        {
                            FutureVel.Y *= -MainObj.Bounciness;
                            MainObj.Color = Color.Yellow;
                        }
                        if (CollisionArea.Y >= CollisionArea.X)
                        {
                            FutureVel.X *= -MainObj.Bounciness;
                            MainObj.Color = Color.Red;
                        }
                    }
                }

                MainObj.VelX = FutureVel.X;
                MainObj.VelY = FutureVel.Y;

                //if (!FuturePosition.ContainsKey(MainObj))
                    FuturePosition.TryAdd(MainObj, MainObj.Position + FutureVel);
            });
            // TODO:
            /*
            for (int i = 0; i < Objects.Count; i++)
            {
            }
            */

            foreach(var kv in FuturePosition)
            {
                if (kv.Key != null)
                    kv.Key.Position = kv.Value;
            }
        }
    }
}
