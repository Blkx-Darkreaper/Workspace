using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace NachosCrazyTaxiService
{
    static class Program
    {
        private static List<Entity> allEntities { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void GenerateWorld(int width, int height)
        {
            int seed = 0;
            GenerateWorld(width, height, seed);
        }

        public static void GenerateWorld(int width, int height, int seed)
        {
            allEntities = new List<Entity>();

            Entity box = new Entity(50, 50, 30, 30, Color.Black, 1);
            allEntities.Add(box);

            for (int i = 0; i < 2; i++)
            {
                box.TakeDamage(5, i);   //debug
            }
        }

        public static void DrawWorld(Bitmap image, Color lineColour, int lineThickness)
        {
            if (image == null)
            {
                return;
            }

            using (Graphics graphics = Graphics.FromImage(image))
            {
                // Fill image with white
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                graphics.FillRectangle(Brushes.White, rect);

                // Draw all grids
                foreach (Entity entity in allEntities)
                {
                    entity.Draw(graphics);
                }
            }
        }
    }
}
