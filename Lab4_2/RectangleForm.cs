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
    public partial class RectangleForm : Form
    {
        private Thread animationThread;
        private ManualResetEvent pauseEvent;
        private bool isRunning;  
        private int width, height, dw;
        private Button btnPause, btnResume;

        public RectangleForm()
        {
            pauseEvent = new ManualResetEvent(true);
            isRunning = true;  
            width = 50; height = 50; dw = 5;

            btnPause = new Button() { Text = "Pause", Location = new Point(10, 10) };
            btnResume = new Button() { Text = "Resume", Location = new Point(100, 10) };

            btnPause.Click += (sender, args) => pauseEvent.Reset();
            btnResume.Click += (sender, args) => pauseEvent.Set();

            Controls.Add(btnPause);
            Controls.Add(btnResume);

            animationThread = new Thread(AnimateRectangle);
            animationThread.IsBackground = true;
            animationThread.Start();
        }

        private void AnimateRectangle()
        {
            while (isRunning)
            {
                pauseEvent.WaitOne();

                width += dw;
                if (width > 200 || width < 50) dw = -dw;

                Invalidate();

                Thread.Sleep(60);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Red, 50, 50, width, height);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isRunning = false;
            pauseEvent.Set();
            animationThread.Join();
            base.OnFormClosing(e);
        }
    }
}

