namespace PixelEarth
{
    partial class MainWindow
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
            this.Display = new System.Windows.Forms.PictureBox();
            this.Clock = new System.Windows.Forms.Label();
            this.UpdateSpeed = new System.Windows.Forms.GroupBox();
            this.Speed10k = new System.Windows.Forms.RadioButton();
            this.Speed1k = new System.Windows.Forms.RadioButton();
            this.Speed250 = new System.Windows.Forms.RadioButton();
            this.Speed50 = new System.Windows.Forms.RadioButton();
            this.Speed5 = new System.Windows.Forms.RadioButton();
            this.Speed2 = new System.Windows.Forms.RadioButton();
            this.Speed1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.UpdateSpeed.SuspendLayout();
            this.SuspendLayout();
            // 
            // Display
            // 
            this.Display.Location = new System.Drawing.Point(12, 25);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(360, 180);
            this.Display.TabIndex = 0;
            this.Display.TabStop = false;
            // 
            // Clock
            // 
            this.Clock.AutoSize = true;
            this.Clock.Location = new System.Drawing.Point(12, 9);
            this.Clock.Name = "Clock";
            this.Clock.Size = new System.Drawing.Size(34, 13);
            this.Clock.TabIndex = 1;
            this.Clock.Text = "Clock";
            // 
            // UpdateSpeed
            // 
            this.UpdateSpeed.Controls.Add(this.Speed10k);
            this.UpdateSpeed.Controls.Add(this.Speed1k);
            this.UpdateSpeed.Controls.Add(this.Speed250);
            this.UpdateSpeed.Controls.Add(this.Speed50);
            this.UpdateSpeed.Controls.Add(this.Speed5);
            this.UpdateSpeed.Controls.Add(this.Speed2);
            this.UpdateSpeed.Controls.Add(this.Speed1);
            this.UpdateSpeed.Location = new System.Drawing.Point(15, 211);
            this.UpdateSpeed.Name = "UpdateSpeed";
            this.UpdateSpeed.Size = new System.Drawing.Size(334, 41);
            this.UpdateSpeed.TabIndex = 2;
            this.UpdateSpeed.TabStop = false;
            this.UpdateSpeed.Text = "Speed";
            // 
            // Speed10k
            // 
            this.Speed10k.AutoSize = true;
            this.Speed10k.Location = new System.Drawing.Point(276, 18);
            this.Speed10k.Name = "Speed10k";
            this.Speed10k.Size = new System.Drawing.Size(48, 17);
            this.Speed10k.TabIndex = 6;
            this.Speed10k.Text = "x10k";
            this.Speed10k.UseVisualStyleBackColor = true;
            this.Speed10k.CheckedChanged += new System.EventHandler(this.Speed10k_CheckedChanged);
            // 
            // Speed1k
            // 
            this.Speed1k.AutoSize = true;
            this.Speed1k.Location = new System.Drawing.Point(228, 18);
            this.Speed1k.Name = "Speed1k";
            this.Speed1k.Size = new System.Drawing.Size(42, 17);
            this.Speed1k.TabIndex = 5;
            this.Speed1k.Text = "x1k";
            this.Speed1k.UseVisualStyleBackColor = true;
            this.Speed1k.CheckedChanged += new System.EventHandler(this.Speed1k_CheckedChanged);
            // 
            // Speed250
            // 
            this.Speed250.AutoSize = true;
            this.Speed250.Location = new System.Drawing.Point(180, 18);
            this.Speed250.Name = "Speed250";
            this.Speed250.Size = new System.Drawing.Size(48, 17);
            this.Speed250.TabIndex = 4;
            this.Speed250.Text = "x250";
            this.Speed250.UseVisualStyleBackColor = true;
            this.Speed250.CheckedChanged += new System.EventHandler(this.Speed250_CheckedChanged);
            // 
            // Speed50
            // 
            this.Speed50.AutoSize = true;
            this.Speed50.Location = new System.Drawing.Point(132, 19);
            this.Speed50.Name = "Speed50";
            this.Speed50.Size = new System.Drawing.Size(42, 17);
            this.Speed50.TabIndex = 3;
            this.Speed50.Text = "x50";
            this.Speed50.UseVisualStyleBackColor = true;
            this.Speed50.CheckedChanged += new System.EventHandler(this.Speed50_CheckedChanged);
            // 
            // Speed5
            // 
            this.Speed5.AutoSize = true;
            this.Speed5.Location = new System.Drawing.Point(90, 19);
            this.Speed5.Name = "Speed5";
            this.Speed5.Size = new System.Drawing.Size(36, 17);
            this.Speed5.TabIndex = 2;
            this.Speed5.Text = "x5";
            this.Speed5.UseVisualStyleBackColor = true;
            this.Speed5.CheckedChanged += new System.EventHandler(this.Speed5_CheckedChanged);
            // 
            // Speed2
            // 
            this.Speed2.AutoSize = true;
            this.Speed2.Location = new System.Drawing.Point(48, 18);
            this.Speed2.Name = "Speed2";
            this.Speed2.Size = new System.Drawing.Size(36, 17);
            this.Speed2.TabIndex = 1;
            this.Speed2.Text = "x2";
            this.Speed2.UseVisualStyleBackColor = true;
            this.Speed2.CheckedChanged += new System.EventHandler(this.Speed2_CheckedChanged);
            // 
            // Speed1
            // 
            this.Speed1.AutoSize = true;
            this.Speed1.Checked = true;
            this.Speed1.Location = new System.Drawing.Point(6, 18);
            this.Speed1.Name = "Speed1";
            this.Speed1.Size = new System.Drawing.Size(36, 17);
            this.Speed1.TabIndex = 0;
            this.Speed1.TabStop = true;
            this.Speed1.Text = "x1";
            this.Speed1.UseVisualStyleBackColor = true;
            this.Speed1.CheckedChanged += new System.EventHandler(this.Speed1_CheckedChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 264);
            this.Controls.Add(this.UpdateSpeed);
            this.Controls.Add(this.Clock);
            this.Controls.Add(this.Display);
            this.Name = "MainWindow";
            this.Text = "Pixel Earth";
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.UpdateSpeed.ResumeLayout(false);
            this.UpdateSpeed.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Display;
        private System.Windows.Forms.Label Clock;
        private System.Windows.Forms.GroupBox UpdateSpeed;
        private System.Windows.Forms.RadioButton Speed10k;
        private System.Windows.Forms.RadioButton Speed1k;
        private System.Windows.Forms.RadioButton Speed250;
        private System.Windows.Forms.RadioButton Speed50;
        private System.Windows.Forms.RadioButton Speed5;
        private System.Windows.Forms.RadioButton Speed2;
        private System.Windows.Forms.RadioButton Speed1;
    }
}

