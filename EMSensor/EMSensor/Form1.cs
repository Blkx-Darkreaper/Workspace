using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMSensor
{
    public partial class Form1 : Form
    {
        public Point receiver { get; set; }
        Sensor sensorPackage { get; set; }
        bool displayOn { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        public void SetupSensor()
        {
            displayOn = true;

            decimal propegationVelocity = 5m;

            Random rand = new Random();

            List<decimal> allFrequencies = new List<decimal>();
            int maxFrequency = 500;
            for (int i = 0; i < 5; i++)
            {
                decimal frequency = Math.Round((decimal)rand.NextDouble() * maxFrequency, 2);
                allFrequencies.Add(frequency);
            }

            Environment world = new Environment(propegationVelocity);

            int worldWidth = 100;
            int worldHeight = 150;

            receiver = new Point(worldWidth / 2, worldHeight / 2);

            //int rightX = worldWidth / 2 + 20;
            //int rightY = worldHeight / 2;
            //AddSource(rand, allFrequencies, world, rightX, rightY);

            //int upX = worldWidth / 2;
            //int upY = worldHeight / 2 - 30;
            //AddSource(rand, allFrequencies, world, upX, upY);

            //int leftX = worldWidth / 2 - 10;
            //int leftY = worldHeight / 2;
            //AddSource(rand, allFrequencies, world, leftX, leftY);

            //int downX = worldWidth / 2;
            //int downY = worldHeight / 2 + 10;
            //AddSource(rand, allFrequencies, world, downX, downY);

            AddRandomSources(rand, allFrequencies, world, worldWidth, worldHeight);

            sensorPackage = new Sensor(world, allFrequencies, 5, 0);
        }

        private void AddSource(Random rand, List<decimal> allFrequencies, Environment world, int locationX, int locationY)
        {
            Point nextLocation = new Point(locationX, locationY);
            HashSet<decimal> signatureFrequencies = new HashSet<decimal>();

            for (int j = 0; j < 5; j++)
            {
                int randomIndex = rand.Next(allFrequencies.Count);
                decimal frequency = allFrequencies[randomIndex];
                signatureFrequencies.Add(frequency);
            }

            int min = rand.Next(20);
            int max = rand.Next(100);

            if (max < min)
            {
                int placeholder = max;
                max = min;
                min = placeholder;
            }

            EmissionSource toAdd = new EmissionSource(nextLocation, signatureFrequencies.ToList(), min, max);
            world.AddEmissionSource(toAdd);
        }

        private static void AddRandomSources(Random rand, List<decimal> allFrequencies, Environment world, int worldWidth, int worldHeight)
        {
            for (int i = 0; i < 5; i++)
            {
                int X = rand.Next(worldWidth);
                int Y = rand.Next(worldHeight);
                Point nextLocation = new Point(X, Y);
                HashSet<decimal> signatureFrequencies = new HashSet<decimal>();

                for (int j = 0; j < 5; j++)
                {
                    int randomIndex = rand.Next(allFrequencies.Count);
                    decimal frequency = allFrequencies[randomIndex];
                    signatureFrequencies.Add(frequency);
                }

                int min = rand.Next(20);
                int max = rand.Next(100);

                if (max < min)
                {
                    int placeholder = max;
                    max = min;
                    min = placeholder;
                }

                EmissionSource toAdd = new EmissionSource(nextLocation, signatureFrequencies.ToList(), min, max);
                world.AddEmissionSource(toAdd);
            }
        }

        private void setup_Click(object sender, EventArgs e)
        {
            SetupSensor();
            Update.Enabled = true;

            Up.Enabled = true;
            Down.Enabled = true;
            Left.Enabled = true;
            Right.Enabled = true;

            UpdateScreen(36);
            UpdateWorld();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            decimal timeElapsed = 1;
            sensorPackage.Update(timeElapsed);
            UpdateScreen(18);
            UpdateWorld();
        }

        private void UpdateWorld()
        {
            World.Refresh();
            if (displayOn == false)
            {
                return;
            }

            Graphics canvas = World.CreateGraphics();

            List<Emission> allEmissions = sensorPackage.signalData.allEmissions;
            Pen pen = new Pen(Color.Red);
            int cornerX;
            int cornerY;
            Rectangle boundingBox;

            foreach (Emission toDraw in allEmissions)
            {
                Point origin = toDraw.origin;
                decimal radius = 3*toDraw.distancePropegated;
                int diameter = (int)radius * 2;
                cornerX = (int)(3*origin.X - radius);
                cornerY = (int)(3*origin.Y - radius);

                boundingBox = new Rectangle(cornerX, cornerY, diameter, diameter);
                canvas.DrawEllipse(pen, boundingBox);
            }

            List<EmissionSource> allSources = sensorPackage.signalData.allSources;
            Brush brush = new SolidBrush(Color.Blue);
            int dotSize;
            int halfDot;
            foreach (EmissionSource source in allSources)
            {
                Point origin = source.location;
                dotSize = (int)source.emissionSignature.Values.Max() / 5;
                halfDot = dotSize / 2;

                cornerX = (int)(3*origin.X - halfDot);
                cornerY = (int)(3*origin.Y - halfDot);

                boundingBox = new Rectangle(cornerX, cornerY, dotSize, dotSize);
                canvas.FillEllipse(brush, boundingBox);
            }

            brush = new SolidBrush(Color.Black);
            dotSize = 10;
            halfDot = dotSize / 2;
            cornerX = (int)(3*receiver.X - halfDot);
            cornerY = (int)(3*receiver.Y - halfDot);

            boundingBox = new Rectangle(cornerX, cornerY, dotSize, dotSize);
            canvas.FillEllipse(brush, boundingBox);
        }

        private void UpdateScreen(int bearingPoints)
        {
            Screen.Refresh();
            Graphics canvas = Screen.CreateGraphics();
            int screenWidth = Screen.Width;
            int screenHeight = Screen.Height;

            Dictionary<decimal, List<Point>> allReadings = sensorPackage.GetAllReadings(screenWidth, screenHeight, bearingPoints, receiver);
            foreach (decimal frequency in allReadings.Keys)
            {
                Color drawColour = GetFrequencyColour(frequency, 500);
                Pen pen = new Pen(drawColour);

                List<Point> allPoints = allReadings[frequency];

                Point firstPoint = allPoints[0];
                for (int i = 1; i < allPoints.Count; i++)
                {
                    Point secondPoint = allPoints[i];

                    canvas.DrawLine(pen, firstPoint, secondPoint);

                    firstPoint = secondPoint;
                }
            }
        }

        private static Color GetFrequencyColour(decimal frequency, decimal maxFrequency)
        {
            var white = Color.White.ToArgb();
            var black = Color.Black.ToArgb();
            int aRGB = (int)(frequency * (black - white) / maxFrequency);
            Color drawColour = Color.FromArgb(aRGB);
            return drawColour;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            displayOn = !displayOn;
            UpdateScreen(18);
            UpdateWorld();
        }

        private void Up_Click(object sender, EventArgs e)
        {
            int x = receiver.X;
            int y = receiver.Y;

            receiver = new Point(x, y - 2);

            Update_Click(sender, e);
        }

        private void Down_Click(object sender, EventArgs e)
        {
            int x = receiver.X;
            int y = receiver.Y;

            receiver = new Point(x, y + 2);

            Update_Click(sender, e);
        }

        private void Left_Click(object sender, EventArgs e)
        {
            int x = receiver.X;
            int y = receiver.Y;

            receiver = new Point(x - 2, y);

            Update_Click(sender, e);
        }

        private void Right_Click(object sender, EventArgs e)
        {
            int x = receiver.X;
            int y = receiver.Y;

            receiver = new Point(x + 2, y);

            Update_Click(sender, e);
        }
    }
}
