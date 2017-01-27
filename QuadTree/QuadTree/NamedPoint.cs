using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

public class NamedPoint
{
    public string Name { get; protected set; }
    public int X { get { return point.X; } protected set { point = new Point(value, this.Y); } }
    public int Y { get { return point.Y; } protected set { point = new Point(this.X, value); } }
    protected Point point { get; set; }

    public NamedPoint(string name, int x, int y)
    {
        this.Name = name;
        this.point = new Point(x, y);
    }

    public override string ToString()
    {
        string output = JsonConvert.SerializeObject(this);
        return output;
    }

    public void Draw(Graphics graphics)
    {
        Pen pen = Pens.Green;
        Rectangle bounds = new Rectangle(X - 2, Y - 2, 4, 4);

        graphics.DrawEllipse(pen, bounds);
    }
}