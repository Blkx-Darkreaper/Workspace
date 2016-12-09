using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Node
{
    public partial class MainWindow : Form
    {
        public static Timer timer { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Display.Paint += Display_Paint;

            GenerateWorld();
            Start();
        }

        protected void Display_Paint(object sender, PaintEventArgs e)
        {
            Size screenSize = Display.Size;

            Program.Draw(e.Graphics, screenSize);
        }

        protected void GenerateWorld()
        {
            Program.allEntities = new List<Entity>();

            NetworkNode nodeA = new NetworkNode("A", 50 * 5, 80 * 5, 80 * 5, Color.Red);
            Program.allEntities.Add(nodeA);

            NetworkNode nodeB = new NetworkNode("B", 50 * 5, 150 * 5, 150 * 5, Color.Blue);
            Program.allEntities.Add(nodeB);

            Link link = new Link(20 * 5, nodeA, nodeB, Color.Green);
            Program.allEntities.Add(link);

            nodeA.SpawnData();
        }

        protected void Start()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(Update);
            timer.Interval = 400;
            //timer.Enabled = true;
            timer.Start();
        }

        protected void Update(object sender, EventArgs e)
        {
            int updateInterval = timer.Interval;
            float timeElapsed = updateInterval / 1000f;

            Program.Update(timeElapsed);
            Display.Refresh();
            //Display.Invalidate();
        }
    }
}
