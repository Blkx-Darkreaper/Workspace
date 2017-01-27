namespace Bits
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

        #region Windows Form Designer generated opcode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the opcode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Display = new System.Windows.Forms.PictureBox();
            this.InputDisplay = new System.Windows.Forms.TextBox();
            this.Execute = new System.Windows.Forms.Button();
            this.DisplayPanel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Input = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.DisplayPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Display
            // 
            this.Display.Location = new System.Drawing.Point(3, 3);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(237, 235);
            this.Display.TabIndex = 0;
            this.Display.TabStop = false;
            this.Display.Paint += new System.Windows.Forms.PaintEventHandler(this.Update);
            // 
            // InputDisplay
            // 
            this.InputDisplay.AcceptsReturn = true;
            this.InputDisplay.Location = new System.Drawing.Point(572, 30);
            this.InputDisplay.Multiline = true;
            this.InputDisplay.Name = "InputDisplay";
            this.InputDisplay.ReadOnly = true;
            this.InputDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputDisplay.Size = new System.Drawing.Size(200, 490);
            this.InputDisplay.TabIndex = 1;
            // 
            // Execute
            // 
            this.Execute.Location = new System.Drawing.Point(697, 526);
            this.Execute.Name = "Execute";
            this.Execute.Size = new System.Drawing.Size(75, 23);
            this.Execute.TabIndex = 2;
            this.Execute.Text = "Execute";
            this.Execute.UseVisualStyleBackColor = true;
            this.Execute.Click += new System.EventHandler(this.Execute_Click);
            // 
            // DisplayPanel
            // 
            this.DisplayPanel.AutoScroll = true;
            this.DisplayPanel.Controls.Add(this.Display);
            this.DisplayPanel.Location = new System.Drawing.Point(12, 27);
            this.DisplayPanel.Name = "DisplayPanel";
            this.DisplayPanel.Size = new System.Drawing.Size(554, 519);
            this.DisplayPanel.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(572, 526);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(119, 20);
            this.Input.TabIndex = 5;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.DisplayPanel);
            this.Controls.Add(this.Execute);
            this.Controls.Add(this.InputDisplay);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Bits";
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.DisplayPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Display;
        private System.Windows.Forms.TextBox InputDisplay;
        private System.Windows.Forms.Button Execute;
        private System.Windows.Forms.Panel DisplayPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox Input;
    }
}

