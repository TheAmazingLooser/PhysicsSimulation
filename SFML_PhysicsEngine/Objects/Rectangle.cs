using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFMLTest.Shapes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SFMLTest.Objects
{
    public class Rectangle : PhysicsObject
    {
        public Rectangle(uint Width, uint Height)
        {
            Texture = new Texture(Width, Height);
        }
        public Rectangle()
        {}

        public override bool IsInsideObject(PhysicsObject Obj)
        {
            if (Obj is Rectangle rect)
            {
                if (Position.X + Width > Obj.Position.X &&
                Position.X < Obj.Position.X + Obj.Width &&
                Position.Y + Height > Obj.Position.Y &&
                Position.Y < Obj.Position.Y + Obj.Height)
                    return true;
                return false;
            }
            return false;
        }

        // Test Rotation
        public bool IsInsideObjectRotation(PhysicsObject Obj)
        {
            // rpx = (radius * cos(angle)) + Middle
            // rPx = ( * Mathf.Cos(Rotation)) + GetCenterOfMass()

            // angle = Rotation
            // Middle = centerOfMass()
            // Center.X = Position.X + WIDTH / 2
            // Center = Postion.Y + HEIGHT / 2
            // radius = Math.Abs(



            if (Obj is Rectangle rect)
            {
                if (Position.X + Width > Obj.Position.X &&
                Position.X < Obj.Position.X + Obj.Width &&
                Position.Y + Height > Obj.Position.Y &&
                Position.Y < Obj.Position.Y + Obj.Height)
                    return true;
                return false;
            }
            return false;
        }

        Vector2f GetRotatedPosition(float angle, Vector2f center, float radius)
        {
            return new Vector2f((float)(radius * Math.Cos(angle) + center.X), (float)(radius * Math.Sin(angle) + center.X));
        }
        // End Test Rotation

        public override bool IsInsideObjectInFuture(PhysicsObject Obj, float gravity, int Ticks, out Vector2f CollisionPosition, out Vector2f PreCollisionPosition)
        {
            Vector2f future_pos_main = new Vector2f(Position.X, Position.Y);
            CollisionPosition = future_pos_main;
            PreCollisionPosition = future_pos_main;

            Vector2f velMain = new Vector2f(VelX, VelY);

            var Maxvel = Math.Max(0.01, Math.Max(VelX, VelY));
            float mult = 100;

            velMain.Y += gravity;

            for (int i = 0; i < Ticks * mult; i++)
            {
                var dist = GetDistance(Obj);


                if (!IsSolid)
                {
                    velMain.Y += gravity / mult;
                    future_pos_main.X += velMain.X / mult;
                    future_pos_main.Y += velMain.Y / mult;
                }

                if (future_pos_main.X + Width > Obj.Position.X &&
                future_pos_main.X < Obj.Position.X + Obj.Width &&
                future_pos_main.Y + Height > Obj.Position.Y &&
                future_pos_main.Y < Obj.Position.Y + Obj.Height)
                {
                    CollisionPosition = future_pos_main;
                    return true;
                }

                // Distance is increasing, we cannot (and will not) collide in the near Future (and we also do not "bounce" in this prediction)
                if (dist < GetDistance(Obj))
                    return false;
                
                PreCollisionPosition = new Vector2f(future_pos_main.X, future_pos_main.Y);
            }

            return false;
        }
    }
}
