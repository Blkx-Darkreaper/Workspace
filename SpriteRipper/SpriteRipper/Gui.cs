using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

namespace SpriteRipper
{
    public partial class Gui : Form
    {
        protected Bitmap image { get; set; }
        protected const string INIT_TOTAL = "Initial total";
        protected const string FINAL_TOTAL = "Final total";
        protected const string DUPLICATES = "Duplicates";
        protected const string SORT_TIME = "Sort time (ms)";
        protected const string TIME_PER_TILE = "Time/Tile (ms)";

        public Gui()
        {
            InitializeComponent();
            Status.Text = String.Empty;
        }

        protected void BackgroundProgress_DoWork(object sender, DoWorkEventArgs e)
        {
            while (Progress.Value < Progress.Maximum)
            {
                int progress = Program.tasksComplete;
                BackgroundProgress.ReportProgress(progress);

                //Thread.Sleep(100);
            }
        }

        protected void BackgroundProgress_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Value = e.ProgressPercentage;
        }

        protected void BackgroundProgress_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Progress.Value = Progress.Maximum;
            Progress.Enabled = false;
        }

        protected void TileSize_ValueChanged(object sender, EventArgs e)
        {
            EnableUpdating();
            EnableSorting();
            SetOffsetMax();
        }

        protected void Bits_ValueChanged(object sender, EventArgs e)
        {
            EnableSorting();
        }

        protected void Zoom_ValueChanged(object sender, EventArgs e)
        {
            DrawDisplay();
            DrawTileset();
        }

        protected void PatternThreshold_ValueChanged(object sender, EventArgs e)
        {
            EnableSorting();
        }

        protected void ColourThreshold_ValueChanged(object sender, EventArgs e)
        {
            EnableSorting();
        }

        protected void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the filename of the selected file
            OpenFileDialog dialog = Program.GetFilenameToOpen();
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            string filename = dialog.FileName;

            if (filename == null)
            {
                MessageBox.Show("Invalid filename", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (filename.Equals(string.Empty))
            {
                MessageBox.Show("Invalid filename", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Program.ResetAll();

            image = Program.LoadImage(filename);

            DisableSaving();
            EnableUpdating();

            Status.Text = String.Format("Loaded {0}", filename);
        }

        protected void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TilesetDisplay.Image == null)
            {
                return;
            }
            if (Program.tilesetReady == false)
            {
                return;
            }

            // Get the filename of the selected file
            SaveFileDialog dialog = Program.GetFilenameToSave();
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            string filename = dialog.FileName;

            if (filename == null)
            {
                MessageBox.Show("Invalid filename", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (filename.Equals(string.Empty))
            {
                MessageBox.Show("Invalid filename", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ImageFormat format;
            try
            {
                format = Program.GetImageFormat(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid filetype.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Image tileset = GetTileset();
            tileset.Save(filename, format);

            Status.Text = String.Format("Saved {0}", filename);
        }

        protected void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            int bitsPerColour = (int)Bits.Value;
            int tileSize = (int)TileSize.Value;

            int offsetX = (int)OffsetX.Value;
            int offsetY = (int)OffsetY.Value;

            Status.Text = String.Format("Updating image tiles");

            int tasks = Program.GetTileCount(image.Width, image.Height, tileSize);
            StartProgressBar(tasks);
            Program.LoadAllTiles(image, bitsPerColour, tileSize, offsetX, offsetY);
            //Program.LoadTilesThreaded(image, bitsPerColour, tileSize, offsetX, offsetY);

            DrawDisplay();
            Update.Enabled = false;

            int totalTiles = Program.GetTileCount();
            DisplayResult(InitTotal, totalTiles);

            DrawTileset();

            if (Program.tilesetReady == true)
            {
                return;
            }

            EnableSorting();
        }

        protected void Sort_Click(object sender, EventArgs e)
        {
            int bitsPerColour = (int)Bits.Value;
            int tileSize = (int)TileSize.Value;
            float patternThreshold = (float)PatternThreshold.Value;
            float colourThreshold = (float)ColourThreshold.Value;

            Status.Text = String.Format("Sorting image tiles");

            Program.ResetTileset();

            int tasks = Program.GetTileCount(image.Width, image.Height, tileSize);
            StartProgressBar(tasks);
            Program.SortTiles(patternThreshold, colourThreshold);

            //Bitmap tileset = Program.ProcessImage(image, bitsPerColour, tileSize, patternThreshold, colourThreshold);

            DrawTileset();

            EnableSaving();
            Sort.Enabled = false;

            int totalAfterSorting = Program.GetSortedTileCount();
            DisplayResult(FinalTotal, totalAfterSorting);

            int duplicates = Program.duplicates;
            DisplayResult(Duplicates, duplicates);

            long sortTime = Program.sortTime;
            DisplayResult(SortTime, sortTime);

            int totalTiles = Program.GetTileCount();
            double timePerTile = Math.Round((double)sortTime / (double)totalTiles, 2);

            DisplayResult(TimePerTile, timePerTile);
        }

        protected void DisplayResult(Label label, double result)
        {
            string resultName = string.Empty;
            string resultValue = String.Format("{0}", result);

            string name = label.Name;
            switch (name) {
                case "InitTotal":
                    resultName = INIT_TOTAL;
                    break;

                case "FinalTotal":
                    resultName = FINAL_TOTAL;
                    break;

                case "Duplicates":
                    resultName = DUPLICATES;
                    break;

                case "SortTime":
                    resultName = SORT_TIME;
                    resultValue = result.ToString("#,##0");
                    break;

                case "TimePerTile":
                    resultName = TIME_PER_TILE;
                    resultValue = String.Format("{0:N2}", result);
                    break;

                default:
                    return;
            }

            label.Text = String.Format("{0}: {1}", resultName, resultValue);
        }

        protected void DrawDisplay()
        {
            if (Program.displayReady == false)
            {
                return;
            }

            int tileSize = (int)TileSize.Value;

            int offsetX = (int)OffsetX.Value;
            int offsetY = (int)OffsetY.Value;

            int zoom = (int)Zoom.Value;

            int width = image.Width - tileSize * (int)Math.Ceiling((double)offsetX / tileSize);
            int height = image.Height - tileSize * (int)Math.Ceiling((double)offsetY / tileSize);

            int tilesWide = width / tileSize;
            int tilesHigh = height / tileSize;

            width *= zoom;
            height *= zoom;

            // increase size to account for spaces between tiles
            width += tilesWide - 1;
            height += tilesHigh - 1;

            Bitmap displayImage = new Bitmap(width, height);
            Program.DrawAllTilesOntoImage(displayImage, tilesWide, tileSize, zoom);

            ImageDisplay.Image = displayImage;

            Status.Text = String.Format("Displayed image tiles");
        }

        private void DrawTileset()
        {
            if (Program.tilesetReady == false)
            {
                return;
            }

            //TilesetDisplay.Image = null;
            int zoom = (int)Zoom.Value;

            Bitmap tileset = GetTileset(zoom);
            TilesetDisplay.Image = tileset;

            Status.Text = String.Format("Displayed tileset");
        }

        protected Bitmap GetTileset()
        {
            return GetTileset(1);
        }

        protected Bitmap GetTileset(int zoom)
        {
            if (Program.tilesetReady == false)
            {
                return null;
            }

            int tileSize = (int)TileSize.Value;
            PixelFormat format = image.PixelFormat;

            Bitmap tileset;
            if (Grouped.Checked == true)
            {
                tileset = Program.GetGroupedTileset(format, tileSize, zoom);
            }
            else
            {
                int tilesWide = (int)TilesWide.Value;
                tileset = Program.GetTileset(format, tileSize, tilesWide, zoom);
            }

            return tileset;
        }

        public void StartProgressBar(int tasks)
        {
            Progress.Enabled = true;
            Progress.Value = 0;
            //Progress.Step = 1;
            Progress.Minimum = 0;
            Progress.Maximum = tasks;

            BackgroundProgress.WorkerReportsProgress = true;
            while (BackgroundProgress.IsBusy == true)
            {
                Thread.Sleep(100);
            }

            BackgroundProgress.RunWorkerAsync();
        }

        protected void EnableSaving()
        {
            SaveAsMenuItem.Enabled = true;
            SaveMenuItem.Enabled = true;
        }

        protected void DisableSaving()
        {
            SaveAsMenuItem.Enabled = false;
            SaveMenuItem.Enabled = false;
        }

        protected void EnableUpdating()
        {
            if (image == null)
            {
                return;
            }

            Update.Enabled = true;
            Progress.Value = 0;
        }

        protected void EnableSorting()
        {
            if (image == null)
            {
                return;
            }
            if (Program.displayReady == false)
            {
                return;
            }

            Sort.Enabled = true;
            Progress.Value = 0;
        }

        protected void Preset8_CheckedChanged(object sender, EventArgs e)
        {
            TileSize.Value = 8;
            SetOffsetMax();
        }

        protected void Preset16_CheckedChanged(object sender, EventArgs e)
        {
            TileSize.Value = 16;
            SetOffsetMax();
        }

        protected void Preset32_CheckedChanged(object sender, EventArgs e)
        {
            TileSize.Value = 32;
            SetOffsetMax();
        }

        protected void Preset64_CheckedChanged(object sender, EventArgs e)
        {
            TileSize.Value = 64;
            SetOffsetMax();
        }

        protected void Preset128_CheckedChanged(object sender, EventArgs e)
        {
            TileSize.Value = 128;
            SetOffsetMax();
        }

        protected void Preset256_CheckedChanged(object sender, EventArgs e)
        {
            TileSize.Value = 256;
            SetOffsetMax();
        }

        protected void SetOffsetMax()
        {
            int tileSize = (int)TileSize.Value;
            int maxOffset = tileSize - 1;

            OffsetX.Maximum = maxOffset;
            OffsetY.Maximum = maxOffset;

            //TilesWide.Minimum = 5 * tileSize;
            TilesetDisplay.Image = null;
            Program.ResetTileset();
        }

        protected void OffsetX_ValueChanged(object sender, EventArgs e)
        {
            EnableUpdating();
            //EnableSorting();
        }

        protected void OffsetY_ValueChanged(object sender, EventArgs e)
        {
            EnableUpdating();
            //EnableSorting();
        }

        protected void Grouped_CheckedChanged(object sender, EventArgs e)
        {
            TilesWide.Enabled = false;
            EnableUpdating();
        }

        protected void Compressed_CheckedChanged(object sender, EventArgs e)
        {
            TilesWide.Enabled = true;
            EnableUpdating();
        }

        private void BackgroundColour_Click(object sender, EventArgs e)
        {
            DialogResult result = ColourPicker.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            BackgroundColour.BackColor = ColourPicker.Color;
            Program.backgroundColour = ColourPicker.Color;

            EnableUpdating();
        }

        private void TilesWide_ValueChanged(object sender, EventArgs e)
        {
            EnableUpdating();
        }
    }
}
