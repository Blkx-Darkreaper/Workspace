using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FortyK
{
    class Battle
    {
        private int roundNumber { get; set; }
        private int turnNumber { get; set; }
        private LinkedList<Army> turnOrder { get; set; }
        private Dictionary<Army, Point> deploymentPoints { get; set; }
        private Battlefield field { get; set; }

        public Battle(Battlefield inField)
        {
            roundNumber = 1;
            turnNumber = 1;
            field = inField;
        }

        public void addArmy(Army toAdd, Point spawn)
        {
            turnOrder.AddLast(toAdd);
            deploymentPoints.Add(toAdd, spawn);

            field.addArmy(toAdd);
        }

        public void removeArmy(Army toRemove)
        {
            turnOrder.Remove(toRemove);
            deploymentPoints.Remove(toRemove);

            field.removeArmy(toRemove);
        }

        public void round()
        {
            foreach (Army player in turnOrder)
            {
                turn(player);
            }

            roundNumber++;
        }

        public void turn(Army player)
        {
            movementPhase(player);
            shootingPhase(player);
            assaultPhase(player);
            endPhase(player);

            turnNumber++;
        }

        public void movementPhase(Army player)
        {
            throw new NotImplementedException();
        }

        public void shootingPhase(Army player)
        {
            throw new NotImplementedException();
        }

        public void assaultPhase(Army player)
        {
            throw new NotImplementedException();
        }

        public void endPhase(Army player)
        {
            throw new NotImplementedException();
        }
    }
}
