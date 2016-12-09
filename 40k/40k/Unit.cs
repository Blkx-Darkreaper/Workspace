using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FortyK
{
    class Unit
    {
        private object modifier;
        public string name { get; private set; }
        public Faction faction { get; private set; }
        public string armyName { get; set; }
        public LinkedList<Model> allUnitMembers { get; set; }
        private int startingStrength { get; set; }

        public bool hasMoved { get; set; }
        public bool hasFired { get; set; }
        public bool canCharge { get; set; }
        public bool isDelayed { get; set; }
        public bool isFallingBack { get; set; }
        public bool isPinned { get; set; }
        public bool isDestroyed { get; set; }

        public Unit(string inName, Faction inFaction)
        {
            name = inName;
            faction = inFaction;
            allUnitMembers = new LinkedList<Model>();
            startingStrength = 0;

            hasMoved = false;
            hasFired = false;
            canCharge = true;
            isDelayed = false;
            isFallingBack = false;
            isPinned = false;
            isDestroyed = false;
        }

        public Unit clone()
        {
            Unit cloneUnit = new Unit(name, faction);
            foreach (Model member in allUnitMembers)
            {
                Model cloneMember = member.clone();
                cloneUnit.addModel(cloneMember);
            }

            return cloneUnit;
        }

        public void addLeader(Model toAdd)
        {
            allUnitMembers.AddFirst(toAdd);
            startingStrength = allUnitMembers.Count;
        }

        public void addModel(Model toAdd)
        {
            allUnitMembers.AddLast(toAdd);
            startingStrength = allUnitMembers.Count;
        }

        public void removeModel(Model toRemove)
        {
            allUnitMembers.Remove(toRemove);
            startingStrength = allUnitMembers.Count;
        }

        public int getStrength()
        {
            int strength = 0;
            foreach (Model member in allUnitMembers)
            {
                string modelType = member.modelType;
                bool isMonstrous = modelType.Equals(Model.monstrousCreature);
                if (isMonstrous == true)
                {
                    strength += 10;
                    continue;
                }

                bool isVehicle = member.isVehicle;
                if (isVehicle == true)
                {
                    if (member.weaponSkill == 0)
                    {
                        continue;
                    }
                    Vehicle vehicle = (Vehicle)member;
                    int frontArmour = vehicle.frontArmour;
                    if (frontArmour < 12)
                    {
                        strength += 5;
                        continue;
                    }

                    strength += 10;
                    continue;
                }

                strength += member.wounds;
            }

            return strength;
        }

        public Point getLocation()
        {
            Model unitLeader = allUnitMembers.First();
            Point leaderLocation = unitLeader.currentLocation;

            return leaderLocation;
        }

        public void move(Point destination, decimal movement)
        {
            if (isFallingBack == true)
            {
                bool regrouped = regroupCheck();
                if (regrouped == true)
                {
                    regroup();
                    return;
                }
            }

            SortedList<decimal, Model> allDistancesMoved = new SortedList<decimal, Model>();
            foreach (Model member in allUnitMembers)
            {
                decimal distanceMoved = member.move(destination);
                allDistancesMoved.Add(distanceMoved, member);
            }
            decimal mostDistanceMoved = allDistancesMoved.First().Key;

            if (mostDistanceMoved == movement)
            {
                return;
            }

            if (isFallingBack == false)
            {
                return;
            }

            bool isTrapped = true;
            isDestroyed = true;
        }

        private void reformUnitCohesion(decimal distancePool)
        {
            throw new NotImplementedException();
        }

        private void repositionUnit(int endingDirection, decimal distancePool)
        {
            throw new NotImplementedException();
        }

        public void assault(Unit enemySquad)
        {
            if (canCharge == false)
            {
                return;
            }
            Announcer.announceUnitAction(this, Announcer.UnitActions.assault, enemySquad);

            bool massacre = false;
            Point currentLocation = getLocation();
            Point enemyLocation = enemySquad.getLocation();
            decimal distanceToTarget = Global.getDistance(currentLocation, enemyLocation);
            bool chargeSucceeds = chargeCheck(distanceToTarget);

            if (chargeSucceeds == false)
            {

                enemySquad.overwatch(this);
                throw new NotImplementedException();
            }

            bool enemyFallingBack = enemySquad.isFallingBack;
            if (enemyFallingBack == true)
            {
                bool enemyRegroups = enemySquad.canRegroupCheck();
                if (enemyRegroups == false)
                {
                    massacre = true;
                    enemySquad.isDestroyed = true;

                    consolidatePosition(massacre);
                    return;
                }

                enemySquad.regroup();
            }

            enemySquad.overwatch(this);

            move(enemyLocation, distanceToTarget);

            closeCombat(enemySquad);

            int friendlyWoundsInflicted = getAssaultWoundsInflicted();
            int enemyWoundsInflicted = enemySquad.getAssaultWoundsInflicted();
            removeCasualties();
            enemySquad.removeCasualties();

            bool friendliesVanquished = isDestroyed;
            if (friendliesVanquished == true)
            {
                enemySquad.consolidatePosition(friendliesVanquished);
                return;
            }

            bool enemyVanquished = enemySquad.isDestroyed;
            if (enemyVanquished == true)
            {
                consolidatePosition(enemyVanquished);
                return;
            }

            bool stillInMelee = !enemyVanquished;

            Unit assaultVictor = null;
            Unit assaultLoser = null;
            if (friendlyWoundsInflicted > enemyWoundsInflicted)
            {
                assaultVictor = this;
                assaultLoser = enemySquad;
            }
            if (enemyWoundsInflicted > friendlyWoundsInflicted)
            {
                assaultVictor = enemySquad;
                assaultLoser = this;
            }
            if (friendlyWoundsInflicted == enemyWoundsInflicted)
            {
                return;
            }
            int modifier = 0;
            bool isBelowHalfStrength = assaultLoser.belowHalfStrengthCheck();
            if (isBelowHalfStrength == true)
            {
                modifier = -1;
            }

            int outnumberedModifier = assaultLoser.getOutnumberedModifier(assaultVictor);
            modifier += outnumberedModifier;

            bool loserStandsGround = assaultLoser.moraleCheck(modifier);
            if (loserStandsGround == true)
            {
                return;
            }

            massacre = assaultVictor.sweepingAdvanceCheck(assaultLoser);
            if (massacre == true)
            {
                stillInMelee = false;
                enemySquad.isDestroyed = true;
            }
            else
            {
                assaultLoser.fallBack();
            }

            assaultVictor.consolidatePosition(massacre);

            if (stillInMelee == false)
            {
                return;
            }

            pileIn(enemyLocation);
            enemySquad.pileIn(currentLocation);
        }

        private int getOutnumberedModifier(Unit enemySquad)
        {
            int friendlyStrength = getStrength();
            int enemyStrength = enemySquad.getStrength();

            if (enemyStrength >= 4 * friendlyStrength)
            {
                return -4;
            }
            if (enemyStrength > 3 * friendlyStrength)
            {
                return -3;
            }
            if (enemyStrength > 2 * friendlyStrength)
            {
                return -2;
            }
            if (enemyStrength > friendlyStrength)
            {
                return -1;
            }

            return 0;
        }

        private void disengage()
        {
            if (isDestroyed == true)
            {
                return;
            }

            foreach (Model member in allUnitMembers)
            {
                member.isEngaged = false;
            }
        }

        private void fallBack()
        {
            disengage();
            isFallingBack = true;

            decimal fallBackMovement = Global.rollDice(2);
            Point fallBackPoint = getFallBackPoint();

            move(fallBackPoint, fallBackMovement);
        }

        private Point getFallBackPoint()
        {
            Point fallBackPoint = new Point();

            return fallBackPoint;
        }

        private bool canRegroupCheck()
        {
            int currentStrength = allUnitMembers.Count;
            decimal strengthPercentage = currentStrength / startingStrength;
            if (strengthPercentage < 0.5m)
            {
                return false;
            }

            Point currentLocation = getLocation();
            List<Unit> nearbyEnemies = Battlefield.getAllEnemySquadsInRange(currentLocation, 6, faction);
            if (nearbyEnemies.Count > 0)
            {
                return false;
            }

            bool hasCoherency = cohesionCheck();
            if (hasCoherency == false)
            {
                return false;
            }

            return true;
        }

        private bool cohesionCheck()
        {
            throw new NotImplementedException();
        }

        private bool regroupCheck()
        {
            bool canRegroup = canRegroupCheck();
            if (canRegroup == false)
            {
                return false;
            }

            int modifier = 0;
            Point currentLocation = getLocation();
            List<Unit> enemiesInSight = Battlefield.getEnemySquadsInSight(currentLocation, faction);
            if (enemiesInSight.Count == 0)
            {
                modifier += 1;
            }

            bool testPassed = leadershipCheck(modifier);
            if (testPassed == false)
            {
                return false;
            }

            return true;
        }

        private void regroup()
        {
            isFallingBack = false;
            hasMoved = true;
        }

        private void pileIn(Point enemyLocation)
        {
            if (allUnitMembers.Count == 0)
            {
                return;
            }

            List<Model> unengagedMembers = new List<Model>();
            foreach (Model member in allUnitMembers)
            {
                bool isEngaged = member.isEngaged;
                if (isEngaged == true)
                {
                    continue;
                }

                Point currentLocation = member.currentLocation;
                decimal distanceToEnemy = Global.getDistance(currentLocation, enemyLocation);
                decimal distanceToMove = 6;
                if (distanceToEnemy < distanceToMove)
                {
                    distanceToMove = distanceToEnemy;
                }

                member.move(enemyLocation, distanceToMove);
            }
        }

        private bool chargeCheck(decimal distanceToTarget)
        {
            int roll = Global.rollDice(2);

            if (roll < distanceToTarget)
            {
                return false;
            }

            return true;
        }

        private void closeCombat(Unit enemySquad)
        {
            SortedList<int, Tuple<Model, Model>> closeCombatants = new SortedList<int, Tuple<Model, Model>>();

            foreach (Model member in allUnitMembers)
            {
                Model enemyCombatant = member.chooseEnemy(enemySquad);

                bool isEngaged = member.meleeRangeCheck(enemyCombatant, this);
                member.isEngaged = isEngaged;
                enemyCombatant.isEngaged = isEngaged;
                if (isEngaged == false)
                {
                    continue;
                }

                int memberInitiative = member.initiative;
                bool inCover = member.isInCoverCheck();
                if (inCover == true)
                {
                    memberInitiative = 10;
                }

                Tuple<Model, Model> combatantPair = new Tuple<Model, Model>(member, enemyCombatant);
                closeCombatants.Add(memberInitiative, combatantPair);

                combatantPair = new Tuple<Model, Model>(enemyCombatant, member);
                int enemyInitiative = enemyCombatant.initiative;
                inCover = enemyCombatant.isInCoverCheck();
                if (inCover == true)
                {
                    enemyInitiative = 10;
                }

                closeCombatants.Add(enemyInitiative, combatantPair);
            }

            foreach (Tuple<Model, Model> combatantPair in closeCombatants.Values)
            {
                Model attacker = combatantPair.Item1;
                Model defender = combatantPair.Item2;

                attacker.meleeCombat(defender, this);
            }
        }

        private int getAssaultWoundsInflicted()
        {
            int totalWoundsInflicted = 0;

            foreach (Model member in allUnitMembers)
            {
                totalWoundsInflicted += member.assaultWoundsInflicted;
                member.assaultWoundsInflicted = 0;
            }

            return totalWoundsInflicted;
        }

        private bool sweepingAdvanceCheck(Unit retreatingEnemy)
        {
            int initiativeCharacteristic = getInitiative();
            int roll = Global.rollDice(1);
            initiativeCharacteristic += roll;

            int enemyInitiativeCharacteristic = retreatingEnemy.getInitiative();
            roll = Global.rollDice(1);
            enemyInitiativeCharacteristic += roll;

            if (enemyInitiativeCharacteristic > initiativeCharacteristic)
            {
                return false;
            }

            return true;
        }

        private int getInitiative()
        {
            List<int> allInitiatives = new List<int>();
            int lowestInitiative = allUnitMembers.First().initiative;
            foreach (Model member in allUnitMembers)
            {
                int initiative = member.initiative;
                allInitiatives.Add(initiative);
                if (initiative < lowestInitiative)
                {
                    lowestInitiative = initiative;
                }
            }

            var sortedInitiatives = allInitiatives.GroupBy(item => item).OrderByDescending(g => g.Count()).Select(g => g.Key);
            int majorityInitiative = sortedInitiatives.ElementAt(0);
            int secondMajorityInitiative = sortedInitiatives.ElementAt(1);

            if (majorityInitiative == secondMajorityInitiative)
            {
                return lowestInitiative;
            }

            return majorityInitiative;
        }

        private void consolidatePosition(bool enemyVanquished)
        {
            Point currentLocation = getLocation();
            Unit nearestEnemySquad = Battlefield.getNearestEnemySquad(currentLocation, faction);
            Point nearestEnemyLocation = nearestEnemySquad.getLocation();

            int nearestEnemyDirection = Global.getDirection(currentLocation, nearestEnemyLocation);

            if (enemyVanquished == true)
            {
                repositionUnit(nearestEnemyDirection, 6);
                return;
            }

            reformUnitCohesion(3);
        }

        private void rangedCombat(Unit enemySquad)
        {
            foreach (Model member in allUnitMembers)
            {
                Model enemy = member.chooseEnemy(enemySquad);
                member.normalRangedAttack(enemy);
            }

            enemySquad.removeCasualties();
            bool checkPassed = enemySquad.shootingCasualtiesCheck();
            if (checkPassed == true)
            {
                return;
            }

            int modifier = 0;
            bool isBelowHalfStrength = enemySquad.belowHalfStrengthCheck();
            if (isBelowHalfStrength == true)
            {
                modifier = -1;
            }

            checkPassed = enemySquad.moraleCheck(modifier);
            if (checkPassed == true)
            {
                return;
            }

            enemySquad.fallBack();
        }

        private void overwatch(Unit chargingUnit)
        {
            foreach (Model member in allUnitMembers)
            {
                Model enemy = member.chooseEnemy(chargingUnit);
                member.overwatch(enemy);
            }
        }

        private void removeCasualties()
        {
            List<Model> allMembers = new List<Model>(allUnitMembers);
            foreach (Model member in allMembers)
            {
                bool isAlive = member.isAliveCheck();
                if (isAlive == true)
                {
                    continue;
                }

                allUnitMembers.Remove(member);
            }
        }

        private bool massacreCheck()
        {
            if (allUnitMembers.Count == 0)
            {
                return true;
            }

            return false;
        }

        private bool lastManStandingCheck()
        {
            if (allUnitMembers.Count > 1)
            {
                return false;
            }

            if (startingStrength == 1)
            {
                return false;
            }

            return true;
        }

        public bool shootingCasualtiesCheck()
        {
            int unitMembers = allUnitMembers.Count;
            int casualties = 0;
            foreach (Model member in allUnitMembers)
            {
                bool isAlive = member.isAliveCheck();
                if (isAlive == true)
                {
                    continue;
                }
                casualties++;
            }

            decimal lossesPercentage = casualties / unitMembers;
            if (lossesPercentage < 0.25m)
            {
                return false;
            }

            return true;
        }

        public void setUnitCover()
        {
            throw new NotImplementedException();
        }

        private bool unitInCover()
        {
            foreach (Model member in allUnitMembers)
            {
                string coverType = member.coverType;
                bool notInCover = coverType.Equals(Cover.clear);
                if (notInCover == true)
                {
                    return false;
                }
            }
            return true;
        }

        private bool belowHalfStrengthCheck()
        {
            int currentStrength = allUnitMembers.Count;

            decimal strengthPercentage = currentStrength / startingStrength;
            if (strengthPercentage < 0.5m)
            {
                return true;
            }

            return false;
        }

        private bool moraleCheck(int modifier)
        {
            int successes = 0;
            foreach (Model member in allUnitMembers)
            {
                bool checkPassed = member.moraleCheck(modifier);
                if (checkPassed == false)
                {
                    continue;
                }

                successes++;
            }

            int unitMembers = allUnitMembers.Count;
            decimal successPercentage = successes / unitMembers;
            if (successPercentage < 0.5m)   // To be Updated
            {
                return false;
            }

            return true;
        }

        private bool leadershipCheck(int modifier)
        {
            throw new NotImplementedException();
        }

        public void deploy(Point deploymentLocation)
        {
            throw new NotImplementedException();
        }

        public bool evacuateCheck()
        {
            foreach (Model member in allUnitMembers)
            {
                bool evactuating = member.isEvacuating;
                if (evactuating == false)
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
