using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Pressure_Puzzle_Maker
{
    public partial class Form1 : Form
    {
        protected int pixelWidth = 0;
        protected int pixelHeight = 0;
        protected const int defaultWidth = 10;
        protected const int defaultHeight = 6;

        public Form1()
        {
            InitializeComponent();

            BuildPuzzle();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            widthNumeric.Value = defaultWidth;
            heightNumeric.Value = defaultHeight;

            BuildPuzzle();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "New Puzzle.png";
            dialog.DefaultExt = "png";
            dialog.FilterIndex = 4;
            dialog.ValidateNames = true;
            dialog.Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpg)|*.jpg|Png Image (.png)|*.png" +
                "|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";

            ImageFormat format = ImageFormat.Png;
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string ext = System.IO.Path.GetExtension(dialog.FileName).ToLower();
            switch (ext)
            {
                case ".bmp":
                    format = ImageFormat.Bmp;
                    break;

                case ".gif":
                    format = ImageFormat.Gif;
                    break;

                case ".jpg":
                    format = ImageFormat.Jpeg;
                    break;

                case ".tiff":
                    format = ImageFormat.Tiff;
                    break;

                case ".wmf":
                    format = ImageFormat.Wmf;
                    break;
            }

            puzzleImage.Image.Save(dialog.FileName, format);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            BuildPuzzle();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (puzzleImage.Image == null)
            {
                return;
            }

            Point position = puzzleImage.PointToClient(Cursor.Position);
            Tile tile = Program.GetTileAtPosition(position);
            if (tile.isValid == false)
            {
                return;
            }

            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            if (mouseEvent == null)
            {
                return;
            }

            tile.ToggleTile(mouseEvent);
            //Program.ImageChanged();

            Bitmap bitmap = (Bitmap)puzzleImage.Image;
            if (bitmap == null)
            {
                bitmap = new Bitmap(pixelWidth, pixelHeight);
            }

            Program.RedrawPuzzle(ref bitmap);

            puzzleImage.Image = bitmap;

            //pictureBox1.Invalidate();
            //pictureBox1.Refresh();
        }

        private void BuildPuzzle()
        {
            Program.SetPerfectPathLength((int)perfectNumeric.Value);

            SetDisplay();
        }

        private void SetDisplay()
        {
            int width = (int)widthNumeric.Value;
            int height = (int)heightNumeric.Value;

            // Resize Panel
            this.pixelWidth = width * Program.blankImage.Width;
            this.pixelHeight = height * Program.blankImage.Height;

            Size size = new Size(pixelWidth, pixelHeight);

            //pictureBox1.MaximumSize = size;
            puzzleImage.Size = size;

            Bitmap bitmap = new Bitmap(pixelWidth, pixelHeight);
            Program.GeneratePuzzle(ref bitmap, width, height);

            puzzleImage.Image = bitmap; // Update display
        }

        private void perfectOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Program.perfectPathOnly = perfectOnlyCheckBox.Checked;

            Size size = puzzleImage.Size;
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            Program.RedrawPuzzle(ref bitmap);

            puzzleImage.Image = bitmap; // Update display
        }
    }
}