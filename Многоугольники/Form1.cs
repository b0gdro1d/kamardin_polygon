using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Многоугольники
{
    public partial class Form1 : Form
    {
        int dxmax = 0, dymax = 0;
        int x_move, y_move;
        bool Check = false;
        List<Shape> shapes = new List<Shape>();
        byte formTop = 1;
        Color Line = Color.Black;
        Color Fill = Color.White;
        bool alg = false;
        Timer timer = new Timer();
        static Random rnd = new Random();
        int t = 10;
        public Form1()
        {
            InitializeComponent();
            //Compare(5);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                for (int i = shapes.Count - 1; i >= 0; i--)
                {
                    if (shapes[i].IsInside(e.X, e.Y))
                    {
                        shapes.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {

                foreach (Shape shape in shapes)
                {
                    if (shape.IsInside(e.X, e.Y))
                    {
                        Check = true;
                        shape.Taken = true;
                        shape.dx = e.X - shape.x;
                        shape.dy = e.Y - shape.y;
                    }
                }
                if (Check == false)
                { 
                    
                    /*if (ShouldDrag(e.X, e.Y, shapes) == true)
                    {
                        foreach (Shape shape in shapes)
                        {
                            shape.Taken = true;
                            shape.dx = e.X - shape.x;
                            shape.dy = e.Y - shape.y;
                        }
                            
                    }
                    else*/
                    {
                        if (formTop == 1) shapes.Add(new Circle(e.X, e.Y));
                        if (formTop == 2) shapes.Add(new Triangle(e.X, e.Y));
                        if (formTop == 3) shapes.Add(new Square(e.X, e.Y));
                    }
                }
            }
            this.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
             if (Check)
            {
                foreach (Shape shape in shapes)
                {
                    if (shape.Taken)
                    {
                        shape.x = e.X - shape.dx;
                        shape.y = e.Y - shape.dy;
                    }

                }
                this.Refresh();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Check = false;
            foreach (Shape shape in shapes)
            {
                shape.Taken = false;
            }
            if (shapes.Count > 2)
            {
                for (int i = 0; i < shapes.Count; i++)
                {
                    if (shapes[i].Poly == false)
                    {
                        shapes.RemoveAt(i);
                        i = 0;
                    }
                }
            }
            this.Refresh();
        }

        private bool ShouldDrag (int x, int y, List <Shape> shhapes)
        {
            if (shapes.Count > 2)
            {
                List<Shape> shhhapes = new List<Shape>();
                Shape circle = new Circle(x, y);
                shhhapes = shhapes;
                shhapes.Add(circle);
                bool left = false;
                bool right = false;
                double k;
                double b;
                for (int i = 0; i < shhapes.Count; i++)
                {
                    for (int j = i + 1; j < shhapes.Count; j++)
                    {
                        left = false;
                        right = false;
                        if (shhapes[i].x != shhapes[j].x)
                        {
                            k = (double)(shhapes[i].y - shhapes[j].y) / (shhapes[i].x - shhapes[j].x);
                            b = shhapes[i].y - (k * shhapes[i].x);
                            for (int f = 0; f < shhapes.Count; f++)
                            {
                                if (f != i && f != j)
                                {
                                    if (shhapes[f].y <= (shhapes[f].x * k + b))
                                    {
                                        left = true;
                                    }
                                    else right = true;
                                }
                            }

                            if (left != right)
                            {
                                shhhapes.Add(shhapes[i]);
                                shhhapes.Add(shhapes[j]);
                                //g.DrawLine(new Pen(Line), shapes[i].x, shapes[i].y, shapes[j].x, shapes[j].y);
                            }
                        }
                        else
                        {
                            for (int f = 0; f < shhapes.Count; f++)
                            {
                                if (f != i && f != j)
                                {
                                    if (shhapes[f].x <= shhapes[i].x)
                                    {
                                        left = true;
                                    }
                                    else right = true;
                                }
                            }
                            if (left != right)
                            {
                                shhhapes.Add(shhapes[i]);
                                shhhapes.Add(shhapes[j]);
                                //g.DrawLine(new Pen(Line), shapes[i].x, shapes[i].y, shapes[j].x, shapes[j].y);
                            }
                        }
                    }
                }
                if (!shhhapes.Contains(circle))
                {
                    shhapes.Remove(circle);
                    return true;
                }
                shhapes.Remove(circle);
                
            }
            return false;
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Shape shape in shapes)
                shape.Poly = false;
            Graphics g = e.Graphics;
            if (shapes.Count >= 3)
            {
                if (alg) Djarvis(e);
                else DefitionAl(shapes, g);
            }
            foreach (Shape shape in shapes)
            {
                shape.Draw(g);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            this.Invalidate();
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formTop = 1;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formTop = 2;
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formTop = 3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                Line = colorDialog1.Color;
                Shape.LineC = colorDialog1.Color;
                this.Refresh();
            }
        }

        private void DefitionAl(List<Shape> shapes, Graphics g)
        {
            bool left = false;
            bool right = false;
            double k;
            double b;
            if (shapes.Count > 2)
            {
                for (int n = 0; n < shapes.Count; n++)
                {
                    shapes[n].Poly = false;
                }

                for (int i = 0; i < shapes.Count; i++)
                {
                    for (int j = i + 1; j < shapes.Count; j++)
                    {
                        left = false;
                        right = false;
                        if (shapes[i].x != shapes[j].x)
                        {
                            k = (double)(shapes[i].y - shapes[j].y) / (shapes[i].x - shapes[j].x);
                            b = shapes[i].y - (k * shapes[i].x);
                            for (int f = 0; f < shapes.Count; f++)
                            {
                                if (f != i && f != j)
                                {
                                    if (shapes[f].y <= (shapes[f].x * k + b))
                                    {
                                        left = true;
                                    }
                                    else right = true;
                                }
                            }

                            if (left != right)
                            {
                                shapes[i].Poly = true;
                                shapes[j].Poly = true;
                                g.DrawLine(new Pen(Line), shapes[i].x, shapes[i].y, shapes[j].x, shapes[j].y);
                            }
                        }
                        else
                        {
                            for (int f = 0; f < shapes.Count; f++)
                            {
                                if (f != i && f != j)
                                {
                                    if (shapes[f].x <= shapes[i].x)
                                    {
                                        left = true;
                                    }
                                    else right = true;
                                }
                            }
                            if (left != right)
                            {
                                shapes[i].Poly = true;
                                shapes[j].Poly = true;
                                g.DrawLine(new Pen(Line), shapes[i].x, shapes[i].y, shapes[j].x, shapes[j].y);
                            }
                        }
                    }
                }
            }
            foreach (Shape shape in shapes)
            {
                shape.Draw(g);
            }
        }
        private void Djarvis(PaintEventArgs e)
        {
            foreach (Shape shape in shapes)
            {
                shape.Poly = false;
            }
            List<Shape> poly = new List<Shape>();
            poly.Clear();
            Shape A = shapes[0];
            foreach (Shape shape in shapes)
            {
                if (shape.y > A.y) A = shape;
                else if (shape.y == A.y)
                {
                    if (shape.x < A.x) A = shape;
                }
            }
            poly.Add(A);
            double min = 2;
            Point M = new Point(A.x - 1000, A.y);
            Shape P = shapes[0];
            for (int i = 0; i < shapes.Count; i++)
            {
                if (Cos(shapes[i], A, M) < min)
                {
                    min = Cos(shapes[i], A, M);
                    P = shapes[i];
                }
            }
            e.Graphics.DrawLine(new Pen(new SolidBrush(Line)), new Point(A.x, A.y), new Point(P.x, P.y));
            A.Poly = true;
            P.Poly = true;
            Shape end = A, next = P;
            do
            {
                min = 2000;
                for (int i = 0; i < shapes.Count; i++)
                {
                    if (Cos(A, P, new Point(shapes[i].x, shapes[i].y)) < min)
                    {
                        min = Cos(A, P, new Point(shapes[i].x, shapes[i].y));
                        next = shapes[i];
                    }
                }
                e.Graphics.DrawLine(new Pen(new SolidBrush(Line)), new Point(P.x, P.y), new Point(next.x, next.y));
                next.Poly = true;
                A = P;
                P = next;
                poly.Add(P);
            } while (P != end);
            double Cos(Shape start, Shape finish, Point point)
            {
                Point v1 = new Point(finish.x - start.x, finish.y - start.y);
                Point v2 = new Point(finish.x - point.X, finish.y - point.Y);
                return ((v1.X * v2.X) + (v1.Y * v2.Y)) / (Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y) * Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                Fill = colorDialog1.Color;
                Shape.FillC = colorDialog1.Color;
                this.Refresh();
            }
        }

        private void definitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alg = false;
        }

        private void djarvisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alg = true;
        }
        private void Compare(int n)
        {
            List<Shape> shapes = new List<Shape>();
            for (int i = 0; i < n; i++)
            {
                shapes.Add(new Circle(i*15, i * 10 + 10));
            }
            TimeSpan time_Def, time_Djar;
            time_Def = Def_time(shapes).Elapsed;
            time_Djar = Djarvis_time(shapes).Elapsed;
            Console.WriteLine("{0} - Definition\n{1} - Djarvis", time_Def, time_Djar);
        }
        private Stopwatch Def_time(List<Shape> shapes)
        {
            Stopwatch time = Stopwatch.StartNew();
            for (int l = 0; l < shapes.Count; l++)
            {
                bool left = false;
                bool right = false;
                double k;
                double b;
                if (shapes.Count > 2)
                {
                    for (int n = 0; n < shapes.Count; n++)
                    {
                        shapes[n].Poly = false;
                    }

                    for (int i = 0; i < shapes.Count; i++)
                    {
                        for (int j = i + 1; j < shapes.Count; j++)
                        {
                            left = false;
                            right = false;
                            if (shapes[i].x != shapes[j].x)
                            {
                                k = (double)(shapes[i].y - shapes[j].y) / (shapes[i].x - shapes[j].x);
                                b = shapes[i].y - (k * shapes[i].x);
                                for (int f = 0; f < shapes.Count; f++)
                                {
                                    if (f != i && f != j)
                                    {
                                        if (shapes[f].y <= (shapes[f].x * k + b))
                                        {
                                            left = true;
                                        }
                                        else right = true;
                                    }
                                }

                                if (left != right)
                                {
                                    shapes[i].Poly = true;
                                    shapes[j].Poly = true;
                                    //g.DrawLine(new Pen(Line), shapes[i].x, shapes[i].y, shapes[j].x, shapes[j].y);
                                }
                            }
                            else
                            {
                                for (int f = 0; f < shapes.Count; f++)
                                {
                                    if (f != i && f != j)
                                    {
                                        if (shapes[f].x <= shapes[i].x)
                                        {
                                            left = true;
                                        }
                                        else right = true;
                                    }
                                }
                                if (left != right)
                                {
                                    shapes[i].Poly = true;
                                    shapes[j].Poly = true;
                                    //g.DrawLine(new Pen(Line), shapes[i].x, shapes[i].y, shapes[j].x, shapes[j].y);
                                }
                            }
                        }
                    }
                }

            }
            time.Stop();
            return time;
        }
        private Stopwatch Djarvis_time(List<Shape> shapes)
        {
            Stopwatch time = Stopwatch.StartNew();
            List<Shape> poly = new List<Shape>();
            poly.Clear();
            Shape A = shapes[0];
            foreach (Shape shape in shapes)
            {
                if (shape.y > A.y) A = shape;
                else if (shape.y == A.y)
                {
                    if (shape.x < A.x) A = shape;
                }
            }
            poly.Add(A);
            double min = 2000;
            Point M = new Point(A.x - 1000, A.y);
            Shape P = shapes[0];
            for (int i = 0; i < shapes.Count; i++)
            {
                if (Cos(shapes[i], A, M) < min)
                {
                    min = Cos(shapes[i], A, M);
                    P = shapes[i];
                }
            }
            //e.Graphics.DrawLine(new Pen(new SolidBrush(Line)), new Point(A.x, A.y), new Point(P.x, P.y));
            A.Poly = true;
            P.Poly = true;
            Shape end = A, next = P;
            do
            {
                min = 2000;
                for (int i = 0; i < shapes.Count; i++)
                {
                    if (Cos(A, P, new Point(shapes[i].x, shapes[i].y)) < min)
                    {
                        min = Cos(A, P, new Point(shapes[i].x, shapes[i].y));
                        next = shapes[i];
                    }
                }
                //e.Graphics.DrawLine(new Pen(new SolidBrush(Line)), new Point(P.x, P.y), new Point(next.x, next.y));
                next.Poly = true;
                A = P;
                P = next;
                poly.Add(P);
            } while (P != end);
            double Cos(Shape start, Shape finish, Point point)
            {
                Point v1 = new Point(finish.x - start.x, finish.y - start.y);
                Point v2 = new Point(finish.x - point.X, finish.y - point.Y);
                return ((v1.X * v2.X) + (v1.Y * v2.Y)) / (Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y) * Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y));
            }
            time.Stop();
            return time;
        }


        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Interval = t;
            timer.Tick += new EventHandler(TimerOnTick);
            timer.Start();
        }

        private void changeIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            string s = this.textBoxt.Text;
            t = Convert.ToInt32(s);
            timer.Interval = t;
            timer.Start();
        }

        private void TimerOnTick (object obj, EventArgs ea)
        {
             timer = (Timer)obj;
            if (dxmax <= 10 && dymax <= 15 && dxmax >= -15 && dymax >= -15)
            {
                x_move = rnd.Next(-5, 5);
                y_move = rnd.Next(-5, 5);
                dxmax += x_move;
                dymax += y_move;
                foreach (Shape shape in shapes)
                {
                    shape.x += x_move;
                    shape.y += y_move;
                }
            }
            else
            {
                if (dxmax > 10)
                {
                    x_move += -5;
                    dxmax += x_move;
                    foreach (Shape shape in shapes)
                    {
                        shape.x += x_move;
                    }
                }
                if (dxmax < -10)
                {
                    x_move += 5;
                    dxmax += x_move;
                    foreach (Shape shape in shapes)
                    {
                        shape.x += x_move;
                    }
                }
                if (dymax > 10)
                {
                    y_move += -5;
                    dymax += y_move;
                    foreach (Shape shape in shapes)
                    {
                        shape.y += y_move;
                    }
                }
                if (dymax < -10)
                {
                    y_move += 5;
                    dymax += y_move;
                    foreach (Shape shape in shapes)
                    {
                        shape.y += y_move;
                    }
                }
            }
            this.Refresh();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }
    }
}
