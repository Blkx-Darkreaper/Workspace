using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pressure_Puzzle_Maker
{
    public partial class Form1 : Form
    {
        protected int pixelWidth = 0;
        protected int pixelHeight = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int width = (int)numericUpDown1.Value;
            int height = (int)numericUpDown2.Value;

            // Resize Panel
            this.pixelWidth = width * Program.blankImage.Width;
            this.pixelHeight = height * Program.blankImage.Height;

            Size size = new Size(pixelWidth, pixelHeight);

            //pictureBox1.MaximumSize = size;
            pictureBox1.Size = size;

            Bitmap bitmap = new Bitmap(pixelWidth, pixelHeight);
            Program.GeneratePuzzle(ref bitmap, width, height);

            pictureBox1.Image = bitmap;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image == null)
            {
                return;
            }

            Point position = pictureBox1.PointToClient(Cursor.Position);
            Tile tile = Program.GetTileAtPosition(position);
            if (tile.isValid == false)
            {
                return;
            }

            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            if(mouseEvent == null)
            {
                return;
            }

            tile.ToggleTile(mouseEvent);
            //Program.ImageChanged();

            Bitmap bitmap = (Bitmap)pictureBox1.Image;
            if (bitmap == null)
            {
                bitmap = new Bitmap(pixelWidth, pixelHeight);
            }

            Program.RedrawPuzzle(ref bitmap);

            pictureBox1.Image = bitmap;

            //pictureBox1.Invalidate();
            //pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            return; //testing

            if(pixelWidth <= 0 || pixelHeight <= 0)
            {
                return;
            }

            Bitmap bitmap = (Bitmap)pictureBox1.Image;
            if(bitmap == null)
            {
                bitmap = new Bitmap(pixelWidth, pixelHeight);
            }

            Program.RedrawPuzzle(ref bitmap);

            pictureBox1.Image = bitmap;
        }
    }
}
