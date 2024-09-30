using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4_2
{
    public partial class MainForm : Form
    {
        Button btnBall, btnRectangle, btnSine, btnStar;

        public MainForm()
        {
            btnBall = new Button() { Text = "Ball Animation", Location = new Point(10, 10) };
            btnRectangle = new Button() { Text = "Rectangle Animation", Location = new Point(10, 50) };
            btnSine = new Button() { Text = "Sine Animation", Location = new Point(10, 90) };
            btnStar = new Button() { Text = "Star Animation", Location = new Point(10, 130) };

            btnBall.Click += (sender, args) => { new BallForm().Show(); };
            btnRectangle.Click += (sender, args) => { new RectangleForm().Show(); };
            btnSine.Click += (sender, args) => { new SineForm().Show(); };
            btnStar.Click += (sender, args) => { new StarForm().Show(); };          

            Controls.Add(btnBall);
            Controls.Add(btnRectangle);
            Controls.Add(btnSine);
            Controls.Add(btnStar);
        }
    }
}
