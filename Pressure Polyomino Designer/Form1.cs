using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Polyominoes
{
    public partial class Form1 : Form
    {
        string fullFilePath;

        public Form1()
        {
            InitializeComponent();

            Program.LoadImages();

            InitDisplay();

            AddRecentFiles();
        }

        private void AddRecentFiles()
        {
            string fullPath = Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files");
            if (Directory.Exists(fullPath) != true)
            {
                return;
            }

            string[] allFilenames = Directory.GetFiles(fullPath);

            List<ToolStripMenuItem> allFiles = new List<ToolStripMenuItem>();
            for (int i = 0; i < allFilenames.Length; i++)
            {
                string filePath = allFilenames[i];

                string filename = Path.GetFileName(filePath);
                string[] filenameArr = filename.Split('.');

                string extension = filenameArr[1];
                if (extension.ToLower().Equals("json") != true)
                {
                    continue;
                }

                ToolStripMenuItem recentFile = new ToolStripMenuItem()
                {
                    Name = filePath,
                    Text = filenameArr[0]
                };
                recentFile.Click += new EventHandler(RecentFileToolStripMenuItem_Click);

                allFiles.Add(recentFile);
            }

            if (allFiles.Count == 0)
            {
                return;
            }

            recentFilesToolStripMenuItem.DropDownItems.AddRange(allFiles.ToArray());
        }

        private void InitDisplay()
        {
            Tile.size = Program.tileImage.Width;

            int width = Tile.size * (37 + 14);
            int height = Tile.size * (3 * 2 + 1);

            Bitmap bitmap = new Bitmap(width, height);

            pictureBox1.Size = new Size(width, height);
            pictureBox1.Image = bitmap; // Update display
            //pictureBox1.Enabled = true;
        }

        private void ResetPolys()
        {
            InitDisplay();

            Bitmap bitmap = (Bitmap)pictureBox1.Image;

            Program.GeneratePolys(ref bitmap);

            pictureBox1.Image = bitmap; // Update display
            pictureBox1.Enabled = true;
        }

        private void ReDraw()
        {
            if (pictureBox1.Image == null)
            {
                InitDisplay();
            }

            Bitmap bitmap = (Bitmap)pictureBox1.Image;

            Program.RedrawPolys(ref bitmap);

            pictureBox1.Image = bitmap; // Update display
            pictureBox1.Enabled = true;
        }

        private void SetUnsavedChanges(bool unsavedChanges)
        {
            saveToolStripMenuItem.Enabled = unsavedChanges;
        }

        private void SetSaveAsEnabled(bool enabled)
        {
            saveAsToolStripMenuItem.Enabled = enabled;
            exportAsToolStripMenuItem.Enabled = enabled;
        }

        private void SetRadioButtonsEnabled(bool enabled)
        {
            radioButton1.Enabled = enabled;
            radioButton1.Checked = enabled;

            radioButton2.Enabled = enabled;
            radioButton3.Enabled = enabled;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Enabled != true)
            {
                return;
            }
            if (pictureBox1.Image == null)
            {
                return;
            }

            Point position = pictureBox1.PointToClient(Cursor.Position);
            EndTile tile = Program.GetEndTileAtPosition(position);
            if (tile == null)
            {
                return;
            }

            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            if (mouseEvent == null)
            {
                return;
            }

            tile.ToggleTile(mouseEvent);

            ReDraw();

            SetUnsavedChanges(true);
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fullPath = Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files");
            if (Directory.Exists(fullPath) != true)
            {
                Directory.CreateDirectory(fullPath);
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = "Polyominoes",
                DefaultExt = "json",
                ValidateNames = true,
                Filter = "Json File (.json)|*.json",
                RestoreDirectory = true,
                InitialDirectory = fullPath
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ResetPolys();

            string json = JsonConvert.SerializeObject(Program.allTiles, Formatting.Indented, new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            this.fullFilePath = $"{dialog.FileName}";

            File.WriteAllText(fullFilePath, json);

            SetSaveAsEnabled(true);
            SetUnsavedChanges(false);
            SetRadioButtonsEnabled(true);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fullPath = Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files");
            if (Directory.Exists(fullPath) != true)
            {
                Directory.CreateDirectory(fullPath);
            }

            OpenFileDialog dialog = new OpenFileDialog
            {
                DefaultExt = "json",
                ValidateNames = true,
                Filter = "Json File (.json)|*.json",
                RestoreDirectory = true,
                InitialDirectory = fullPath
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string json = File.ReadAllText(dialog.FileName);
            //Program.allTiles = JsonConvert.DeserializeObject<Tile[,]>(json);
            Program.allTiles = JsonConvert.DeserializeObject<EndTile[,]>(json);

            ReDraw();

            this.fullFilePath = $"{dialog.FileName}";

            SetSaveAsEnabled(true);
            SetUnsavedChanges(false);
            SetRadioButtonsEnabled(true);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(Program.allTiles, Formatting.Indented, new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            File.WriteAllText(fullFilePath, json);

            SetUnsavedChanges(false);
        }

        private void SaveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fullPath = Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Files");
            if (Directory.Exists(fullPath) != true)
            {
                Directory.CreateDirectory(fullPath);
            }

            string defaultFilename = "Polyominoes";
            if (fullFilePath != null && fullFilePath.IndexOf('.') != -1)
            {
                string filename = Path.GetFileName(fullFilePath).Split('.')[0];

                defaultFilename = filename;
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = defaultFilename,
                DefaultExt = "json",
                ValidateNames = true,
                Filter = "Json File (.json)|*.json",
                RestoreDirectory = true,
                InitialDirectory = fullPath
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string json = JsonConvert.SerializeObject(Program.allTiles, Formatting.Indented, new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            this.fullFilePath = $"{dialog.FileName}";

            File.WriteAllText(fullFilePath, json);

            SetUnsavedChanges(false);
        }

        private void ExportAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = "Polyominoes.png",
                DefaultExt = "png",
                FilterIndex = 4,
                ValidateNames = true,
                Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpg)|*.jpg|Png Image (.png)|*.png" +
                "|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf"
            };

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

            pictureBox1.Image.Save(dialog.FileName, format);
        }

        private void RecentFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem recentFile = (ToolStripMenuItem)sender;
            if (File.Exists(recentFile.Name) != true)
            {
                return;
            }

            string json = File.ReadAllText(recentFile.Name);
            //Program.allTiles = JsonConvert.DeserializeObject<Tile[,]>(json);
            Program.allTiles = JsonConvert.DeserializeObject<EndTile[,]>(json);

            ReDraw();

            this.fullFilePath = $"{recentFile.Name}";

            SetSaveAsEnabled(true);
            SetUnsavedChanges(false);
            SetRadioButtonsEnabled(true);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Program.puzzleType = Program.PuzzleType.Elec;
            ReDraw();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Program.puzzleType = Program.PuzzleType.Mech;
            ReDraw();
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Program.puzzleType = Program.PuzzleType.Plm;
            ReDraw();
        }
    }
}
