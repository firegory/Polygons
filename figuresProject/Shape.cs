using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.IO;

namespace figuresProject
{
    [Serializable]
    abstract class Shape
    {
        public int index;
        public static int nowind;
        public int x;
        public int y;
        static public int r;
        static public System.Drawing.Color color;
        public static List<Shape> allPoints;
        public static int pointType;
        [NonSerialized]
        public bool isDragged;
        [NonSerialized]
        public bool part;
        [NonSerialized]
        public Point mouseDiffernce;
        static Shape()
        {
            nowind = 0;
            pointType = 0;
            allPoints = new List<Shape>();
            r = 30;
            color = System.Drawing.Color.Red;
        }
        public Shape(int x1, int y1)
        {
            index = nowind;
            nowind++;
            part = true;
            isDragged = false;
            x = x1;
            y = y1;
        }
        public Shape(int x1, int y1, int ind)
        {
            index = ind;
            part = true;
            isDragged = false;
            x = x1;
            y = y1;
        }
        public abstract void draw(Graphics g);
        public abstract bool isInside(int xm, int ym);
    }
    [Serializable]
    class Circle : Shape
    {
        public Circle(int x1, int y1) : base(x1, y1)
        {
        }
        public override void draw(Graphics g)
        {
            Brush b = new SolidBrush(color);
            Pen p = new Pen(b);
            g.DrawEllipse(p, x - r, y - r, 2 * r, 2 * r);
        }
        public override bool isInside(int xm, int ym)
        {
            if ((x-xm)* (x - xm) + (y - ym)* (y - ym) <= r*r)
            {
                return true;
            }
            return false;
        }

    }
    [Serializable]
    class Triangle : Shape
    {
        private PointF point1;
        private PointF point2;
        private PointF point3;
        public Triangle(int x1, int y1) : base(x1, y1)
        {
        }
        public override void draw(Graphics g)
        {
            Brush b = new SolidBrush(color);
            Pen p = new Pen(b);
            point1 = new PointF(x, y - r);
            point2 = new PointF(x - (r * (float)Math.Sqrt(3) / 2), y + (r / 2));
            point3 = new PointF(x + (r * (float)Math.Sqrt(3) / 2), y + (r / 2));
            PointF[] points = new PointF[] {point1, point2, point3, point1};
            g.DrawLines(p, points);
        }
        public override bool isInside(int xm, int ym)
        {
            PointF pointm = new PointF(xm, ym);
            if (Math.Abs(squarethis - geron(pointm, point1 , point2)- geron(pointm, point1, point3) - geron(pointm, point2, point3)) <= 0.1)
            {
                return true;
            }
            return false;
        }
        private double squarethis
        {
            get
            {
                return r * r * 0.25 * Math.Sqrt(3)*3;
            }
        }
        private double distance(PointF pointf1, PointF pointf2)
        {
            return Math.Sqrt(((pointf1.X - pointf2.X) *(pointf1.X - pointf2.X)) + ((pointf1.Y - pointf2.Y) * (pointf1.Y - pointf2.Y)));
        }
        private double geron(PointF pointf1, PointF pointf2, PointF pointf3)
        {
            double a = distance(pointf1, pointf2);
            double b = distance(pointf1, pointf3);
            double c = distance(pointf2, pointf3);
            double p = (a + b + c) / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
    [Serializable]
    class Square : Shape
    {
        public Square(int x1, int y1) : base(x1, y1)
        {
        }
        public override void draw(Graphics g)
        {
            Brush b = new SolidBrush(color);
            Pen p = new Pen(b);
            PointF[] points = new PointF[] {new PointF(x- r2, y- r2), new PointF(x + r2, y - r2), new PointF(x + r2, y + r2), new PointF(x - r2, y + r2), new PointF(x - r2, y - r2) };
            g.DrawLines(p, points);
        }
        public override bool isInside(int xm, int ym)
        {
            if (xm > x - r2 && xm < x + r2 && ym > y - r2 && ym < y + r2)
            {
                return true;
            }
            return false;
        }
        private float r2
        {
            get
            {
                return r / (float)Math.Sqrt(2);
            }
        }
    }
}
