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
    public partial class BallForm : Form
    {
        private Thread animationThread;
        private ManualResetEvent pauseEvent;
        private bool isRunning; 
        private int x, y, dx, dy;
        private Button btnPause, btnResume;

        public BallForm()
        {
            pauseEvent = new ManualResetEvent(true); 
            isRunning = true; 
            x = 50; y = 50; dx = 5; dy = 5;

            btnPause = new Button() { Text = "Pause", Location = new Point(10, 10) };
            btnResume = new Button() { Text = "Resume", Location = new Point(100, 10) };

            btnPause.Click += (sender, args) => pauseEvent.Reset();
            btnResume.Click += (sender, args) => pauseEvent.Set();   

            Controls.Add(btnPause);
            Controls.Add(btnResume);

            animationThread = new Thread(AnimateBall);
            animationThread.IsBackground = true;
            animationThread.Start();
        }

        private void AnimateBall()
        {
            while (isRunning)  
            {
                pauseEvent.WaitOne(); 

                x += dx; y += dy;
                if (x < 0 || x > ClientSize.Width - 20) dx = -dx;
                if (y < 0 || y > ClientSize.Height - 20) dy = -dy;

                Invalidate();

                Thread.Sleep(60); 
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isRunning = false;  
            pauseEvent.Set();  
            animationThread.Join();  
            base.OnFormClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.Blue, x, y, 20, 20);
        }
    }
}
