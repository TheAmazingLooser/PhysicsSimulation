using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml.Linq;

namespace SFMLTest
{
    public class PhysicsObject : Sprite
    {
        private static int indexes = 0;
        private int ID = 0;


        private Vector2f velocity = new Vector2f();


        // For future stuff.
        //private Vector2f friction = new Vector2f();
        // End Future Stuff

        public float Bounciness { get; set; } = 0.3f;

        public float DragForce { get; set; } = 0.05f;

        public float Mass { get; set; } = 1f;

        public float VelX
        {
            get => velocity.X;
            set
            {
                velocity.X = value;
            }
        }
        public float VelY
        {
            get => velocity.Y;
            set
            {
                velocity.Y = value;
            }
        }

        public PhysicsObject()
        {
            indexes++;
            ID = indexes;
        }

        public int Width => TextureRect.Width;
        public int Height => TextureRect.Height;

        public bool IsSolid { get; set; }

        public virtual bool IsInsideObject(PhysicsObject Obj)
        {
            return false;
        }

        public virtual bool IsInsideObjectInFuture(PhysicsObject Obj, float gravity, int Ticks, out Vector2f CollisionPosition, out Vector2f PreCollisionPosition)
        {
            CollisionPosition = new Vector2f(Position.X, Position.Y);
            PreCollisionPosition = new Vector2f(Position.X, Position.Y);
            return false;
        }

        public float GetDistance(PhysicsObject obj)
        {
            float xd = obj.Position.X - Position.X;
            float yd = obj.Position.Y - Position.Y;

            return (float)Math.Sqrt(xd * xd + yd * yd);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PhysicsObject);
        }
    
        public bool Equals(PhysicsObject other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Check all simple properties
            if (GetHashCode() != other.GetHashCode()
                || ID != other.ID)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public virtual Vector2f GetCenterOfMass()
        {
            return new Vector2f(Position.X + Width / 2, Position.Y + Height / 2);
        }
    }
}
