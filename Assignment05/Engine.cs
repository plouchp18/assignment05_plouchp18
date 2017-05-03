using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Assignment05
{
    public partial class Engine : Form
    {
        public static Elephant elephant = new Elephant(100, 100);
        public static Sprite canvas = new Sprite();
        public static int enemyCount = 0;
        public static Enemy jason = new Enemy(1000, 200);
        public static Rectangle rect = new Rectangle(0, 0, 1400, 900, 200);
        public static bool win=false;
        public static bool lose=false;
        public static TextSprite text = new TextSprite(0, 0, "You Win!\nPress R to restart.");
        public static TextSprite loss = new TextSprite(0, 0, "You Lose.\nPress R to restart.");
        public static TextSprite intro = new TextSprite(0, 0, "Use the arrow keys to move.\nUse A and D to shoot.\nPress H to close the help menu.\nPress M to start the music.\nPress N to mute the music.\nPress R to restart.");

        public Sprite Canvas
        {
            set { canvas = value; }
            get { return canvas; }
        }

        //public static Sprite parent = new Sprite();
        public static Engine form;
        public Thread rthread;
        public Thread uthread;
        public static int fps = 30;
        public static double running_fps = 30.0;
        public static bool rendering = false;
        public static bool updating = false;
        public static bool resetting = false;
        public static SoundPlayer jukebox = new SoundPlayer(Properties.Resources.music);
        //public static SoundPlayer phwoah = new SoundPlayer(Properties.Resources.phwoah);

        public Engine()
        {
            DoubleBuffered = true;
            InitializeComponent();
            //canvas = new Sprite();
            form = this;
            Size = new Size(1300, 800);
            StartPosition = FormStartPosition.CenterScreen;
            rthread = new Thread(new ThreadStart(render));
            uthread = new Thread(new ThreadStart(update));
            rthread.Start();
            uthread.Start();
            canvas.add(rect);
            canvas.add(text);
            canvas.add(loss);
            canvas.add(intro);
            intro.setVisibility(true);
            //parent.add(Program.elephant);
        }

        public static void reset()
        {
            canvas.RemoveAll();
            resetting = true;
            while (rendering || updating) { }
            elephant = new Elephant(100, 100);
            elephant.alive = true;
            canvas.csAdd(elephant);
            for (int i = 0; i < 13; i++)
            {
                Box box = new Box(i * 100, 0);
                canvas.csAdd(box);
                box = new Box(i * 100, 600);
                canvas.csAdd(box);
            }
            for (int i = 0; i < 7; i++)
            {
                Box box = new Box(0, i * 100);
                canvas.csAdd(box);
                box = new Box(1200, i * 100);
                canvas.csAdd(box);
            }
            jason = new Enemy(1000, 200);
            enemyCount = 1;
            canvas.csAdd(jason);
            canvas.add(rect);
            canvas.add(text);
            canvas.add(loss);
            canvas.add(intro);
            intro.setVisibility(true);
            resetting = false;
        }

        public static void render()
        {
            DateTime last = DateTime.Now;
            DateTime now = last;
            TimeSpan frameTime = new TimeSpan(10000000 / fps);
            while (true)
            {
                DateTime temp = DateTime.Now;
                running_fps = .9 * running_fps + .1 * 1000.0 / (temp - now).TotalMilliseconds;
                now = temp;
                TimeSpan diff = now - last;
                if (diff.TotalMilliseconds < frameTime.TotalMilliseconds)
                    Thread.Sleep((frameTime - diff).Milliseconds);
                last = DateTime.Now;
                //form.Refresh();
                if (resetting) continue;
                rendering = true;
                if (enemyCount <= 0 && !lose)
                {
                    rect.setColor(Rectangle.initColor);
                    rect.setVisibility(true);
                    text.setVisibility(true);
                    text.changeLocation((form.ClientSize.Width / 2) - 50, (form.ClientSize.Height / 2) - 50);
                    win = true;
                }
                else if (!elephant.alive && !win)
                {
                    rect.setColor(Color.FromArgb(200, Color.Red));
                    rect.setVisibility(true);
                    loss.changeLocation((form.ClientSize.Width / 2) - 50, (form.ClientSize.Height / 2) - 50);
                    loss.setVisibility(true);
                    lose = true;
                }
                else
                {
                    rect.setVisibility(false);
                    text.setVisibility(false);
                    loss.setVisibility(false);
                    win = false;
                    lose = false;
                }
                form.Invoke(new MethodInvoker(form.Refresh));
                rendering = false;
            }
        }

        public static void update()
        {
            DateTime last = DateTime.Now;
            DateTime now = last;
            TimeSpan frameTime = new TimeSpan(10000000 / fps);
            while (true)
            {
                DateTime temp = DateTime.Now;
                now = temp;
                TimeSpan diff = now - last;
                if (diff.TotalMilliseconds < frameTime.TotalMilliseconds)
                    Thread.Sleep((frameTime - diff).Milliseconds);
                last = DateTime.Now;
                if (resetting) continue;
                updating = true;
                canvas.update();
                updating = false;
                if (!updating && !rendering)
                {
                    canvas.queueClear();
                    canvas.updateAllTracking();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            /*base.OnPaint(e);
            e.Graphics.FillRectangle(Brushes.Black, ClientRectangle);
            parent.render(e.Graphics);
            e.Graphics.DrawString("Collisions: " + elephant.getCollisions().Count, new Font("Comic Sans MS", 10), Brushes.LawnGreen, 0, 0);
            e.Graphics.DrawString("Count: " + elephant.TrackedSprites.Count, new Font("Comic Sans MS", 10), Brushes.LawnGreen, 0, 20);
            e.Graphics.DrawString("Enemies Remaining: " + enemyCount, new Font("Comic Sans MS", 10), Brushes.LawnGreen, 0, 0);*/
            canvas.render(e.Graphics);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Down)
            {
                elephant.Vy = 5;
                elephant.Ay = -0.1f;
            }
            else if (e.KeyCode == Keys.Up)
            {
                elephant.Vy = -5;
                elephant.Ay = 0.1f;
            }
            else if (e.KeyCode == Keys.Left)
            {
                elephant.Vx = -5;
                elephant.Ax = 0.1f;
            }
            else if (e.KeyCode == Keys.Right)
            {
                elephant.Vx = 5;
                elephant.Ax = -0.1f;
            }
            /*else if (e.KeyCode == Keys.K)
            {
                jason.Kill();
            }
            else if(e.KeyCode == Keys.W)
            {
                elephant.shoot(0);
            }*/
            else if(e.KeyCode == Keys.D)
            {
                elephant.shoot(1);
            }
            /*else if(e.KeyCode == Keys.S)
            {
                elephant.shoot(2);
            }*/
            else if(e.KeyCode == Keys.A)
            {
                elephant.shoot(3);
            }
            else if(e.KeyCode == Keys.R)
            {
                reset();
            }
            else if(e.KeyCode == Keys.M)
            {
                jukebox.PlayLooping();
            }
            else if(e.KeyCode == Keys.N)
            {
                jukebox.Stop();
            }
            else if(e.KeyCode == Keys.H)
            {
                intro.setVisibility(false);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            uthread.Abort();
            rthread.Abort();
        }

    }

}
