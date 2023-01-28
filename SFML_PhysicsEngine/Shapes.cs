using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;


namespace SFMLTest.Shapes
{
    class Primitive2D
    {
        static int static_idnum = 0;

        public Sprite sp;
        public string id;
        public int index;

        public Vector2f position;

        // Potentiell ein Vector (x und y Speed)
        public Vector2f velocity;
        public float bounciness;

        Texture texture;

        public bool isAffectedByGravity;

        public bool isColliding;

        public Primitive2D(bool gravity, string id, Vector2f position, float bounciness)
        {
            this.id = id;
            index = static_idnum;
            static_idnum++;

            texture = new Texture("texture.jpg");
            sp = new Sprite(texture);
            this.position = this.sp.Position = position;
            sp.Color = Color.White;

            this.bounciness = bounciness;
            isAffectedByGravity = gravity;

            velocity = new Vector2f(0.0f,0.0f);

            isColliding= false;
        }

        public Primitive2D(bool gravity, string id, Vector2f position, float bounciness, Texture texture)
        {
            this.id = id;
            index = static_idnum;
            static_idnum++;

            this.texture = texture;
            sp = new Sprite(texture);
            this.position = this.sp.Position = position;

            this.bounciness = bounciness;

            velocity = new Vector2f(0.0f, 0.0f);
        }

        public void UpdatePos(Vector2f newPos)
        {
            position = newPos;
            sp.Position = newPos;
        }

        // yup
        public bool IsInsideRect(Primitive2D rect2)
        {
            if (position.X + texture.Size.X > rect2.position.X &&
                position.X < rect2.position.X + rect2.texture.Size.X &&
                position.Y + texture.Size.Y > rect2.position.Y &&
                position.Y < rect2.position.Y + rect2.texture.Size.Y)
                return true;
            return false; 
        }

        // yup
        public bool IsInsideRectFuture(Primitive2D rect2, float time, Vector2f vel, float gravity, int Ticks)
        {
            Vector2f future_pos = new Vector2f(position.X, position.Y);

            for (int i = 0; i < Ticks; i++)
            {
                //var dist = GetDistance(rect2);

                vel.Y += gravity * time;
                future_pos.X += vel.X;
                future_pos.Y += vel.Y;

                if (future_pos.X + texture.Size.X > rect2.position.X &&
                future_pos.X < rect2.position.X + rect2.texture.Size.X &&
                future_pos.Y + texture.Size.Y > rect2.position.Y &&
                future_pos.Y < rect2.position.Y + rect2.texture.Size.Y)
                    return true;

                // Distance is increasing, we cannot (and will not) collide in the near Future (and we also do not "bounce" in this prediction)
                //if (dist < GetDistance(rect2))
                //    return false;
            }

            return false;
            /*
            Vector2f future_pos = new Vector2f(position.X + vel.X + (gravity ), position.Y + vel.Y + (gravity * ticks));
            
            if (future_pos.X + texture.Size.X > rect2.position.X &&
                future_pos.X < rect2.position.X + rect2.texture.Size.X &&
                future_pos.Y + texture.Size.Y > rect2.position.Y &&
                future_pos.Y < rect2.position.Y + rect2.texture.Size.Y)
                return true;
            return false;
            */
        }

        public float GetDistance(Primitive2D pr2d)
        {
            float xd = pr2d.position.X - position.X;
            float yd = pr2d.position.Y - position.Y;

            return (float)Math.Sqrt(xd * xd + yd * yd);
        }

        public uint Width => texture.Size.X;
        public uint Height => texture.Size.Y;

        public void FixPosition(Primitive2D rect2)
        {

        }
    }
}
