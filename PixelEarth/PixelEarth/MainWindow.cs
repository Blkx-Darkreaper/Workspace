using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelEarth
{
    public partial class MainWindow : Form
    {
        public static Timer timer { get; set; }
        public static int updateSpeed { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            updateSpeed = 1;

            Display.Paint += Display_Paint;

            GenerateWorld();
            Start();
        }

        protected void Display_Paint(object sender, PaintEventArgs e)
        {
            Size screenSize = Display.Size;

            Program.Draw(e.Graphics, screenSize);
        }

        protected void GenerateWorld()
        {
            int width = Display.Width;
            int height = Display.Height;

            Program.GenerateWorld(width, height);
        }

        protected void Start()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(Update);
            timer.Interval = 100;
            //timer.Enabled = true;
            timer.Start();
        }

        protected void Update(object sender, EventArgs e)
        {
            int updateInterval = timer.Interval;
            float hoursElapsed = updateInterval / 1000f * updateSpeed;

            Program.Update(hoursElapsed);
            Clock.Text = Program.GetDateTime().ToString() + " GMT";
            Display.Refresh();
            //Display.Invalidate();
        }

        private void Speed1_CheckedChanged(object sender, EventArgs e)
        {
            updateSpeed = 1;
        }

        private void Speed2_CheckedChanged(object sender, EventArgs e)
        {
            updateSpeed = 2;
        }

        private void Speed5_CheckedChanged(object sender, EventArgs e)
        {
            updateSpeed = 5;
        }

        private void Speed50_CheckedChanged(object sender, EventArgs e)
        {
            updateSpeed = 50;
        }

        private void Speed250_CheckedChanged(object sender, EventArgs e)
        {
            updateSpeed = 250;
        }

        private void Speed1k_CheckedChanged(object sender, EventArgs e)
        {
            updateSpeed = 1000;
        }

        private void Speed10k_CheckedChanged(object sender, EventArgs e)
        {
            updateSpeed = 10000;
        }

        private void Daylight_CheckedChanged(object sender, EventArgs e)
        {
            Program.view = Program.View.Daylight;
        }

        private void Temp_CheckedChanged(object sender, EventArgs e)
        {
            Program.view = Program.View.Temperature;
        }
    }
}
