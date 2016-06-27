using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Node
{
    public class NetworkNode : Entity
    {
        public string address { get; protected set; }
        public List<Link> incoming { get; set; }
        public List<Link> outgoing { get; set; }
        public List<Data> allData { get; set; }

        public NetworkNode(string address, int diameter, Point location, Color mainColour, Color backgroundColour)
            : base(location, diameter, mainColour, backgroundColour)
        {
            this.address = address;
            this.incoming = new List<Link>();
            this.outgoing = new List<Link>();
            this.allData = new List<Data>();
        }

        public NetworkNode(string address, int diameter, int x, int y, Color mainColour, Color backgroundColour) : this(address, diameter, new Point(x, y), mainColour, backgroundColour) { }

        public NetworkNode(string address, int diameter, Point location, Color mainColour)
            : base(location, diameter, mainColour)
        {
            this.address = address;
            this.incoming = new List<Link>();
            this.outgoing = new List<Link>();
            this.allData = new List<Data>();
        }

        public NetworkNode(string address, int diameter, int x, int y, Color mainColour) : this(address, diameter, new Point(x, y), mainColour) { }

        public override void Update(float timeElapsed)
        {
            foreach (Entity data in allData)
            {
                int diameter = size;

                bool orbiting = data.orbiting;
                if (orbiting == true)
                {
                    data.Orbit(location, diameter / 2 - 1, timeElapsed);
                }
                else
                {
                    data.Update(timeElapsed);
                    bool collisionDetected = DetectCollision(data);
                    if (collisionDetected == false)
                    {
                        continue;
                    }

                    Point collisionPoint = Program.GetNearestSecantLinePoint(data.location, data.heading, location, diameter / 2);

                    int arcAngle = (int)Math.Round(Program.GetArcAngle(collisionPoint, location, diameter / 2), 0);
                    int sign = 0;
                    if (arcAngle != 0)
                    {
                        sign = arcAngle / Math.Abs(arcAngle);
                    }

                    int deltaX = collisionPoint.X - location.X;
                    int deltaY = collisionPoint.Y - location.Y;

                    //int normalAngle = (180 + Math.Abs(arcAngle)) % 360;
                    int normalAngle = 180;
                    if (deltaY < 0)
                    {
                        normalAngle -= arcAngle;
                    }
                    else
                    {

                        if (deltaX < 0)
                        {
                            normalAngle -= arcAngle;
                            if (deltaY != 0)
                            {
                                normalAngle = 90 + arcAngle;
                            }
                        }
                        else
                        {
                            normalAngle = 360 - arcAngle;
                        }
                    }

                    normalAngle %= 360;

                    data.Bounce(normalAngle);
                }
            }

            base.Update(timeElapsed);
        }

        public override void DrawLayer2(System.Drawing.Graphics graphics)
        {
            int diameter = size;
            DrawCircle(graphics, mainColour, diameter / 2);

            foreach (Entity data in allData)
            {
                data.DrawLayer2(graphics);
            }
        }

        public override void DrawLayer0(Graphics graphics)
        {
            if (drawBackground == false)
            {
                return;
            }

            int diameter = size;
            DrawCircle(graphics, backgroundColour, diameter / 2);

            foreach (Entity data in allData)
            {
                data.DrawLayer0(graphics);
            }
        }

        public override Rectangle GetBounds()
        {
            int diameter = size;
            return base.GetBounds(diameter, diameter);
        }

        public override Rectangle GetDrawBounds()
        {
            int diameter = size;
            return base.GetDrawBounds(diameter, diameter);
        }

        public void SpawnData()
        {
            SpawnData(1);
        }

        public void SpawnData(int dataToSpawn)
        {
            for (int i = 0; i < dataToSpawn; i++)
            {
                //Data data = new Data(location.X + diameter - 1, location.Y, Color.Black);
                Data data = new Data(location.X + 50, location.Y, 10, Color.Black);
                //data.heading = 30;
                data.orbiting = true;
                data.ChangeVelocity(10);

                allData.Add(data);
            }
        }

        public override bool DetectCollision(Entity other)
        {
            Rectangle bounds = GetBounds();
            int radius = bounds.Width / 2;

            Rectangle otherBounds = other.GetBounds();
            int otherRadius = otherBounds.Width / 2;

            int distance = (int)Math.Round(Program.GetDistance(location, other.location),0);
            int maxDistance = radius - otherRadius;

            if (distance < maxDistance)
            {
                return false;
            }

            return true;
        }
    }
}
