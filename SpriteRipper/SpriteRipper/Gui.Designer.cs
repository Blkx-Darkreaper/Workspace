namespace SpriteRipper
{
    partial class Gui
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Sort = new System.Windows.Forms.Button();
            this.ImageDisplay = new System.Windows.Forms.PictureBox();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TileSizeBox = new System.Windows.Forms.GroupBox();
            this.Preset256 = new System.Windows.Forms.RadioButton();
            this.Preset128 = new System.Windows.Forms.RadioButton();
            this.Preset64 = new System.Windows.Forms.RadioButton();
            this.Preset32 = new System.Windows.Forms.RadioButton();
            this.Preset16 = new System.Windows.Forms.RadioButton();
            this.Preset8 = new System.Windows.Forms.RadioButton();
            this.TileSize = new System.Windows.Forms.NumericUpDown();
            this.DisplayBox = new System.Windows.Forms.GroupBox();
            this.Update = new System.Windows.Forms.Button();
            this.BitBox = new System.Windows.Forms.GroupBox();
            this.Bits = new System.Windows.Forms.NumericUpDown();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.TilesetPanel = new System.Windows.Forms.Panel();
            this.TilesetDisplay = new System.Windows.Forms.PictureBox();
            this.ZoomBox = new System.Windows.Forms.GroupBox();
            this.Zoom = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ColourThreshold = new System.Windows.Forms.NumericUpDown();
            this.PatternThreshold = new System.Windows.Forms.NumericUpDown();
            this.BackgroundProgress = new System.ComponentModel.BackgroundWorker();
            this.Offset = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OffsetY = new System.Windows.Forms.NumericUpDown();
            this.OffsetX = new System.Windows.Forms.NumericUpDown();
            this.Grouping = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TilesWide = new System.Windows.Forms.NumericUpDown();
            this.Compressed = new System.Windows.Forms.RadioButton();
            this.Grouped = new System.Windows.Forms.RadioButton();
            this.Status = new System.Windows.Forms.Label();
            this.ColourBox = new System.Windows.Forms.GroupBox();
            this.BackgroundColour = new System.Windows.Forms.PictureBox();
            this.Results = new System.Windows.Forms.GroupBox();
            this.TimePerTile = new System.Windows.Forms.Label();
            this.SortTime = new System.Windows.Forms.Label();
            this.FinalTotal = new System.Windows.Forms.Label();
            this.InitTotal = new System.Windows.Forms.Label();
            this.Duplicates = new System.Windows.Forms.Label();
            this.ColourPicker = new System.Windows.Forms.ColorDialog();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplay)).BeginInit();
            this.Menu.SuspendLayout();
            this.TileSizeBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileSize)).BeginInit();
            this.DisplayBox.SuspendLayout();
            this.BitBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bits)).BeginInit();
            this.ImagePanel.SuspendLayout();
            this.TilesetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetDisplay)).BeginInit();
            this.ZoomBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Zoom)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColourThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatternThreshold)).BeginInit();
            this.Offset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetX)).BeginInit();
            this.Grouping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesWide)).BeginInit();
            this.ColourBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColour)).BeginInit();
            this.Results.SuspendLayout();
            this.SuspendLayout();
            // 
            // Sort
            // 
            this.Sort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Sort.Enabled = false;
            this.Sort.Location = new System.Drawing.Point(67, 23);
            this.Sort.Name = "Sort";
            this.Sort.Size = new System.Drawing.Size(45, 33);
            this.Sort.TabIndex = 0;
            this.Sort.Text = "Sort";
            this.Sort.UseVisualStyleBackColor = true;
            this.Sort.Click += new System.EventHandler(this.Sort_Click);
            // 
            // ImageDisplay
            // 
            this.ImageDisplay.Location = new System.Drawing.Point(7, 3);
            this.ImageDisplay.Name = "ImageDisplay";
            this.ImageDisplay.Size = new System.Drawing.Size(585, 490);
            this.ImageDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ImageDisplay.TabIndex = 2;
            this.ImageDisplay.TabStop = false;
            // 
            // Progress
            // 
            this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Progress.Enabled = false;
            this.Progress.Location = new System.Drawing.Point(118, 23);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(233, 33);
            this.Progress.TabIndex = 3;
            // 
            // Menu
            // 
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.HelpMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(1008, 24);
            this.Menu.TabIndex = 7;
            this.Menu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuItem,
            this.SaveMenuItem,
            this.SaveAsMenuItem,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "File";
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Size = new System.Drawing.Size(152, 22);
            this.OpenMenuItem.Text = "Open";
            this.OpenMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Enabled = false;
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SaveMenuItem.Text = "Save";
            // 
            // SaveAsMenuItem
            // 
            this.SaveAsMenuItem.Enabled = false;
            this.SaveAsMenuItem.Name = "SaveAsMenuItem";
            this.SaveAsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SaveAsMenuItem.Text = "Save As";
            this.SaveAsMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpMenuItem.Text = "Help";
            // 
            // TileSizeBox
            // 
            this.TileSizeBox.Controls.Add(this.Preset256);
            this.TileSizeBox.Controls.Add(this.Preset128);
            this.TileSizeBox.Controls.Add(this.Preset64);
            this.TileSizeBox.Controls.Add(this.Preset32);
            this.TileSizeBox.Controls.Add(this.Preset16);
            this.TileSizeBox.Controls.Add(this.Preset8);
            this.TileSizeBox.Controls.Add(this.TileSize);
            this.TileSizeBox.Location = new System.Drawing.Point(376, 27);
            this.TileSizeBox.Name = "TileSizeBox";
            this.TileSizeBox.Size = new System.Drawing.Size(333, 75);
            this.TileSizeBox.TabIndex = 8;
            this.TileSizeBox.TabStop = false;
            this.TileSizeBox.Text = "Tile Size";
            // 
            // Preset256
            // 
            this.Preset256.AutoSize = true;
            this.Preset256.Location = new System.Drawing.Point(285, 26);
            this.Preset256.Name = "Preset256";
            this.Preset256.Size = new System.Drawing.Size(43, 17);
            this.Preset256.TabIndex = 7;
            this.Preset256.TabStop = true;
            this.Preset256.Text = "256";
            this.Preset256.UseVisualStyleBackColor = true;
            this.Preset256.CheckedChanged += new System.EventHandler(this.Preset256_CheckedChanged);
            // 
            // Preset128
            // 
            this.Preset128.AutoSize = true;
            this.Preset128.Location = new System.Drawing.Point(236, 26);
            this.Preset128.Name = "Preset128";
            this.Preset128.Size = new System.Drawing.Size(43, 17);
            this.Preset128.TabIndex = 6;
            this.Preset128.TabStop = true;
            this.Preset128.Text = "128";
            this.Preset128.UseVisualStyleBackColor = true;
            this.Preset128.CheckedChanged += new System.EventHandler(this.Preset128_CheckedChanged);
            // 
            // Preset64
            // 
            this.Preset64.AutoSize = true;
            this.Preset64.Location = new System.Drawing.Point(193, 26);
            this.Preset64.Name = "Preset64";
            this.Preset64.Size = new System.Drawing.Size(37, 17);
            this.Preset64.TabIndex = 5;
            this.Preset64.TabStop = true;
            this.Preset64.Text = "64";
            this.Preset64.UseVisualStyleBackColor = true;
            this.Preset64.CheckedChanged += new System.EventHandler(this.Preset64_CheckedChanged);
            // 
            // Preset32
            // 
            this.Preset32.AutoSize = true;
            this.Preset32.Location = new System.Drawing.Point(150, 26);
            this.Preset32.Name = "Preset32";
            this.Preset32.Size = new System.Drawing.Size(37, 17);
            this.Preset32.TabIndex = 4;
            this.Preset32.TabStop = true;
            this.Preset32.Text = "32";
            this.Preset32.UseVisualStyleBackColor = true;
            this.Preset32.CheckedChanged += new System.EventHandler(this.Preset32_CheckedChanged);
            // 
            // Preset16
            // 
            this.Preset16.AutoSize = true;
            this.Preset16.Checked = true;
            this.Preset16.Location = new System.Drawing.Point(107, 26);
            this.Preset16.Name = "Preset16";
            this.Preset16.Size = new System.Drawing.Size(37, 17);
            this.Preset16.TabIndex = 3;
            this.Preset16.TabStop = true;
            this.Preset16.Text = "16";
            this.Preset16.UseVisualStyleBackColor = true;
            this.Preset16.CheckedChanged += new System.EventHandler(this.Preset16_CheckedChanged);
            // 
            // Preset8
            // 
            this.Preset8.AutoSize = true;
            this.Preset8.Location = new System.Drawing.Point(70, 26);
            this.Preset8.Name = "Preset8";
            this.Preset8.Size = new System.Drawing.Size(31, 17);
            this.Preset8.TabIndex = 2;
            this.Preset8.Text = "8";
            this.Preset8.UseVisualStyleBackColor = true;
            this.Preset8.CheckedChanged += new System.EventHandler(this.Preset8_CheckedChanged);
            // 
            // TileSize
            // 
            this.TileSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TileSize.Location = new System.Drawing.Point(9, 23);
            this.TileSize.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.TileSize.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.TileSize.Name = "TileSize";
            this.TileSize.Size = new System.Drawing.Size(55, 20);
            this.TileSize.TabIndex = 0;
            this.TileSize.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.TileSize.ValueChanged += new System.EventHandler(this.TileSize_ValueChanged);
            // 
            // DisplayBox
            // 
            this.DisplayBox.Controls.Add(this.Update);
            this.DisplayBox.Controls.Add(this.Sort);
            this.DisplayBox.Controls.Add(this.Progress);
            this.DisplayBox.Location = new System.Drawing.Point(12, 27);
            this.DisplayBox.Name = "DisplayBox";
            this.DisplayBox.Size = new System.Drawing.Size(358, 75);
            this.DisplayBox.TabIndex = 9;
            this.DisplayBox.TabStop = false;
            this.DisplayBox.Text = "Display";
            // 
            // Update
            // 
            this.Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Update.Enabled = false;
            this.Update.Location = new System.Drawing.Point(6, 23);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(55, 33);
            this.Update.TabIndex = 4;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // BitBox
            // 
            this.BitBox.Controls.Add(this.Bits);
            this.BitBox.Location = new System.Drawing.Point(356, 108);
            this.BitBox.Name = "BitBox";
            this.BitBox.Size = new System.Drawing.Size(77, 75);
            this.BitBox.TabIndex = 9;
            this.BitBox.TabStop = false;
            this.BitBox.Text = "Bits/Colour";
            // 
            // Bits
            // 
            this.Bits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bits.Location = new System.Drawing.Point(20, 23);
            this.Bits.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.Bits.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.Bits.Name = "Bits";
            this.Bits.Size = new System.Drawing.Size(37, 20);
            this.Bits.TabIndex = 0;
            this.Bits.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Bits.ValueChanged += new System.EventHandler(this.Bits_ValueChanged);
            // 
            // ImagePanel
            // 
            this.ImagePanel.AutoScroll = true;
            this.ImagePanel.Controls.Add(this.ImageDisplay);
            this.ImagePanel.Location = new System.Drawing.Point(12, 189);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(600, 500);
            this.ImagePanel.TabIndex = 11;
            // 
            // TilesetPanel
            // 
            this.TilesetPanel.AutoScroll = true;
            this.TilesetPanel.Controls.Add(this.TilesetDisplay);
            this.TilesetPanel.Location = new System.Drawing.Point(618, 189);
            this.TilesetPanel.Name = "TilesetPanel";
            this.TilesetPanel.Size = new System.Drawing.Size(378, 500);
            this.TilesetPanel.TabIndex = 12;
            // 
            // TilesetDisplay
            // 
            this.TilesetDisplay.Location = new System.Drawing.Point(7, 3);
            this.TilesetDisplay.Name = "TilesetDisplay";
            this.TilesetDisplay.Size = new System.Drawing.Size(363, 490);
            this.TilesetDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.TilesetDisplay.TabIndex = 2;
            this.TilesetDisplay.TabStop = false;
            // 
            // ZoomBox
            // 
            this.ZoomBox.Controls.Add(this.Zoom);
            this.ZoomBox.Location = new System.Drawing.Point(100, 108);
            this.ZoomBox.Name = "ZoomBox";
            this.ZoomBox.Size = new System.Drawing.Size(77, 75);
            this.ZoomBox.TabIndex = 10;
            this.ZoomBox.TabStop = false;
            this.ZoomBox.Text = "Zoom";
            // 
            // Zoom
            // 
            this.Zoom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Zoom.Location = new System.Drawing.Point(20, 23);
            this.Zoom.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Zoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Zoom.Name = "Zoom";
            this.Zoom.Size = new System.Drawing.Size(37, 20);
            this.Zoom.TabIndex = 0;
            this.Zoom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Zoom.ValueChanged += new System.EventHandler(this.Zoom_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ColourThreshold);
            this.groupBox1.Controls.Add(this.PatternThreshold);
            this.groupBox1.Location = new System.Drawing.Point(439, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(104, 75);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thresholds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Colour";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Pattern";
            // 
            // ColourThreshold
            // 
            this.ColourThreshold.DecimalPlaces = 2;
            this.ColourThreshold.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.ColourThreshold.Location = new System.Drawing.Point(53, 19);
            this.ColourThreshold.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ColourThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ColourThreshold.Name = "ColourThreshold";
            this.ColourThreshold.Size = new System.Drawing.Size(41, 20);
            this.ColourThreshold.TabIndex = 1;
            this.ColourThreshold.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.ColourThreshold.ValueChanged += new System.EventHandler(this.ColourThreshold_ValueChanged);
            // 
            // PatternThreshold
            // 
            this.PatternThreshold.DecimalPlaces = 2;
            this.PatternThreshold.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.PatternThreshold.Location = new System.Drawing.Point(6, 19);
            this.PatternThreshold.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PatternThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PatternThreshold.Name = "PatternThreshold";
            this.PatternThreshold.Size = new System.Drawing.Size(41, 20);
            this.PatternThreshold.TabIndex = 0;
            this.PatternThreshold.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PatternThreshold.ValueChanged += new System.EventHandler(this.PatternThreshold_ValueChanged);
            // 
            // BackgroundProgress
            // 
            this.BackgroundProgress.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundProgress_DoWork);
            this.BackgroundProgress.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundProgress_ProgressChanged);
            this.BackgroundProgress.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundProgress_RunWorkerCompleted);
            // 
            // Offset
            // 
            this.Offset.Controls.Add(this.label4);
            this.Offset.Controls.Add(this.label3);
            this.Offset.Controls.Add(this.OffsetY);
            this.Offset.Controls.Add(this.OffsetX);
            this.Offset.Location = new System.Drawing.Point(11, 108);
            this.Offset.Name = "Offset";
            this.Offset.Size = new System.Drawing.Size(83, 75);
            this.Offset.TabIndex = 13;
            this.Offset.TabStop = false;
            this.Offset.Text = "Offset";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "X";
            // 
            // OffsetY
            // 
            this.OffsetY.Location = new System.Drawing.Point(7, 45);
            this.OffsetY.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.OffsetY.Name = "OffsetY";
            this.OffsetY.Size = new System.Drawing.Size(42, 20);
            this.OffsetY.TabIndex = 1;
            this.OffsetY.ValueChanged += new System.EventHandler(this.OffsetY_ValueChanged);
            // 
            // OffsetX
            // 
            this.OffsetX.Location = new System.Drawing.Point(7, 19);
            this.OffsetX.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.OffsetX.Name = "OffsetX";
            this.OffsetX.Size = new System.Drawing.Size(42, 20);
            this.OffsetX.TabIndex = 0;
            this.OffsetX.ValueChanged += new System.EventHandler(this.OffsetX_ValueChanged);
            // 
            // Grouping
            // 
            this.Grouping.Controls.Add(this.label5);
            this.Grouping.Controls.Add(this.TilesWide);
            this.Grouping.Controls.Add(this.Compressed);
            this.Grouping.Controls.Add(this.Grouped);
            this.Grouping.Location = new System.Drawing.Point(549, 108);
            this.Grouping.Name = "Grouping";
            this.Grouping.Size = new System.Drawing.Size(160, 75);
            this.Grouping.TabIndex = 14;
            this.Grouping.TabStop = false;
            this.Grouping.Text = "Tileset";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(96, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Tiles wide";
            // 
            // TilesWide
            // 
            this.TilesWide.Enabled = false;
            this.TilesWide.Location = new System.Drawing.Point(96, 42);
            this.TilesWide.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.TilesWide.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.TilesWide.Name = "TilesWide";
            this.TilesWide.Size = new System.Drawing.Size(54, 20);
            this.TilesWide.TabIndex = 2;
            this.TilesWide.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TilesWide.ValueChanged += new System.EventHandler(this.TilesWide_ValueChanged);
            // 
            // Compressed
            // 
            this.Compressed.AutoSize = true;
            this.Compressed.Location = new System.Drawing.Point(6, 42);
            this.Compressed.Name = "Compressed";
            this.Compressed.Size = new System.Drawing.Size(83, 17);
            this.Compressed.TabIndex = 1;
            this.Compressed.TabStop = true;
            this.Compressed.Text = "Compressed";
            this.Compressed.UseVisualStyleBackColor = true;
            this.Compressed.CheckedChanged += new System.EventHandler(this.Compressed_CheckedChanged);
            // 
            // Grouped
            // 
            this.Grouped.AutoSize = true;
            this.Grouped.Checked = true;
            this.Grouped.Location = new System.Drawing.Point(6, 19);
            this.Grouped.Name = "Grouped";
            this.Grouped.Size = new System.Drawing.Size(66, 17);
            this.Grouped.TabIndex = 0;
            this.Grouped.TabStop = true;
            this.Grouped.Text = "Grouped";
            this.Grouped.UseVisualStyleBackColor = true;
            this.Grouped.CheckedChanged += new System.EventHandler(this.Grouped_CheckedChanged);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(12, 707);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(72, 13);
            this.Status.TabIndex = 15;
            this.Status.Text = "Status Output";
            // 
            // ColourBox
            // 
            this.ColourBox.Controls.Add(this.BackgroundColour);
            this.ColourBox.Location = new System.Drawing.Point(270, 108);
            this.ColourBox.Name = "ColourBox";
            this.ColourBox.Size = new System.Drawing.Size(80, 75);
            this.ColourBox.TabIndex = 16;
            this.ColourBox.TabStop = false;
            this.ColourBox.Text = "Background";
            // 
            // BackgroundColour
            // 
            this.BackgroundColour.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BackgroundColour.BackColor = System.Drawing.Color.LightSeaGreen;
            this.BackgroundColour.Location = new System.Drawing.Point(31, 23);
            this.BackgroundColour.Name = "BackgroundColour";
            this.BackgroundColour.Size = new System.Drawing.Size(24, 20);
            this.BackgroundColour.TabIndex = 0;
            this.BackgroundColour.TabStop = false;
            this.BackgroundColour.Click += new System.EventHandler(this.BackgroundColour_Click);
            // 
            // Results
            // 
            this.Results.Controls.Add(this.TimePerTile);
            this.Results.Controls.Add(this.SortTime);
            this.Results.Controls.Add(this.FinalTotal);
            this.Results.Controls.Add(this.InitTotal);
            this.Results.Controls.Add(this.Duplicates);
            this.Results.Location = new System.Drawing.Point(715, 27);
            this.Results.Name = "Results";
            this.Results.Size = new System.Drawing.Size(281, 156);
            this.Results.TabIndex = 17;
            this.Results.TabStop = false;
            this.Results.Text = "Sorting Results";
            // 
            // TimePerTile
            // 
            this.TimePerTile.AutoSize = true;
            this.TimePerTile.Location = new System.Drawing.Point(6, 81);
            this.TimePerTile.Name = "TimePerTile";
            this.TimePerTile.Size = new System.Drawing.Size(77, 13);
            this.TimePerTile.TabIndex = 4;
            this.TimePerTile.Text = "Time/Tile (ms):";
            // 
            // SortTime
            // 
            this.SortTime.AutoSize = true;
            this.SortTime.Location = new System.Drawing.Point(6, 62);
            this.SortTime.Name = "SortTime";
            this.SortTime.Size = new System.Drawing.Size(73, 13);
            this.SortTime.TabIndex = 3;
            this.SortTime.Text = "Sort time (ms):";
            // 
            // FinalTotal
            // 
            this.FinalTotal.AutoSize = true;
            this.FinalTotal.Location = new System.Drawing.Point(6, 33);
            this.FinalTotal.Name = "FinalTotal";
            this.FinalTotal.Size = new System.Drawing.Size(64, 13);
            this.FinalTotal.TabIndex = 2;
            this.FinalTotal.Text = "Sorted total:";
            // 
            // InitTotal
            // 
            this.InitTotal.AutoSize = true;
            this.InitTotal.Location = new System.Drawing.Point(6, 16);
            this.InitTotal.Name = "InitTotal";
            this.InitTotal.Size = new System.Drawing.Size(57, 13);
            this.InitTotal.TabIndex = 1;
            this.InitTotal.Text = "Initial total:";
            // 
            // Duplicates
            // 
            this.Duplicates.AutoSize = true;
            this.Duplicates.Location = new System.Drawing.Point(6, 46);
            this.Duplicates.Name = "Duplicates";
            this.Duplicates.Size = new System.Drawing.Size(60, 13);
            this.Duplicates.TabIndex = 0;
            this.Duplicates.Text = "Duplicates:";
            // 
            // ColourPicker
            // 
            this.ColourPicker.Color = System.Drawing.Color.LightSeaGreen;
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(152, 22);
            this.AboutMenuItem.Text = "About";
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.Results);
            this.Controls.Add(this.ColourBox);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Grouping);
            this.Controls.Add(this.Offset);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ZoomBox);
            this.Controls.Add(this.TilesetPanel);
            this.Controls.Add(this.ImagePanel);
            this.Controls.Add(this.BitBox);
            this.Controls.Add(this.DisplayBox);
            this.Controls.Add(this.TileSizeBox);
            this.Controls.Add(this.Menu);
            this.MainMenuStrip = this.Menu;
            this.Name = "Gui";
            this.Text = "Tileset Ripper";
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplay)).EndInit();
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.TileSizeBox.ResumeLayout(false);
            this.TileSizeBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileSize)).EndInit();
            this.DisplayBox.ResumeLayout(false);
            this.BitBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Bits)).EndInit();
            this.ImagePanel.ResumeLayout(false);
            this.ImagePanel.PerformLayout();
            this.TilesetPanel.ResumeLayout(false);
            this.TilesetPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetDisplay)).EndInit();
            this.ZoomBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Zoom)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColourThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatternThreshold)).EndInit();
            this.Offset.ResumeLayout(false);
            this.Offset.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetX)).EndInit();
            this.Grouping.ResumeLayout(false);
            this.Grouping.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesWide)).EndInit();
            this.ColourBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColour)).EndInit();
            this.Results.ResumeLayout(false);
            this.Results.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Sort;
        private System.Windows.Forms.PictureBox ImageDisplay;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.GroupBox TileSizeBox;
        private System.Windows.Forms.GroupBox DisplayBox;
        private System.Windows.Forms.GroupBox BitBox;
        private System.Windows.Forms.Panel ImagePanel;
        private System.Windows.Forms.Panel TilesetPanel;
        private System.Windows.Forms.PictureBox TilesetDisplay;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.NumericUpDown TileSize;
        private System.Windows.Forms.NumericUpDown Bits;
        private System.Windows.Forms.GroupBox ZoomBox;
        private System.Windows.Forms.NumericUpDown Zoom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown ColourThreshold;
        private System.Windows.Forms.NumericUpDown PatternThreshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.ToolStripMenuItem SaveAsMenuItem;
        private System.ComponentModel.BackgroundWorker BackgroundProgress;
        private System.Windows.Forms.RadioButton Preset16;
        private System.Windows.Forms.RadioButton Preset8;
        private System.Windows.Forms.RadioButton Preset64;
        private System.Windows.Forms.RadioButton Preset32;
        private System.Windows.Forms.RadioButton Preset128;
        private System.Windows.Forms.RadioButton Preset256;
        private System.Windows.Forms.GroupBox Offset;
        private System.Windows.Forms.NumericUpDown OffsetX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown OffsetY;
        private System.Windows.Forms.GroupBox Grouping;
        private System.Windows.Forms.RadioButton Compressed;
        private System.Windows.Forms.RadioButton Grouped;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown TilesWide;
        private System.Windows.Forms.GroupBox ColourBox;
        private System.Windows.Forms.GroupBox Results;
        private System.Windows.Forms.Label InitTotal;
        private System.Windows.Forms.Label Duplicates;
        private System.Windows.Forms.Label FinalTotal;
        private System.Windows.Forms.ColorDialog ColourPicker;
        private System.Windows.Forms.PictureBox BackgroundColour;
        private System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
        private System.Windows.Forms.Label SortTime;
        private System.Windows.Forms.Label TimePerTile;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
    }
}

