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
    public partial class StarForm : Form
    {
        private Thread animationThread;
        private ManualResetEvent pauseEvent;
        private bool isRunning;  
        private float angle;  
        private float scale;  
        private Button btnPause, btnResume;
        private bool increasing = true;

        public StarForm()
        {
            pauseEvent = new ManualResetEvent(true);
            isRunning = true;  
            angle = 0;
            scale = 1.0f;

            btnPause = new Button() { Text = "Pause", Location = new Point(10, 10) };
            btnResume = new Button() { Text = "Resume", Location = new Point(100, 10) };

            btnPause.Click += (sender, args) => pauseEvent.Reset();
            btnResume.Click += (sender, args) => pauseEvent.Set();

            Controls.Add(btnPause);
            Controls.Add(btnResume);

            animationThread = new Thread(AnimateStar);
            animationThread.IsBackground = true;
            animationThread.Start();
        }

        private void AnimateStar()
        {
            while (isRunning)
            {
                pauseEvent.WaitOne(); 

                angle += 0.05f;

                if (increasing)
                    scale += 0.01f;
                else
                    scale -= 0.01f;

                if (scale > 1.5f) increasing = false;
                if (scale < 0.8f) increasing = true;

                Invalidate(); 

                Thread.Sleep(60); 
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            PointF center = new PointF(ClientSize.Width / 2, ClientSize.Height / 2);

            DrawStar(e.Graphics, center, 5, 50 * scale, 25 * scale, angle);
        }

        private void DrawStar(Graphics g, PointF center, int points, float outerRadius, float innerRadius, float angle)
        {
            PointF[] starPoints = new PointF[points * 2];
            double angleStep = Math.PI / points;

            for (int i = 0; i < points * 2; i++)
            {
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                float x = center.X + (float)(radius * Math.Cos(i * angleStep + angle));
                float y = center.Y + (float)(radius * Math.Sin(i * angleStep + angle));
                starPoints[i] = new PointF(x, y);
            }

            g.FillPolygon(Brushes.Yellow, starPoints); 
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
