namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Build.Grow
{
    using UnityEngine;

    public class Line
    {
        public static Vector2? Intersection(
            Vector2 p1,
            Vector2 p2,
            Vector2 q1,
            Vector2 q2)
        {
            float? dist = IntersectionDist(p1, p2, q1, q2);
            if (dist.HasValue)
                return new Vector2(p1.x + dist.Value * (p2.x - p1.x), p1.y + dist.Value * (p2.y - p1.y));
            return null;
        }

        public static float? IntersectionDist(
            Vector2 p1,
            Vector2 p2,
            Vector2 q1,
            Vector2 q2)
        {
            // Ignore zero length lines
            if (p1 == p2 || q1 == q2)
                return null;
            // Return any value for coincidental lines
            //if (p1 == q1 && p2 == q2)
            //    return 0.0f;

            float demon = ((q2.y - q1.y) * (p2.x - p1.x) - (q2.x - q1.x) * (p2.y - p1.y));

            float a = ((q2.x - q1.x) * (p1.y - q1.y) - (q2.y - q1.y) * (p1.x - q1.x)) / demon;

            float b = ((p2.x - p1.x) * (p1.y - q1.y) - (p2.y - p1.y) * (p1.x - q1.x)) / demon;

            // Lines are parallel
            //if (demon == 0.0f)
            //    return 0.0f;

            if (a > 0.0f && a < 1.0f && b > 0.0f && b < 1.0f)
            {
                //if ((a > 0.0f && a < 1.0f))
                return a;
                //else
                //    return new Vector2(p1.X + a * (p2.X - p1.X), p1.Y + a * (p2.Y - p1.Y));
            }
            return null;
        }

        public static Vector2? Intersection(Line line1, Line line2, bool line1Infinite,
            bool line2Infinite)
        {
            // Do some checks to make sure the lines can logically intersect
            if (!(line1.IsZerolLength || line2.IsZerolLength))
            {
                // calculate the gradiants of both lines
                float? line1Gradiant = line1.Gradiant;
                float? line2Gradiant = line2.Gradiant;
                float? poiX = null;
                float? poiY = null;
                if (line1Gradiant != line2Gradiant || (line1.isHorizontal && line2.isVertical)
                    || (line1.isVertical && line1.isHorizontal))
                {
                    if (line1.isVertical)
                        poiX = line1.p1.x;
                    else if (line2.isVertical)
                        poiX = line2.p1.x;
                    if (line1.isHorizontal)
                        poiY = line1.p1.y;
                    else if (line2.isHorizontal)
                        poiY = line2.p1.y;
                    if (poiX == null)
                    {
                        float diffInX = ((float)line1Gradiant - (float)line2Gradiant);
                        poiX = ((float)line2.YIntersect - (float)line1.YIntersect) / diffInX;
                    }
                    if (poiY == null)
                    {
                        float x1 = 0.0f;
                        float y1 = 0.0f;
                        if (line1Gradiant != null)
                        {
                            x1 = (float)line1Gradiant * (float)poiX;
                            y1 = (float)line1.YIntersect;
                        }
                        else
                        {
                            x1 = (float)line2Gradiant * (float)poiX;
                            y1 = (float)line2.YIntersect;
                        }
                        poiY = x1 + y1;
                    }
                    Vector2 poi = new Vector2((float)poiX, (float)poiY);
                    if ((line1Infinite || line1.ToRectangle().Contains(poi)) &&
                        (line2Infinite || line2.ToRectangle().Contains(poi)))
                        return poi;

                }
            }
            return null;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(this.p1, this.p2);
        }

        protected Vector2 p1 = Vector2.zero;
        protected Vector2 p2 = Vector2.zero;
        protected bool isVertical = false;
        protected bool isHorizontal = false;
        protected float length = 0.0f;
        protected float? gradiant = null;
        public Vector2 P1
        {
            get { return this.p1; }
            set
            {
                this.p1 = value;
                this.Recalculate();
            }
        }

        public Vector2 P2
        {
            get { return this.p2; }
            set
            {
                this.p2 = value;
                this.Recalculate();
            }
        }

        public Vector2 Direction
        {
            get { return new Vector2(this.p2.x - this.p1.x, this.p2.y - this.p1.y); }
        }

        protected void Recalculate()
        {
            this.isHorizontal = this.p1.y == this.p2.y;
            this.isVertical = this.p1.x == this.p2.x;
            if (this.isVertical)
                this.gradiant = null;
            else
                this.gradiant = (this.p1.y - this.p2.y) / (this.p1.x - this.p2.x);
            this.length = Vector2.Distance(this.p1, this.p2);
        }

        public Line() { }

        public Line(Vector2 point1, Vector2 point2)
        {
            this.P1 = point1;
            this.P2 = point2;
        }

        public bool IsVertical
        {
            get { return this.isVertical; }
        }

        public bool IsHorizontal
        {
            get { return this.isHorizontal; }
        }

        public bool IsZerolLength
        {
            get
            {
                if (this.p1 == this.p2)
                    return true;
                return false;
            }
        }

        public float? YIntersect
        {
            get
            {
                if (this.gradiant != null)
                    return this.p1.y - (float)this.Gradiant * this.p1.x;
                return null;
            }
        }

        public float? Gradiant
        {
            get { return this.gradiant; }
        }

        public float Length
        {
            get { return this.length; }
        }
    }
}
