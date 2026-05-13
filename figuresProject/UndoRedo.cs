using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace figuresProject
{
    abstract class Action
    {
        public static List<Action> history;
        public static int nowi;
        public static Form1 form;
        public static Label label1;
        static Action()
        {
            nowi = -1;
            history = new List<Action>();
        }
        public static void addHistory(Action action)
        {
            if (nowi != history.Count-1)
            {
                label1.Text = nowi.ToString();
                cutLine(nowi+1);
            }
            nowi++;
            history.Add(action);
        }
        public abstract void undo();
        public abstract void redo();
        public static void undoGlobal()
        {
            if (nowi != -1)
            {
                history[nowi].undo();
                nowi--;
                form.Refresh();
            }
        }
        public static void redoGlobal()
        {
            if (nowi != history.Count-1)
            {
                history[nowi+1].redo();
                nowi++;
                form.Refresh();
            }
        }
        public static void cutLine(int ind1)
        {
            List <Action> temp= new List<Action>();
            for (int i = 0; i < ind1; i++)
            {
                temp.Add(history[i]);
            }
            history = temp;
            nowi = ind1 - 1;
        }
        public static void changeRadValue(int radnow)
        {
            for (int i = history.Count-1; i >= 0; i--)
            {
                if (history[i] is ChangeRad)
                {
                    label1.Text = radnow.ToString();
                    ((ChangeRad)history[i]).radNow = radnow;
                    break;
                }
            }
        }
    }
    class Create : Action
    {
        public Shape point;

        public Create(Shape point1)
        {
            point = point1;

        }
        public override void undo()
        {
            for (int i = 0; i < Shape.allPoints.Count; i++)
            {
                if (Shape.allPoints[i].index == point.index)
                {
                    Shape.allPoints.RemoveAt(i);
                }
            }
        }
        public override void redo()
        {
            Shape.allPoints.Add(point);
        }
    }
    class Delete : Action
    {
        public List<Shape> point = new List<Shape>();

        public Delete(Shape point1)
        {
            this.point.Add(point1);
        }
        public Delete(List<Shape> point1)
        {
            this.point = point1;
        }
        public override void redo()
        {
            foreach (Shape item in point)
            {
                for (int i = 0; i < Shape.allPoints.Count; i++)
                {
                    if (Shape.allPoints[i].index == item.index)
                    {
                        Shape.allPoints.RemoveAt(i);
                    }
                }
            }
        }
        public override void undo()
        {
            foreach (Shape item in point)
            {
                Shape.allPoints.Add(item);
            }
        }
    }
    class Move : Action
    {
        public List<Shape> pointsBefore;
        public List<Shape> pointsNow;
        public Move(List<Shape> points1, List<Shape> points2)
        {
            pointsBefore = points1;
            pointsNow = points2;
        }
        public override void undo()
        {
            foreach (Shape item in pointsBefore)
            {
                for (int i = 0; i < Shape.allPoints.Count; i++)
                {
                    if (Shape.allPoints[i].index == item.index)
                    {
                        label1.Text = item.x.ToString() + ";  "+ Shape.allPoints[i].x.ToString(); 
                        Shape.allPoints[i] = item;
                    }
                }
            }
        }
        public override void redo()
        {
            foreach (Shape item in pointsNow)
            {
                for (int i = 0; i < Shape.allPoints.Count; i++)
                {
                    if (Shape.allPoints[i].index == item.index)
                    {
                        Shape.allPoints[i] = item;
                    }
                }
            }
        }
    }
    class ChangeRad:Action
    {
        public int radBefore;
        public int radNow;
        public ChangeRad(int r1, int r2)
        {
            radBefore = r1;
            radNow = r2;
        }
        public override void undo()
        {
            Shape.r = radBefore;
        }
        public override void redo()
        {
            Shape.r = radNow;
        }
    }
    class ChangeColor : Action
    {
        public Color colorBefore;
        public Color colorNow;
        public ChangeColor(Color color1, Color color2)
        {
            colorBefore = color1;
            colorNow = color2;
        }
        public override void undo()
        {
            Shape.color = colorBefore;
        }
        public override void redo()
        {
            Shape.color = colorNow;
        }
    }
    class Combination:Action
    {
        public List<Action> mas = new List<Action>();
        public Combination(List<Action> act)
        {
            mas = act;
        }
        public override void undo()
        {
            foreach (Action item in mas)
            {
                item.undo();
            }
        }
        public override void redo()
        {
            foreach (Action item in mas)
            {
                item.redo();
            }
        }
    }
}
