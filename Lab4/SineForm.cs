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
    public partial class SineForm : Form
    {
        private Thread animationThread;
        private ManualResetEvent pauseEvent;
        private bool isRunning;  
        private float phase;
        private Button btnPause, btnResume;

        public SineForm()
        {
            pauseEvent = new ManualResetEvent(true);
            isRunning = true; 
            phase = 0;

            btnPause = new Button() { Text = "Pause", Location = new Point(10, 10) };
            btnResume = new Button() { Text = "Resume", Location = new Point(100, 10) };

            btnPause.Click += (sender, args) => pauseEvent.Reset();
            btnResume.Click += (sender, args) => pauseEvent.Set();

            Controls.Add(btnPause);
            Controls.Add(btnResume);

            animationThread = new Thread(AnimateSine);
            animationThread.IsBackground = true;
            animationThread.Start();
        }

        private void AnimateSine()
        {
            while (isRunning)
            {
                pauseEvent.WaitOne();

                phase += 0.1f;
                Invalidate();

                Thread.Sleep(60);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < ClientSize.Width; i++)
            {
                float y = (float)(Math.Sin((i + phase) * 0.1) * 50 + ClientSize.Height / 2);
                e.Graphics.DrawEllipse(Pens.Blue, i, y, 2, 2);
            }
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
