using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Многоугольники
{
    abstract class Shape
    {
        protected int X;
        protected int Y;
        protected static int R;
        public static Color LineC;
        public static Color FillC;
        protected bool IsTaken = true;
        protected int dX;
        protected int dY;
        protected bool poly;
        static Shape()
        {
            R = 25;
            LineC = Color.Black;
            FillC = Color.White;
        }
        public Shape(int x, int y)
        {
            X = x;
            Y = y;
        }
        public abstract bool IsInside(int xx, int yy);
        public abstract void Draw(Graphics G);
        public int x
        {
            get
            {
                return X;
            }
            set
            {
                X = value;
            }
        }
        public int y
        {
            get
            {
                return Y;
            }
            set
            {
                Y = value;
            }
        }
        public int dx
        {
            get
            {
                return dX;
            }
            set
            {
                dX = value;
            }
        }
        public int dy
        {
            get
            {
                return dY;
            }
            set
            {
                dY = value;
            }
        }
        public bool Taken
        {
            get
            {
                return IsTaken;
            }
            set
            {
                IsTaken = value;
            }
        }
        public bool Poly
        {
            get
            {
                return poly;
            }
            set
            {
                 poly = value;
            }
        }
        public Color line
        {
            get
            {
                return LineC;
            }
            set
            {
                LineC = value;
            }
        }
    }
    class Circle : Shape
    {
        public Circle(int X, int Y)
            : base(X, Y)
        {

        }
        public override void Draw(Graphics G)
        {
            Pen pen = new Pen(LineC, 1);
            SolidBrush brush = new SolidBrush(FillC);
            G.FillEllipse(brush, X - R, Y - R, 2 * R, 2 * R);
            G.DrawEllipse(pen, X - R, Y - R, 2 * R, 2 * R);            
        }
        public override bool IsInside(int xx, int yy)
        {
            return ((X+R-xx) * (X - R - xx) + (Y + R - yy)* (Y -  R - yy) <= R * R);
        }
    }
    class Triangle : Shape
    {
        public Triangle(int X, int Y)
            : base(X, Y)
        {

        }
        public override void Draw(Graphics G)
        {
            double a = R * Math.Sqrt(3) / 2;
            Pen pen = new Pen(LineC, 1);
            SolidBrush brush = new SolidBrush(FillC);
            Point[] points = new Point[3];
            points[0] = new Point(X - (int) a, Y + (int) a/2);
            points[1] = new Point(X, Y - R);
            points[2] = new Point(X + (int)a, Y + (int) a / 2);
            G.FillPolygon(brush, points);
            G.DrawPolygon(pen, points);  
        }
        public override bool IsInside(int xx, int yy)
        {
            return Y-yy >= (-2*(X-xx) - R) && (Y-yy >= 2 * (X-xx) - R) && (Y-yy <= R/2);
        }
    }
    class Square : Shape
    {
        public Square(int X, int Y)
            : base(X, Y)
        {

        }
        private double a = R * Math.Sqrt(2);
        public override void Draw(Graphics G)
        {
            double a = R * Math.Sqrt(2) / 2;
            Pen pen = new Pen(LineC, 1);
            SolidBrush brush = new SolidBrush(FillC);
            Point[] points = new Point[4];
            points[0] = new Point(X - (int)a, Y + (int)a);
            points[1] = new Point(X + (int)a, Y + (int)a);
            points[2] = new Point(X + (int)a, Y - (int)a);
            points[3] = new Point(X - (int)a, Y - (int)a);
            G.FillPolygon(brush, points);
            G.DrawPolygon(pen, points);   
        }
        public override bool IsInside(int xx, int yy)
        {
            if ((xx <= X + a/2) && (xx >= X - a/2))
            {
                if ((yy <= Y + a/2) && (yy >= Y - a/2)) return true;
                else return false;
            }
            else return false;
        }
    }
}
