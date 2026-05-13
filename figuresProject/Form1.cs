using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace figuresProject
{
    public delegate void RadiusChanged(object sender, RadiusEventArgs e);
    public partial class Form1 : Form
    {
        DateTime timePassedRad;
        int tempr;
        List<Shape> draggedPoints;
        bool drag;
        Form3 f3;
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            triangleToolStripMenuItem.Checked = true;
            jarvisToolStripMenuItem.Checked = true;
            Action.form = this;
            Action.label1 = label1;
            tempr = Shape.r;
            draggedPoints = new List<Shape>();
            timePassedRad = DateTime.Now;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!checkPerformanceToolStripMenuItem.Checked)
            {
                foreach (var item in Shape.allPoints)
                {
                    item.draw(e.Graphics);
                }
                if (Shape.allPoints.Count > 2)
                {
                    if (byDrfToolStripMenuItem.Checked)
                    {
                        Udobno.byDef(e);
                    }
                    else
                    {
                        Udobno.jarv(e);
                    }
                }
            }
            

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                List<Action> tAct = new List<Action>();
                bool a = true;
                foreach (Shape item in Shape.allPoints)
                {
                    if (item.isInside(e.X, e.Y))
                    {
                        a = false;
                        draggedPoints.Add(Udobno.DeepClone(item));
                        label1.Text = draggedPoints[0].x.ToString();
                        item.isDragged = true;
                        item.mouseDiffernce = new Point(e.X - (int)item.x, e.Y - (int)item.y);
                    }
                }
                if (a)
                {
                    Shape tempFig;
                    switch (Shape.pointType)
                    {
                        case 0:
                            tempFig = new Triangle(e.X, e.Y);
                            Shape.allPoints.Add(tempFig);
                            tAct.Add(new Create(Udobno.DeepClone(tempFig)));
                            break;
                        case 1:
                            tempFig = new Square(e.X, e.Y);
                            Shape.allPoints.Add(tempFig);
                            tAct.Add(new Create(Udobno.DeepClone(tempFig)));
                            break;
                        case 2:
                            tempFig = new Circle(e.X, e.Y);
                            Shape.allPoints.Add(tempFig);
                            tAct.Add(new Create(Udobno.DeepClone(tempFig)));
                            break;
                        default:
                            break;
                    }
                    Refresh();
                    if (!Shape.allPoints[Shape.allPoints.Count - 1].part)
                    {

                        Shape.allPoints.RemoveAt(Shape.allPoints.Count - 1);
                        tAct.RemoveAt(0);
                        drag = true;
                        foreach (Shape item in Shape.allPoints)
                        {
                            draggedPoints.Add(Udobno.DeepClone(item));
                            item.isDragged = true;
                            item.mouseDiffernce = new Point(e.X - (int)item.x, e.Y - (int)item.y);
                        }
                        Refresh();
                    }
                    else
                    {
                        Udobno.changed = true;
                    }
                }
                else
                {
                    drag = true;
                }
                if (!drag)
                {
                    List<Shape> tempP = new List<Shape>();
                    for (int i = 0; i < Shape.allPoints.Count; i++)
                    {
                        if (!Shape.allPoints[i].part)
                        {
                            tempP.Add(Udobno.DeepClone(Shape.allPoints[i]));
                            Shape.allPoints.RemoveAt(i);
                            Refresh();
                            i--;
                        }
                    }
                    if (tempP.Count !=0)
                    {
                        tAct.Add(new Delete(tempP));
                    }
                }
                if (tAct.Count !=0)
                {
                    Action.addHistory(new Combination(tAct));
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                for (int i = Shape.allPoints.Count-1; i >=0 ; i--)
                {
                    if (Shape.allPoints[i].isInside(e.X, e.Y))
                    {
                        Action.addHistory(new Delete(Udobno.DeepClone(Shape.allPoints[i])));
                        Udobno.changed = true;
                        Shape.allPoints.RemoveAt(i);
                        Refresh();
                        break;
                    }
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Udobno.changed = true;
                foreach (Shape item in Shape.allPoints)
                {
                    if (item.isDragged)
                    {
                        item.x = e.X - item.mouseDiffernce.X;
                        item.y = e.Y - item.mouseDiffernce.Y;
                    }
                }
                Refresh();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            List<Action> tAct = new List<Action>();
            List<Shape> temp = new List<Shape>();
            drag = false;
            foreach (Shape item in Shape.allPoints)
            {
                if (item.isDragged)
                {
                    item.isDragged = false;
                    temp.Add(Udobno.DeepClone(item));
                }
            }
            if (temp.Count != 0)
            {
                label1.Text = Action.nowi.ToString();
                tAct.Add(new Move(draggedPoints, temp));
                draggedPoints = new List<Shape>();
            }
            for (int i = 0; i < Shape.allPoints.Count; i++)
            {
                if (!Shape.allPoints[i].part)
                {
                    Udobno.changed = true;
                    tAct.Add(new Delete(Udobno.DeepClone(Shape.allPoints[i])));
                    Shape.allPoints.RemoveAt(i);
                    Refresh();
                    i--;
                }
            }
            if (tAct.Count !=0)
            {
                Action.addHistory(new Combination(tAct));
            }
        }

        private void triangleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Shape.pointType = 0;
            triangleToolStripMenuItem.Checked = true;
            circleToolStripMenuItem.Checked = false;
            squareToolStripMenuItem.Checked = false;
        }

        private void squareToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Shape.pointType = 1;
            triangleToolStripMenuItem.Checked = false;
            circleToolStripMenuItem.Checked = false;
            squareToolStripMenuItem.Checked = true;
        }

        private void circleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Shape.pointType = 2;
            triangleToolStripMenuItem.Checked = false;
            circleToolStripMenuItem.Checked = true;
            squareToolStripMenuItem.Checked = false;
        }

        private void byDrfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jarvisToolStripMenuItem.Checked = false;
            byDrfToolStripMenuItem.Checked = true;
            Refresh();
        }

        private void jarvisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jarvisToolStripMenuItem.Checked = true;
            byDrfToolStripMenuItem.Checked = false;
            Refresh();
        }

        private void checkPerformanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (byDrfToolStripMenuItem.Checked)
            {
                Point[] graph = new Point[50];
                int n = 10;
                graph[0].X = 0;
                graph[0].Y = 0;
                for (int i = 1; i < graph.Length; i++)
                {
                    graph[i].X = i * n;
                    graph[i].Y = (int)Udobno.byDef(i * n);
                }
                Form2 f2 = new Form2();
                f2.Show();
                f2.f2Show(null, graph, 2);
            }
            else
            {
                Point[] graph = new Point[50];
                int n = 300;
                graph[0].X = 0;
                graph[0].Y = 0;
                for (int i = 1; i < graph.Length; i++)
                {
                    graph[i].X = i * n;
                    graph[i].Y = (int)Udobno.jarv(i * n);
                }
                Form2 f2 = new Form2();
                f2.Show();
                f2.f2Show(graph, null, 1);
            }
            Refresh();
        }

        private void checkPerformanceAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point[] graphByDef = new Point[50];
            int n = 10;
            Point[] graphJarv = new Point[50];
            graphJarv[0].X = 0;
            graphJarv[0].Y = 0;
            for (int i = 1; i < graphJarv.Length; i++)
            {
                graphJarv[i].X = i * n;
                graphJarv[i].Y = (int)Udobno.jarv(i * n);
            }
            graphByDef[0].X = 0;
            graphByDef[0].Y = 0;
            for (int i = 1; i < graphByDef.Length; i++)
            {
                graphByDef[i].X = i * n;
                graphByDef[i].Y = (int)Udobno.byDef(i * n);
            }
            Form2 f2 = new Form2();
            f2.Show();
            f2.f2Show(graphJarv, graphByDef, 3);
        }

        private void radToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f3 == null || f3.IsDisposed)
            {
                f3 = new Form3();
                f3.Show();
                f3.RadiusChange += new RadiusChanged(DI);
                f3.setStartr(Shape.r);
            }
            else
            {
                f3.Activate();
                f3.WindowState = FormWindowState.Normal;
            }
        }
        private void DI(object sender, RadiusEventArgs e)
        {
            tempr = Shape.r;
            if (DateTime.Now - timePassedRad > TimeSpan.FromSeconds(1))
            {
                Action.addHistory(new ChangeRad(Shape.r, e.rad));

            }
            else
            {
                Action.changeRadValue(e.rad);
            }
            timePassedRad = DateTime.Now;
            Shape.r = e.rad;
            Refresh();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            Color tempColor = Shape.color;
            dialog.Color = Shape.color;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Shape.color = dialog.Color;
                Action.addHistory(new ChangeColor(tempColor, Shape.color));
                Refresh();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                Random r = new Random();
                foreach (Shape item in Shape.allPoints)
                {
                    item.x += r.Next(-9, 10);
                    item.y += r.Next(-9, 10);
                }
                for (int i = 0; i < Shape.allPoints.Count; i++)
                {
                    if (!Shape.allPoints[i].part)
                    {
                        Shape.allPoints.RemoveAt(i);
                        Refresh();
                        i--;
                    }
                }
                Refresh();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer1.Enabled =  true;
            toolStripButton2.Enabled = true;
            toolStripButton1.Enabled = false;
            Udobno.changed = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Udobno.saveAs();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Udobno.changed || Shape.allPoints.Count == 0)
            {
                Udobno.openFile(f3);
                Refresh();
            }
            else
            {
                string message = "You havent saved file, do you want to save it?";
                string caption = "Saving";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (Udobno.path != "none")
                    {
                        Udobno.WriteToBinaryFile();
                    }
                    else
                    {
                        Udobno.saveAs();
                    }
                    //this.Close();
                }
                if (!Udobno.canceled)
                {
                    Udobno.openFile(f3);
                    Shape.pointType = 0;
                    triangleToolStripMenuItem.Checked = true;
                    circleToolStripMenuItem.Checked = false;
                    squareToolStripMenuItem.Checked = false;
                    Refresh();
                }
                Udobno.canceled = false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Udobno.changed && Shape.allPoints.Count != 0)
            {
                string message = "You havent saved file, do you want to save it?";
                string caption = "Saving";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (Udobno.path != "none")
                    {
                        Udobno.WriteToBinaryFile();
                    }
                    else
                    {
                        Udobno.saveAs();
                    }
                    //this.Close();
                }
            }
            if (!Udobno.canceled)
            {
                if (f3 != null)
                {
                    f3.Close();
                }
                Shape.pointType = 0;
                triangleToolStripMenuItem.Checked = true;
                circleToolStripMenuItem.Checked = false;
                squareToolStripMenuItem.Checked = false;
                Shape.allPoints = new List<Shape>();
                Shape.r = 30;
                Shape.color = System.Drawing.Color.Red;
                Udobno.changed = false;
                Udobno.path = "none";
                Action.history = new List<Action>();
                Action.nowi = -1;
                Refresh();
            }
            Udobno.canceled = false;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Udobno.path != "none")
            {
                Udobno.WriteToBinaryFile();
            }
            else
            {
                Udobno.saveAs();
                Udobno.canceled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Udobno.changed && Shape.allPoints.Count != 0)
            {
                string message = "You havent saved file, do you want to save it?";
                string caption = "Saving";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (Udobno.path != "none")
                    {
                        Udobno.WriteToBinaryFile();
                    }
                    else
                    {
                        Udobno.saveAs();
                    }
                    //this.Close();
                }
                if (Udobno.canceled)
                {
                    e.Cancel = true;
                    Udobno.canceled = false;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode==Keys.Z)
            {
                if (e.Shift)
                {
                    Action.redoGlobal();
                }
                else
                {
                    Action.undoGlobal();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //undo
            Action.undoGlobal();
            
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //redo
            Action.redoGlobal();
        }
    }
    class Udobno
    {
        static private List<Shape> tempL;
        static public string path;
        static public bool changed;
        static public bool canceled;
        static Udobno()
        {
            canceled = false;
            changed = false;
            path = "none";
        }
        static public float findUgol(PointF m, int m1, int m2, List<Shape> list)
        {
            float a = (float)distance(new PointF(list[m1].x, list[m1].y), m);
            float b = (float)distance(new PointF(list[m1].x, list[m1].y), new PointF(list[m2].x, list[m2].y));
            return (((list[m1].x-m.X) * (list[m2].x - list[m1].x)) + ((list[m1].y - m.Y) * (list[m2].y - list[m1].y))) / (a * b);
        }
        static public float findUgol(int m, int m1, int m2, List<Shape> list)
        {
            float a = (float)distance(new PointF(list[m1].x, list[m1].y), new PointF(list[m].x, list[m].y));
            float b = (float)distance(new PointF(list[m1].x, list[m1].y), new PointF(list[m2].x, list[m2].y));
            return (((list[m1].x - list[m].x) * (list[m2].x - list[m1].x)) + ((list[m1].y - list[m].y) * (list[m2].y - list[m1].y))) / (a * b);
        }
        static public double distance(PointF pointf1, PointF pointf2)
        {
            return Math.Sqrt(((pointf1.X - pointf2.X) * (pointf1.X - pointf2.X)) + ((pointf1.Y - pointf2.Y) * (pointf1.Y - pointf2.Y)));

        }
        static public double distance(int p1, int p2, List<Shape> list)
        {
            PointF pointf1 = new PointF(list[p1].x, list[p1].y);
            PointF pointf2 = new PointF(list[p2].x, list[p2].y);
            return Math.Sqrt(((pointf1.X - pointf2.X) * (pointf1.X - pointf2.X)) + ((pointf1.Y - pointf2.Y) * (pointf1.Y - pointf2.Y)));
        }
        static public double byDef(int n)
        {
            tempL = new List<Shape>();
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                tempL.Add(new Triangle(r.Next(0,10000), r.Next(0, 10000)));
            }
            DateTime start = DateTime.Now;
            float k;
            float b;
            bool a2;
            bool a1;
            foreach (Shape item in tempL)
            {
                item.part = false;
            }
            for (int i = 0; i < tempL.Count - 1; i++)
            {
                for (int j = i + 1; j < tempL.Count; j++)
                {
                    if (tempL[j].x == tempL[i].x)
                    {
                        a1 = true;
                        a2 = true;
                        for (int l = 0; l < tempL.Count; l++)
                        {
                            if (l != i && l != j)
                            {
                                if (tempL[l].x > tempL[i].x)
                                {
                                    a1 = false;
                                }
                                else if (tempL[l].x < tempL[i].x)
                                {
                                    a2 = false;
                                }
                            }
                        }
                        if (a1 != a2)
                        {
                            tempL[i].part = true;
                            tempL[j].part = true;
                        }
                    }
                    else
                    {
                        a1 = true;
                        a2 = true;
                        k = (tempL[j].y - tempL[i].y + 0.0f) / (tempL[j].x - tempL[i].x);
                        b = tempL[i].y - (k * tempL[i].x);
                        for (int l = 0; l < tempL.Count; l++)
                        {
                            if (l != i && l != j)
                            {
                                if ((tempL[l].x * k) + b > tempL[l].y)
                                {
                                    a1 = false;
                                }
                                else if ((tempL[l].x * k) + b < tempL[l].y)
                                {
                                    a2 = false;
                                }
                            }
                        }
                        if (a1 != a2)
                        {
                            tempL[i].part = true;
                            tempL[j].part = true;
                        }
                    }
                }
            }
            tempL.Clear();
            DateTime end = DateTime.Now;
            TimeSpan ts = (end - start);
            return ts.TotalMilliseconds;
        }
        static public void byDef(PaintEventArgs e)
        {
            float k;
            float b;
            bool a2;
            bool a1;
            foreach (Shape item in Shape.allPoints)
            {
                item.part = false;
            }
            for (int i = 0; i < Shape.allPoints.Count - 1; i++)
            {
                for (int j = i + 1; j < Shape.allPoints.Count; j++)
                {
                    if (Shape.allPoints[j].x == Shape.allPoints[i].x)
                    {
                        a1 = true;
                        a2 = true;
                        for (int l = 0; l < Shape.allPoints.Count; l++)
                        {
                            if (l != i && l != j)
                            {
                                if (Shape.allPoints[l].x > Shape.allPoints[i].x)
                                {
                                    a1 = false;
                                }
                                else if (Shape.allPoints[l].x < Shape.allPoints[i].x)
                                {
                                    a2 = false;
                                }
                            }
                        }
                        if (a1 != a2)
                        {
                            Brush br = new SolidBrush(Color.Red);
                            Pen p = new Pen(br);
                            e.Graphics.DrawLine(p, new PointF(Shape.allPoints[i].x, Shape.allPoints[i].y), new PointF(Shape.allPoints[j].x, Shape.allPoints[j].y));
                            Shape.allPoints[i].part = true;
                            Shape.allPoints[j].part = true;
                        }
                    }
                    else
                    {
                        a1 = true;
                        a2 = true;
                        k = (Shape.allPoints[j].y - Shape.allPoints[i].y + 0.0f) / (Shape.allPoints[j].x - Shape.allPoints[i].x);
                        b = Shape.allPoints[i].y - (k * Shape.allPoints[i].x);
                        for (int l = 0; l < Shape.allPoints.Count; l++)
                        {
                            if (l != i && l != j)
                            {
                                if ((Shape.allPoints[l].x * k) + b > Shape.allPoints[l].y)
                                {
                                    a1 = false;
                                }
                                else if ((Shape.allPoints[l].x * k) + b < Shape.allPoints[l].y)
                                {
                                    a2 = false;
                                }
                            }
                        }
                        if (a1 != a2)
                        {
                            Brush br = new SolidBrush(Color.Red);
                            Pen p = new Pen(br);
                            e.Graphics.DrawLine(p, new PointF(Shape.allPoints[i].x, Shape.allPoints[i].y), new PointF(Shape.allPoints[j].x, Shape.allPoints[j].y));
                            Shape.allPoints[i].part = true;
                            Shape.allPoints[j].part = true;
                        }
                    }
                }
            }
        }
        static public double jarv(int n)
        {
            tempL = new List<Shape>();
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                tempL.Add(new Triangle(r.Next(0, 10000), r.Next(0, 10000)));
            }
            DateTime start = DateTime.Now;
            int firstPoint = 0;
            int thisPoint = 0;
            int prevPoint = 0;
            int index = 0;
            PointF tempPoint;
            float minY = -1;
            float minX = int.MaxValue;
            foreach (Shape item in tempL)
            {
                item.part = false;
            }
            for (int i = 0; i < tempL.Count; i++)
            {
                if (tempL[i].y > minY)
                {
                    minY = tempL[i].y;
                    minX = tempL[i].x;
                    thisPoint = i;
                }
                else if (tempL[i].y == minY)
                {
                    if (tempL[i].x < minX)
                    {
                        minY = tempL[i].y;
                        minX = tempL[i].x;
                        thisPoint = i;
                    }
                }
            }
            firstPoint = thisPoint;
            tempL[firstPoint].part = true;
            tempPoint = new PointF(tempL[thisPoint].x - 10, tempL[thisPoint].y);
            minX = -1;//теперь  это макс косинус
            for (int i = 0; i < tempL.Count; i++)
            {
                if (i != thisPoint)
                {
                    if (minX < Udobno.findUgol(tempPoint, thisPoint, i, tempL))
                    {
                        minX = Udobno.findUgol(tempPoint, thisPoint, i, tempL);
                        index = i;
                    }
                    if (minX == Udobno.findUgol(tempPoint, thisPoint, i, tempL))
                    {
                        if (Udobno.distance(thisPoint, i, tempL) > Udobno.distance(thisPoint, index, tempL))
                        {
                            minX = Udobno.findUgol(tempPoint, thisPoint, i, tempL);
                            index = i;
                        }
                    }
                }
            }
            prevPoint = thisPoint;
            thisPoint = index;
            tempL[thisPoint].part = true;
            for (int j = 0; j < tempL.Count; j++)
            {
                minX = -1;
                index = 0;
                for (int i = 0; i < tempL.Count; i++)
                {
                    if (i != thisPoint && i != prevPoint)
                    {
                        if (minX < Udobno.findUgol(prevPoint, thisPoint, i, tempL))
                        {
                            minX = Udobno.findUgol(prevPoint, thisPoint, i, tempL);
                            index = i;
                        }
                        if (minX == Udobno.findUgol(prevPoint, thisPoint, i, tempL))
                        {
                            if (Udobno.distance(thisPoint, i, tempL) > Udobno.distance(thisPoint, index, tempL))
                            {
                                minX = Udobno.findUgol(tempPoint, thisPoint, i, tempL);
                                index = i;
                            }
                        }
                    }
                }
                prevPoint = thisPoint;
                thisPoint = index;
                tempL[thisPoint].part = true;
                if (thisPoint == firstPoint)
                {
                    break;
                }
            }
            tempL.Clear();
            DateTime end = DateTime.Now;
            TimeSpan ts = (end - start);
            return ts.TotalMilliseconds;
        }
        static public void jarv(PaintEventArgs e)
        {
            int firstPoint = 0;
            int thisPoint = 0;
            int prevPoint = 0;
            int index = 0;
            PointF tempPoint;
            float minY = -1;
            float minX = int.MaxValue;
            foreach (Shape item in Shape.allPoints)
            {
                item.part = false;
            }
            for (int i = 0; i < Shape.allPoints.Count; i++)
            {
                if (Shape.allPoints[i].y > minY)
                {
                    minY = Shape.allPoints[i].y;
                    minX = Shape.allPoints[i].x;
                    thisPoint = i;
                }
                else if (Shape.allPoints[i].y == minY)
                {
                    if (Shape.allPoints[i].x < minX)
                    {
                        minY = Shape.allPoints[i].y;
                        minX = Shape.allPoints[i].x;
                        thisPoint = i;
                    }
                }
            }
            firstPoint = thisPoint;
            Shape.allPoints[firstPoint].part = true;
            tempPoint = new PointF(Shape.allPoints[thisPoint].x - 10, Shape.allPoints[thisPoint].y);
            minX = -1;//теперь  это макс косинус
            for (int i = 0; i < Shape.allPoints.Count; i++)
            {
                if (i != thisPoint)
                {
                    if (minX < Udobno.findUgol(tempPoint, thisPoint, i, Shape.allPoints))
                    {
                        minX = Udobno.findUgol(tempPoint, thisPoint, i, Shape.allPoints);
                        index = i;
                    }
                    if (minX == Udobno.findUgol(tempPoint, thisPoint, i, Shape.allPoints))
                    {
                        if (Udobno.distance(thisPoint, i, Shape.allPoints) > Udobno.distance(thisPoint, index, Shape.allPoints))
                        {
                            minX = Udobno.findUgol(tempPoint, thisPoint, i, Shape.allPoints);
                            index = i;
                        }
                    }
                }
            }
            prevPoint = thisPoint;
            thisPoint = index;
            Shape.allPoints[thisPoint].part = true;
            Brush b = new SolidBrush(Color.Green);
            Pen p = new Pen(b);
            e.Graphics.DrawLine(p, new PointF(Shape.allPoints[thisPoint].x, Shape.allPoints[thisPoint].y), new PointF(Shape.allPoints[prevPoint].x, Shape.allPoints[prevPoint].y));
            for (int j = 0; j < Shape.allPoints.Count; j++)
            {
                minX = -1;
                index = 0;
                for (int i = 0; i < Shape.allPoints.Count; i++)
                {
                    if (i != thisPoint && i != prevPoint)
                    {
                        if (minX < Udobno.findUgol(prevPoint, thisPoint, i, Shape.allPoints))
                        {
                            minX = Udobno.findUgol(prevPoint, thisPoint, i, Shape.allPoints);
                            index = i;
                        }
                        if (minX == Udobno.findUgol(prevPoint, thisPoint, i, Shape.allPoints))
                        {
                            if (Udobno.distance(thisPoint, i, Shape.allPoints) > Udobno.distance(thisPoint, index, Shape.allPoints))
                            {
                                minX = Udobno.findUgol(tempPoint, thisPoint, i, Shape.allPoints);
                                index = i;
                            }
                        }
                    }
                }
                prevPoint = thisPoint;
                thisPoint = index;
                Shape.allPoints[thisPoint].part = true;
                b = new SolidBrush(Color.Green);
                p = new Pen(b);
                e.Graphics.DrawLine(p, new PointF(Shape.allPoints[thisPoint].x, Shape.allPoints[thisPoint].y), new PointF(Shape.allPoints[prevPoint].x, Shape.allPoints[prevPoint].y));
                if (thisPoint == firstPoint)
                {
                    break;
                }
            }
        }
        public static void saveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "C:\\Users\\fireg\\OneDrive\\Рабочий стол\\работа\\школьные задания";
            saveFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.DefaultExt = ".bin";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                path = saveFileDialog.FileName;
                WriteToBinaryFile();
            }
            else
            {
                canceled = true;
            }
        }
        public static void openFile(Form3 f3)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\fireg\\OneDrive\\Рабочий стол\\работа\\школьные задания";
            openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.DefaultExt = ".bin";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                path = openFileDialog.FileName;
                ReadFromBinaryFile(f3);
            }
            else
            {
                canceled = true;
            }
        }
        public static void WriteToBinaryFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            bf.Serialize(fs, Shape.allPoints);
            bf.Serialize(fs, Shape.color);
            bf.Serialize(fs, Shape.r);
            bf.Serialize(fs, Shape.nowind);
            changed = false;
            fs.Close();
        }
        public static void ReadFromBinaryFile(Form3 f3)
        {
            if (f3!=null)
            {
                f3.Close();
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Shape.allPoints = (List<Shape>)bf.Deserialize(fs);
            Shape.color = (System.Drawing.Color)bf.Deserialize(fs);
            Shape.r = (int)bf.Deserialize(fs);
            Shape.nowind = (int)bf.Deserialize(fs);
            Action.history = new List<Action>();
            Action.nowi = -1;
            changed = false;
            fs.Close();
        }
        public static Shape DeepClone<Shape>(Shape obj)
        {
            MemoryStream ms = new MemoryStream();

            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (Shape)formatter.Deserialize(ms);
        }
    }
    public class RadiusEventArgs : EventArgs
    {
        public int rad;
        public RadiusEventArgs(int r)
        {
            rad = r;
        }
    }
}
