using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace Breakout
{
    public partial class Main : Form
    {
        private Matrix m; // Matrix of transformations
        private float s1; // scale
        private int leftpaddlex = -10;
        private int ballx = -3;
        private int bally = -3;
        private int xinc = 1;
        private int yinc = 1;
        private Point prev, next; 

        public Main()
        {
            InitializeComponent();

            //mouse points 
            prev = new Point();
            next = new Point();
            //setup coordinate transformation 
            float width = this.ClientRectangle.Width;
            float height = this.ClientRectangle.Height;
            float x1d = width * 0.1f;
            float x2d = height * 0.9f;
            float x3d = width * 0.9f;
            float x4d = height * 0.1f;
            float x1 = -80, x2 = -50, x3 = 80, x4 = 50;
            s1 = (x1d - x3d) / (x1 - x3);
            float s2 = (x2d - x4d) / (x2 - x4);
            float t1 = (x1 * x3d - x1d * x3) / (x1 - x3);
            float t2 = (x2 * x4d - x2d * x4) / (x2 - x4);
            m = new Matrix();
            m.Translate(t1, t2);
            m.Scale(s1, s2); 

        }

        private void Main_Load(object sender, EventArgs e)
        {
            string s;
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            //do we need to calculate this every time? 

            Graphics g = e.Graphics;
            g.Transform = m;
            using (Pen p = new Pen(Color.Black, 10 / s1))
            {
                //draw outline 
                g.DrawRectangle(p, -80, -50, 160, 100);
                //draw paddle 
                g.DrawRectangle(p, leftpaddlex, -48, 20, 5);
                //draw the ball 
                g.FillEllipse(Brushes.Blue, ballx, bally, 6, 6);

            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (leftpaddlex > -80)
                        leftpaddlex -= 1;
                    break;
                case Keys.Right:
                    if (leftpaddlex < 60)
                        leftpaddlex += 1;
                    break;
            } 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((ballx < -80) || (ballx > 74))
                xinc *= -1;
            if ((bally < -50) || (bally > 44))
                yinc *= -1;
            ballx += xinc;
            bally += yinc;
            this.Refresh(); 

        }

        private void Main_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            this.Capture = true;
            prev.X = e.X;
            prev.Y = e.Y;
           
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Capture == true)
            {
                next.X = e.X;
                next.Y = e.Y;
                using (Graphics g = CreateGraphics())
                {
                    g.DrawLine(Pens.Black, prev, next);
                }
                prev.X = next.X;
                prev.Y = next.Y;
            } 

        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            this.Capture = false; 

        }

       


    }
}
