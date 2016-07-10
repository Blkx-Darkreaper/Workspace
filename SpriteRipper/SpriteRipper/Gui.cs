﻿using System;
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
        protected static string loadedFilename { get; set; }
        //protected Bitmap loadedImage { get; set; }
        protected const string INIT_TOTAL = "Initial total";
        protected const string FINAL_TOTAL = "Final total";
        protected const string DUPLICATES = "Duplicates";
        protected const string SORT_TIME = "Sort time (ms)";
        protected const string TIME_PER_TILE = "Time/Tile (ms)";
        protected List<int> sortedSubImageIndexes { get; set; }

        public Gui()
        {
            InitializeComponent();
            Status.Text = String.Empty;
        }

        protected void BackgroundProgress_DoWork(object sender, DoWorkEventArgs e)
        {
            while (ProgressBar.Value < ProgressBar.Maximum)
            {
                int progress = Program.TasksComplete;
                BackgroundProgress.ReportProgress(progress);

                //Thread.Sleep(100);
            }
        }

        protected void BackgroundProgress_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        protected void BackgroundProgress_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Value = ProgressBar.Maximum;
            ProgressBar.Enabled = false;
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
        }

        private void Padding_CheckedChanged(object sender, EventArgs e)
        {
            DrawDisplay();
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
            // Get the Filename of the selected file
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

            int tileSize = (int)TileSize.Value;
            int offsetX = (int)OffsetX.Value;
            int offsetY = (int)OffsetY.Value;

            //loadedImage = Program.LoadImage(Filename);
            Program.LoadImage(filename, tileSize, offsetX, offsetY);
            //loadedFilename = Filename;

            DisableSaving();

            int tasks = Program.Images.GetSubImageTileCount(0);
            StartProgressBar(tasks);

            int bitsPerColour = (int)Bits.Value;
            Program.LoadSubImage(bitsPerColour, tileSize);
            EnableSubImageSelection();

            int totalSubImages = Program.Images.TotalSubImages;
            sortedSubImageIndexes = new List<int>(totalSubImages);

            DrawDisplay();
            DisableUpdating();

            int totalTiles = Program.GetTileCount();
            DisplayResult(InitTotal, totalTiles);

            EnableSorting();

            //EnableUpdating();

            Status.Text = String.Format("Loaded {0}", filename);
        }

        protected void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TilesetDisplay.Image == null)
            {
                return;
            }
            if (Program.TilesetReady == false)
            {
                return;
            }

            // Get the Filename of the selected file
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

            Status.Text = String.Format("Displaying sub image");

            //Bitmap image = Program.loadedImage;
            //if (image == null)
            //{
            //    Program.LoadImage(loadedFilename, TileSize, offsetX, offsetY);
            //}

            //Bitmap loadedImage = Program.loadedImage;
            int subImageIndex = (int)SubImageSelector.Value;
            int tasks = Program.Images.GetSubImageTileCount(subImageIndex);
            StartProgressBar(tasks);

            Program.LoadSubImage(bitsPerColour, tileSize, subImageIndex);
            //image = Program.CurrentSubImage;

            //int tasks = Program.GetTileCount(image.Width, image.Height, TileSize);

            //Program.LoadAllTilesByRef(BitsPerColour, TileSize, offsetX, offsetY);
            //Program.LoadTilesThreaded(loadedImage, BitsPerColour, TileSize, offsetX, offsetY);

            DrawDisplay();
            DisableUpdating();

            int totalTiles = Program.GetTileCount();
            DisplayResult(InitTotal, totalTiles);

            //DrawTileset();

            //if (Program.TilesetReady == true)
            //{
            //    return;
            //}

            EnableSorting();
        }

        protected void Sort_Click(object sender, EventArgs e)
        {
            int bitsPerColour = (int)Bits.Value;
            int tileSize = (int)TileSize.Value;
            float patternThreshold = (float)PatternThreshold.Value;
            float colourThreshold = (float)ColourThreshold.Value;

            Status.Text = String.Format("Sorting image tiles");

            //Program.ResetTileset();

            //Bitmap loadedImage = Program.loadedImage;
            //Bitmap image = Program.CurrentSubImage;

            //int tasks = Program.GetTileCount(image.Width, image.Height, TileSize);
            int tasks = Program.GetTileCount();
            //StartProgressBar(tasks);  //testing
            Program.SortTiles(patternThreshold, colourThreshold);

            int subImageIndex = (int)SubImageSelector.Value;
            sortedSubImageIndexes.Add(subImageIndex);
            sortedSubImageIndexes.Sort();

            EnableTilesetBuild();
        }

        protected void BuildTileset_Click(object sender, EventArgs e)
        {
            //Bitmap tileset = Program.ProcessImage(loadedImage, BitsPerColour, TileSize, patternThreshold, colourThreshold);

            DrawTileset();

            EnableSaving();
            Sort.Enabled = false;

            int totalAfterSorting = Program.GetSortedTileCount();
            DisplayResult(FinalTotal, totalAfterSorting);

            int duplicates = Program.Duplicates;
            DisplayResult(Duplicates, duplicates);

            long sortTime = Program.SortTime;
            DisplayResult(SortTime, sortTime);

            int totalTiles = Program.GetTileCount();
            double timePerTile = Math.Round((double)sortTime / (double)totalTiles, 2);
            DisplayResult(TimePerTile, timePerTile);

            DisableTilesetBuild();
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
            if (Program.DisplayReady == false)
            {
                return;
            }

            int tileSize = (int)TileSize.Value;

            int offsetX = (int)OffsetX.Value;
            int offsetY = (int)OffsetY.Value;

            int zoom = (int)ImageZoom.Value;

            int subImageIndex = (int)SubImageSelector.Value;

            //Bitmap loadedImage = Program.loadedImage;
            //Bitmap image = Program.CurrentSubImage;
            Size subImageSize = Program.Images.SubImageSize;
            if (subImageSize == null)
            {
                throw new NullReferenceException("No sub image loaded");
            }

            //int width = image.Width - TileSize * (int)Math.Ceiling((double)offsetX / TileSize);
            //int height = image.Height - TileSize * (int)Math.Ceiling((double)offsetY / TileSize);
            int width = subImageSize.Width - tileSize * (int)Math.Ceiling((double)offsetX / tileSize);
            int height = subImageSize.Height - tileSize * (int)Math.Ceiling((double)offsetY / tileSize);

            int tilesWide = width / tileSize;
            int tilesHigh = height / tileSize;

            width *= zoom;
            height *= zoom;

            bool addPadding = Padding.Checked;

            if (addPadding == true)
            {
                // increase TileSize to account for spaces between tiles
                width += tilesWide - 1;
                height += tilesHigh - 1;
            }

            Bitmap displayImage = new Bitmap(width, height);
            Program.DrawAllTilesOntoImage(ref displayImage, tilesWide, tileSize, zoom, addPadding);

            ImageDisplay.Image = displayImage;

            Status.Text = String.Format("Displayed image tiles");
        }

        private void DrawTileset()
        {
            if (Program.TilesetReady == false)
            {
                return;
            }

            //TilesetDisplay.Image = null;
            int zoom = (int)TilesetZoom.Value;

            Bitmap tileset = GetTileset(zoom);
            TilesetDisplay.Image = tileset;

            Status.Text = String.Format("Displayed tileset");
        }

        private void TilesetZoom_ValueChanged(object sender, EventArgs e)
        {
            DrawTileset();
        }

        protected Bitmap GetTileset()
        {
            return GetTileset(1);
        }

        protected Bitmap GetTileset(int zoom)
        {
            if (Program.TilesetReady == false)
            {
                return null;
            }

            int tileSize = (int)TileSize.Value;

            //Bitmap loadedImage = Program.loadedImage;
            //Bitmap image = Program.CurrentSubImage;
            //PixelFormat format = image.PixelFormat;
            PixelFormat format = Program.Images.GetPixelFormat();

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
            ProgressBar.Enabled = true;
            ProgressBar.Value = 0;
            //ProgressBar.Step = 1;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = tasks;

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
            //Bitmap image = Program.CurrentSubImage;
            //if (image == null)
            //{
            //    return;
            //}
            ImageCollection imageGroup = Program.Images;
            if (imageGroup == null)
            {
                return;
            }

            Update.Enabled = true;
            ProgressBar.Value = 0;
        }

        protected void DisableUpdating()
        {
            Update.Enabled = false;
        }

        protected void EnableSubImageSelection()
        {
            SubImageSelector.Enabled = true;
            SubImageSelector.Maximum = Program.Images.TotalSubImages - 1;
        }

        protected void EnableSorting()
        {
            //Bitmap image = Program.loadedImage;
            //Bitmap image = Program.CurrentSubImage;
            //if (image == null)
            //{
            //    return;
            //}
            int tilesToSort = Program.GetTileCount();
            if (tilesToSort == 0)
            {
                return;
            }
            if (Program.DisplayReady == false)
            {
                return;
            }

            int subImageIndex = (int)SubImageSelector.Value;
            bool found = sortedSubImageIndexes.Contains(subImageIndex);
            if (found == true)
            {
                return;
            }

            Sort.Enabled = true;
            ProgressBar.Value = 0;
        }

        protected void DisableSorting()
        {
            Sort.Enabled = false;
        }

        protected void EnableTilesetBuild()
        {
            int sortedTiles = Program.GetSortedTileCount();
            if (sortedTiles == 0)
            {
                return;
            }
            if (Program.TilesetReady == false)
            {
                return;
            }

            BuildTileset.Enabled = true;
            ProgressBar.Value = 0;
            DisableSorting();
        }

        protected void DisableTilesetBuild()
        {
            BuildTileset.Enabled = false;
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

            //TilesWide.Minimum = 5 * TileSize;
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
            EnableTilesetBuild();
        }

        protected void Compressed_CheckedChanged(object sender, EventArgs e)
        {
            TilesWide.Enabled = true;
            EnableTilesetBuild();
        }

        private void BackgroundColour_Click(object sender, EventArgs e)
        {
            DialogResult result = ColourPicker.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            BackgroundColour.BackColor = ColourPicker.Color;
            Program.BackgroundColour = ColourPicker.Color;

            EnableTilesetBuild();
        }

        private void TilesWide_ValueChanged(object sender, EventArgs e)
        {
            EnableTilesetBuild();
        }

        private void SubImageSelector_ValueChanged(object sender, EventArgs e)
        {
            EnableUpdating();
        }
    }
}
