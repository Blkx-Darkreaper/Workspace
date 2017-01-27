﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverlandFlow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Program.Timer = new Timer();
            Program.Timer.Tick += new EventHandler(RefreshDisplay);
            Program.Timer.Interval = 200;

            int displayWidth = Display.Width;

            int[] grids = new int[] { 6, 4, 3, 3, 3, 2, 3, 4, 3};

            Program.GenerateTerrain(displayWidth, grids);

            Program.Timer.Start();
        }

        protected void RefreshDisplay(object sender, EventArgs e)
        {
            float interval = Program.Timer.Interval / 1000f;   // seconds

            AddWater();

            bool isDraining = Drain.Checked;
            Program.WaterFlow(interval, isDraining);

            Display.Invalidate();
        }

        protected void AddWater()
        {
            float water = (float)WaterFlow.Value;
            if (water == 0f)
            {
                return;
            }

            float flowSpeed = (float)FlowSpeed.Value;

            Program.AddWater(water, flowSpeed);
        }

        protected void Display_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int bottom = Display.Height;
            Program.DrawTerrain(g, bottom);
        }
    }
}