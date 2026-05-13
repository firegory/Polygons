using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace figuresProject
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public void f2Show(Point[] jarv, Point[] byDef, int state)//1-только джарвиз, 2 - только поОпр, 3 - оба
        {
            if (state == 1)
            {
                for (int i = 0; i < jarv.Length; i++)
                {
                    chart1.Series[0].Points.AddXY(jarv[i].X, jarv[i].Y);
                }
            }
            if (state == 2)
            {
                for (int i = 0; i < byDef.Length; i++)
                {
                    chart1.Series[1].Points.AddXY(byDef[i].X, byDef[i].Y);
                }
            }
            if (state == 3)
            {
                for (int i = 0; i < byDef.Length; i++)
                {
                    chart1.Series[1].Points.AddXY(byDef[i].X, byDef[i].Y);
                }
                for (int i = 0; i < jarv.Length; i++)
                {
                    chart1.Series[0].Points.AddXY(jarv[i].X, jarv[i].Y);
                }
            }
            Refresh();
        }
    }
}
