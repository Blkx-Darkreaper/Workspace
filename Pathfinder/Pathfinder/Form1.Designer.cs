namespace Pathfinder
{
    partial class Form1
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
            this.buildButton = new System.Windows.Forms.Button();
            this.findPathButton = new System.Windows.Forms.Button();
            this.startBoxX = new System.Windows.Forms.TextBox();
            this.startBoxY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startGroup = new System.Windows.Forms.GroupBox();
            this.endGroup = new System.Windows.Forms.GroupBox();
            this.endBoxX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.endBoxY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.map = new System.Windows.Forms.GroupBox();
            this.startGroup.SuspendLayout();
            this.endGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // buildButton
            // 
            this.buildButton.Location = new System.Drawing.Point(12, 101);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(100, 23);
            this.buildButton.TabIndex = 0;
            this.buildButton.Text = "Build";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.Build_Click);
            // 
            // findPathButton
            // 
            this.findPathButton.Enabled = false;
            this.findPathButton.Location = new System.Drawing.Point(366, 101);
            this.findPathButton.Name = "findPathButton";
            this.findPathButton.Size = new System.Drawing.Size(100, 23);
            this.findPathButton.TabIndex = 1;
            this.findPathButton.Text = "Find Path";
            this.findPathButton.UseVisualStyleBackColor = true;
            this.findPathButton.Click += new System.EventHandler(this.FindPath_Click);
            // 
            // startBoxX
            // 
            this.startBoxX.Location = new System.Drawing.Point(6, 43);
            this.startBoxX.Name = "startBoxX";
            this.startBoxX.Size = new System.Drawing.Size(100, 22);
            this.startBoxX.TabIndex = 2;
            this.startBoxX.TextChanged += new System.EventHandler(this.Box_TextChanged);
            // 
            // startBoxY
            // 
            this.startBoxY.Location = new System.Drawing.Point(113, 43);
            this.startBoxY.Name = "startBoxY";
            this.startBoxY.Size = new System.Drawing.Size(100, 22);
            this.startBoxY.TabIndex = 3;
            this.startBoxY.TextChanged += new System.EventHandler(this.Box_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // startGroup
            // 
            this.startGroup.Controls.Add(this.startBoxX);
            this.startGroup.Controls.Add(this.label2);
            this.startGroup.Controls.Add(this.startBoxY);
            this.startGroup.Controls.Add(this.label1);
            this.startGroup.Location = new System.Drawing.Point(12, 12);
            this.startGroup.Name = "startGroup";
            this.startGroup.Size = new System.Drawing.Size(224, 83);
            this.startGroup.TabIndex = 7;
            this.startGroup.TabStop = false;
            this.startGroup.Text = "Start Point";
            // 
            // endGroup
            // 
            this.endGroup.Controls.Add(this.endBoxX);
            this.endGroup.Controls.Add(this.label3);
            this.endGroup.Controls.Add(this.endBoxY);
            this.endGroup.Controls.Add(this.label4);
            this.endGroup.Location = new System.Drawing.Point(242, 12);
            this.endGroup.Name = "endGroup";
            this.endGroup.Size = new System.Drawing.Size(224, 83);
            this.endGroup.TabIndex = 8;
            this.endGroup.TabStop = false;
            this.endGroup.Text = "End Point";
            // 
            // endBoxX
            // 
            this.endBoxX.Location = new System.Drawing.Point(6, 43);
            this.endBoxX.Name = "endBoxX";
            this.endBoxX.Size = new System.Drawing.Size(100, 22);
            this.endBoxX.TabIndex = 2;
            this.endBoxX.TextChanged += new System.EventHandler(this.Box_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Y";
            // 
            // endBoxY
            // 
            this.endBoxY.Location = new System.Drawing.Point(113, 43);
            this.endBoxY.Name = "endBoxY";
            this.endBoxY.Size = new System.Drawing.Size(100, 22);
            this.endBoxY.TabIndex = 3;
            this.endBoxY.TextChanged += new System.EventHandler(this.Box_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "X";
            // 
            // map
            // 
            this.map.Location = new System.Drawing.Point(13, 131);
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(453, 346);
            this.map.TabIndex = 9;
            this.map.TabStop = false;
            this.map.Text = "Map";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 489);
            this.Controls.Add(this.map);
            this.Controls.Add(this.endGroup);
            this.Controls.Add(this.startGroup);
            this.Controls.Add(this.findPathButton);
            this.Controls.Add(this.buildButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.startGroup.ResumeLayout(false);
            this.startGroup.PerformLayout();
            this.endGroup.ResumeLayout(false);
            this.endGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.Button findPathButton;
        private System.Windows.Forms.TextBox startBoxX;
        private System.Windows.Forms.TextBox startBoxY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox startGroup;
        private System.Windows.Forms.GroupBox endGroup;
        private System.Windows.Forms.TextBox endBoxX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox endBoxY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox map;
    }
}

