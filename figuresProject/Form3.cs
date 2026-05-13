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
    public partial class Form3 : Form
    {
        public int r;
        public event RadiusChanged RadiusChange;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
        }
        public void setStartr(int rad)
        {
            r = rad;
            trackBar1.Value = r;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.RadiusChange(this, new RadiusEventArgs(trackBar1.Value));
        }
    }
}
