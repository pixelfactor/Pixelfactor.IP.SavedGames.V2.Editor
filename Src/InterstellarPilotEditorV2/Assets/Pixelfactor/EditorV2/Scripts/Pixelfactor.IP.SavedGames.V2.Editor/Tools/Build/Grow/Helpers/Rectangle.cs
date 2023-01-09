using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow
{
    /// <summary>
    /// A Precision Rectangle is similar in definition to a XNA Rectangle but using
    /// floats. The point X/Y is defined as the top left of the rectangle.
    /// </summary>
    public class Rectangle
    {
        public float X = 0.0f;
        public float Y = 0.0f;
        protected float width = 0.0f;
        protected float height = 0.0f;

        public float Width
        {
            set
            {
                this.width = value;
                if (this.width < 0.0f)
                    throw new System.ArgumentException();
            }
            get { return this.width; }
        }

        public float Height
        {
            set
            {
                this.height = value;
                if (this.height < 0.0f)
                    throw new System.ArgumentException();
            }
            get { return this.height; }
        }

        public float Top
        {
            set { this.Y = value; }
            get { return this.Y; }
        }

        public float Bottom
        {
            set { this.Y = value + this.height; }
            get { return this.Y - this.height; }
        }

        public float Left
        {
            set { this.X = value; }
            get { return this.X; }
        }

        public float Right
        {
            set { this.X = value - this.width; }
            get { return this.X + this.width; }
        }

        public Rectangle() { }

        public Rectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            if (width < 0.0f || height < 0.0f)
                throw new System.ArgumentException("Negative parameter");
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Creates a rectangle from two points
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public Rectangle(Vector2 p1, Vector2 p2)
        {
            this.X = Mathf.Min(p1.x, p2.x);
            this.Y = Mathf.Max(p1.y, p2.y);
            this.width = Mathf.Max(p1.x, p2.x) - this.X;
            this.height = this.Y - Mathf.Min(p1.y, p2.y);
        }

        public bool Intersects(Rectangle rect2)
        {
            if ((this.Left >= rect2.Left && this.Left <= rect2.Right) ||
                (this.Right >= rect2.Left && this.Right <= rect2.Right) ||
                (this.Left <= rect2.Left && this.Right >= rect2.Right))
                if ((this.Top >= rect2.Bottom && this.Top <= rect2.Top) ||
                    (this.Bottom >= rect2.Bottom && this.Bottom <= rect2.Top) ||
                    (this.Top >= rect2.Top && this.Bottom <= rect2.Bottom))
                    return true;
            return false;
        }

        public bool Contains(Rectangle obj2)
        {
            if (obj2.Left >= this.Left && obj2.Right <= this.Right)
                if (obj2.Bottom >= this.Bottom && obj2.Top <= this.Top)
                    return true;
            return false;
        }

        public bool Contains(Vector2 position)
        {
            if (position.x >= this.Left && position.x <= this.Right)
                if (position.y >= this.Bottom && position.y <= this.Top)
                    return true;
            return false;
        }

        public Rectangle Merge(Rectangle obj2)
        {
            float x = Mathf.Min(this.Left, obj2.Left);
            float width = Mathf.Max(this.Right, obj2.Right) - x;
            float y = Mathf.Max(this.Top, obj2.Top);
            float height = y - Mathf.Min(this.Bottom, obj2.Bottom);
            return new Rectangle(x, y, width, height);
        }

        public Vector2 Center
        {
            get { return new Vector2(this.X + (this.Width / 2.0f), this.Bottom + (this.Height / 2.0f)); }
        }

        public Rectangle CalcIntersectingRectangle(Rectangle rect2)
        {
            Rectangle collisionRectangle = new Rectangle();
            if (this.Intersects(rect2))
            {
                if (this.Left >= rect2.Left &&
                    this.Left <= rect2.Right)
                    collisionRectangle.X = this.Left;
                else
                    collisionRectangle.X = rect2.Left;
                if (this.Right >= rect2.Left &&
                    this.Right <= rect2.Right)
                    collisionRectangle.Width = this.Right - collisionRectangle.X;
                else
                    collisionRectangle.Width = rect2.Right - collisionRectangle.X;

                if (this.Top <= rect2.Top && this.Top >= rect2.Bottom)
                    collisionRectangle.Y = this.Top;
                else
                    collisionRectangle.Y = rect2.Top;
                if (this.Bottom <= rect2.Top && this.Bottom >= rect2.Bottom)
                    collisionRectangle.Height = collisionRectangle.Y - this.Bottom;
                else
                    collisionRectangle.Height = collisionRectangle.Y - rect2.Bottom;
            }
            return collisionRectangle;
        }
    }
}
