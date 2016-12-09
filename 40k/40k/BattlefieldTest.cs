using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FortyK
{
    class BattlefieldTest
    {
        private Battlefield field = new Battlefield(50, 50, 10);
        private Model target = new Model("Target", Model.infantry, 1, 4);
        private Model viewer = new Model("Viewer", Model.infantry, 1, 4);
        private Grid obstruction;

        public void test()
        {
            Point targetLocation = new Point(25, 25);
            field.addModel(target, targetLocation, 0);
            Point viewerLocation = new Point(25, 5);
            field.addModel(viewer, viewerLocation, 0);

            Point obstructionLocation = new Point(25, 15);
            obstruction = new Grid(2, obstructionLocation, 0, Cover.hillCrest);
            field.addTerrain(obstruction, obstructionLocation, 0);

            bool hasLineOfSight = Battlefield.targetIsInLineOfSight(viewer, target);
        }
    }
}
