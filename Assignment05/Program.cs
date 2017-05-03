using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment05
{
    class Program : Engine
    {
        public Program() : base()
        {

        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*List<int> xs = new List<int> { 000, 100, 200, 300, 400, 500, 000, 100, 200, 300, 400, 500, 000, 000, 000, 000, 500, 500, 500, 500 };
            List<int> ys = new List<int> { 500, 500, 500, 500, 500, 500, 000, 000, 000, 000, 000, 000, 100, 200, 300, 400, 100, 200, 300, 400 };
            for(int i = 0; i < xs.Count; i++) {
                Box box = new Box(xs[i], ys[i]);
                Program.canvas.csAdd(box);
            }*/
            Program.canvas.csAdd(elephant);
            for (int i = 0; i < 13; i++)
            {
                Box box = new Box(i * 100, 0);
                canvas.csAdd(box);
                box = new Box(i * 100, 600);
                canvas.csAdd(box);
            }
            for(int i = 0; i < 7; i++)
            {
                Box box = new Box(0, i * 100);
                canvas.csAdd(box);
                box = new Box(1200, i * 100);
                canvas.csAdd(box);
            }
            Program.canvas.csAdd(jason);
            Application.Run(new Program());
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program program = new Program();
            program.Canvas.add(new Elephant(0, 0));
            Application.Run(new Program());*/
        }
    }
}
