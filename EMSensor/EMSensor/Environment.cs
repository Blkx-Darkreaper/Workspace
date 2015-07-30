using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EMSensor
{
    class Environment
    {
        public List<EmissionSource> allSources { get; set; }
        public List<Emission> allEmissions { get; set; }
        public decimal propegationVelocity { get; set; }
        public bool emissionsToRemove { get; set; }

        public Environment(decimal inVelocity)
        {
            propegationVelocity = inVelocity;
            allSources = new List<EmissionSource>();
            allEmissions = new List<Emission>();
            emissionsToRemove = false;
        }

        public void AddEmissionSource(EmissionSource toAdd)
        {
            allSources.Add(toAdd);
        }

        public void Update(decimal timeElapsed)
        {
            UpdateAllSources();
            PropegateAllEmissions(timeElapsed);
            if (emissionsToRemove == false)
            {
                return;
            }
            RemoveDeadEmissions();
        }

        private void UpdateAllSources()
        {
            foreach (EmissionSource toUpdate in allSources)
            {
                allEmissions.Add(toUpdate.Update());
            }
        }

        private void PropegateAllEmissions(decimal timeElapsed)
        {
            foreach (Emission toPropegate in allEmissions)
            {
                toPropegate.Propegate(propegationVelocity, timeElapsed);
                if (emissionsToRemove == true)
                {
                    continue;
                }

                if (toPropegate.noSignal == true)
                {
                    emissionsToRemove = true;
                }
            }
        }

        public void RemoveDeadEmissions()
        {
            List<Emission> allEmissionsToCheck = allEmissions.ToList();
            foreach (Emission toCheck in allEmissionsToCheck)
            {
                if (toCheck.noSignal == false)
                {
                    continue;
                }

                allEmissions.Remove(toCheck);
            }

            emissionsToRemove = false;
        }

        public List<Emission> GetEmissionsWithinRange(Point receiver, decimal sensitivity)
        {
            List<Emission> emissionsWithinRange = new List<Emission>();

            foreach (Emission toCheck in allEmissions)
            {
                bool signalInRange = toCheck.CheckSignalInRange(receiver, sensitivity);
                if (signalInRange == false)
                {
                    continue;
                }

                emissionsWithinRange.Add(toCheck);
            }

            return emissionsWithinRange;
        }
    }
}
