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

        public void ApplyForce(PhysicsObject obj1, PhysicsObject obj2)
        {

        }

        public void Update(List<PhysicsObject> Objects)
        {
            ConcurrentDictionary<PhysicsObject, Vector2f> FuturePosition = new ConcurrentDictionary<PhysicsObject, Vector2f>();
            ConcurrentDictionary<PhysicsObject, Vector2f> FutureVelocities = new ConcurrentDictionary<PhysicsObject, Vector2f>();

            Parallel.For(0, Objects.Count, i =>
            {
                if (Objects[i].IsSolid) return;

                PhysicsObject MainObj = Objects[i];

                //MainObj.Transform.Rotate(1f, MainObj.GetCenterOfMass());

                //MainObj.Origin = new Vector2f(MainObj.Width / 2, MainObj.Height / 2);
                //MainObj.Rotation += .5f;
                
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


                        Vector2f CollisionArea = new Vector2f();
                        // m1 * v1i + m2 * v2i = m1 * v1f + m2 * v2f
                        // m1 * v1i + m2 * v2i = m1 * ((m1 * v1i + m2 * v2i - m2 * v2f) / m1) + m2 * v2f
                        // (m1 * v1i + m2 * v2i - m2 * v2f) / m1 = v1f

                        // v2f=((m1*(v1i-1)+m2*v2i)/(m2)) and v1f=1 or v2f=2 and m2=0 and v1f=v1i or v2f=4 and m1=0 and m2=0 and v1f=3

                        // v1f=((m1*v1i-m2*(v2f-v2i))/(m1))
                        FuturePosition.AddOrUpdate(MainObj, PreCollisionPosition, (key, value) => PreCollisionPosition);


                        if (MainObj.VelY > 0) // We're Falling
                        {
                            float overlappingArea = Math.Abs(MainObj.Position.Y + MainObj.Height - SecondObj.Position.Y);
                            CollisionArea.Y = overlappingArea;
                        }
                        else if (MainObj.VelY <= 0)
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
                        else if (MainObj.VelX <= 0)
                        {
                            float overlappingArea = Math.Abs(MainObj.Position.X - (SecondObj.Position.X + SecondObj.Width));
                            CollisionArea.X = overlappingArea;
                        }

                        if (CollisionArea.X >= CollisionArea.Y)
                        {
                            //FutureVel.Y *= -MainObj.Bounciness;
                            FutureVel.X -= FutureVel.X * MainObj.DragForce;

                            float mainVelFuture = (MainObj.Mass * FutureVel.Y) / (MainObj.Mass + SecondObj.Mass);

                            FutureVel.Y = mainVelFuture;

                            /*
                            Vector2f FourceMain = new Vector2f(MainObj.Mass * (MainObj.VelX * MainObj.VelX), MainObj.Mass * (MainObj.VelY * MainObj.VelY));
                            Vector2f Fource2nd = new Vector2f(SecondObj.Mass * (SecondObj.VelX * SecondObj.VelX), SecondObj.Mass * (SecondObj.VelY * SecondObj.VelY));

                            Vector2f newVelMain = new Vector2f((float)Math.Sqrt(FourceMain.X) * SecondObj.VelX, (float)Math.Sqrt(FourceMain.Y) * SecondObj.VelY);
                            Vector2f newVel2nd = new Vector2f((float)Math.Sqrt(Fource2nd.X) * SecondObj.VelX, (float)Math.Sqrt(Fource2nd.Y) * SecondObj.VelY);

                            if (!MainObj.IsSolid)
                            {
                                FutureVel = newVelMain;
                            }
                            if (!SecondObj.IsSolid)
                            {
                                SecondObj.VelX = newVel2nd.X;
                                SecondObj.VelY = newVel2nd.Y;
                            }
                            */
                            MainObj.Color = Color.Yellow;
                        }
                        else
                        {
                            PreCollisionPosition.Y += MainObj.VelY;
                        }

                        if (CollisionArea.Y >= CollisionArea.X)
                        {
                            //FutureVel.X *= -MainObj.Bounciness;

                            float mainVelFuture = (MainObj.Mass * FutureVel.X) / (MainObj.Mass + SecondObj.Mass);
                            float secondVelFuture = (SecondObj.Mass * SecondObj.VelX) / (MainObj.Mass + SecondObj.Mass);

                            FutureVel.X = mainVelFuture - secondVelFuture;

                                                        /*
                            Vector2f FourceMain = new Vector2f(MainObj.Mass * (MainObj.VelX * MainObj.VelX), MainObj.Mass * (MainObj.VelY * MainObj.VelY));
                            Vector2f Fource2nd = new Vector2f(SecondObj.Mass * (SecondObj.VelX * SecondObj.VelX), SecondObj.Mass * (SecondObj.VelY * SecondObj.VelY));

                            Vector2f newVelMain = new Vector2f((float)Math.Sqrt(FourceMain.X) * SecondObj.VelX, (float)Math.Sqrt(FourceMain.Y) * SecondObj.VelY);
                            Vector2f newVel2nd = new Vector2f((float)Math.Sqrt(Fource2nd.X) * SecondObj.VelX, (float)Math.Sqrt(Fource2nd.Y) * SecondObj.VelY);

                            if (!MainObj.IsSolid)
                            {
                                FutureVel = newVelMain;
                            }
                            if (!SecondObj.IsSolid)
                            {
                                SecondObj.VelX = newVel2nd.X;
                                SecondObj.VelY = newVel2nd.Y;
                            }
                            */

                            MainObj.Color = Color.Red;
                        } else
                        {
                            PreCollisionPosition.X += MainObj.VelX;
                        }

                    } else if (MainObj.IsInsideObject(SecondObj))
                    {
                        MainObj.Color = Color.Green;
                    }
                }

                //MainObj.VelX = FutureVel.X;
                //MainObj.VelY = FutureVel.Y;

                FutureVelocities.TryAdd(MainObj, FutureVel);

                FuturePosition.TryAdd(MainObj, MainObj.Position + FutureVel);
            });

            foreach (var kv in FuturePosition)
            {
                if (kv.Key != null)
                    kv.Key.Position = kv.Value;
            }

            foreach (var kv in FutureVelocities)
            {
                if (kv.Key != null)
                {
                    kv.Key.VelX = kv.Value.X;
                    kv.Key.VelY = kv.Value.Y;
                }
            }
        }
    }
}
