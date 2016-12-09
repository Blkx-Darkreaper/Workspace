using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NachosCrazyTaxiService
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int width = 100;
            int height = 100;
            Program.GenerateWorld(width, height);

            Bitmap image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Program.DrawWorld(image, Color.Red, 2);
            MainDisplay.Image = image;
        }
    }
}
