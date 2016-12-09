namespace Pathfinder
{
    partial class MainForm
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
            this.DisplayBox = new System.Windows.Forms.Panel();
            this.MapDisplay = new System.Windows.Forms.PictureBox();
            this.RandomMap = new System.Windows.Forms.Button();
            this.FindPath = new System.Windows.Forms.Button();
            this.SMap = new System.Windows.Forms.Button();
            this.DisplayBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // DisplayBox
            // 
            this.DisplayBox.AutoScroll = true;
            this.DisplayBox.Controls.Add(this.MapDisplay);
            this.DisplayBox.Location = new System.Drawing.Point(12, 52);
            this.DisplayBox.Name = "DisplayBox";
            this.DisplayBox.Size = new System.Drawing.Size(984, 665);
            this.DisplayBox.TabIndex = 0;
            // 
            // MapDisplay
            // 
            this.MapDisplay.Location = new System.Drawing.Point(3, 3);
            this.MapDisplay.Name = "MapDisplay";
            this.MapDisplay.Size = new System.Drawing.Size(257, 194);
            this.MapDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.MapDisplay.TabIndex = 0;
            this.MapDisplay.TabStop = false;
            // 
            // RandomMap
            // 
            this.RandomMap.Location = new System.Drawing.Point(12, 12);
            this.RandomMap.Name = "RandomMap";
            this.RandomMap.Size = new System.Drawing.Size(75, 23);
            this.RandomMap.TabIndex = 1;
            this.RandomMap.Text = "Random Map";
            this.RandomMap.UseVisualStyleBackColor = true;
            this.RandomMap.Click += new System.EventHandler(this.RandomMap_Click);
            // 
            // FindPath
            // 
            this.FindPath.Enabled = false;
            this.FindPath.Location = new System.Drawing.Point(102, 12);
            this.FindPath.Name = "FindPath";
            this.FindPath.Size = new System.Drawing.Size(75, 23);
            this.FindPath.TabIndex = 2;
            this.FindPath.Text = "Find Path";
            this.FindPath.UseVisualStyleBackColor = true;
            this.FindPath.Click += new System.EventHandler(this.FindPath_Click);
            // 
            // SMap
            // 
            this.SMap.Location = new System.Drawing.Point(184, 11);
            this.SMap.Name = "SMap";
            this.SMap.Size = new System.Drawing.Size(75, 23);
            this.SMap.TabIndex = 3;
            this.SMap.Text = "S Map";
            this.SMap.UseVisualStyleBackColor = true;
            this.SMap.Click += new System.EventHandler(this.SMap_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.SMap);
            this.Controls.Add(this.FindPath);
            this.Controls.Add(this.RandomMap);
            this.Controls.Add(this.DisplayBox);
            this.Name = "MainForm";
            this.Text = "Pathfinder";
            this.DisplayBox.ResumeLayout(false);
            this.DisplayBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel DisplayBox;
        private System.Windows.Forms.PictureBox MapDisplay;
        private System.Windows.Forms.Button RandomMap;
        private System.Windows.Forms.Button FindPath;
        private System.Windows.Forms.Button SMap;
    }
}

