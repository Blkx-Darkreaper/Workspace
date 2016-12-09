using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FortyK
{
    class Model
    {
        public string name { get; set; }

        public string modelType { get; private set; }
        public bool isVehicle { get; private set; }
        public const string infantry = "Infantry";
        public const string bike = "Bike";
        public const string jetbike = "Jetbike";
        public const string monstrousCreature = "Monstrous creature";
        public const string jumpInfantry = "Jump infantry";
        public const string artillery = "Artillery";
        public const string beast = "Beasts & cavalry";
        public const string walker = "Walker";
        public const string skimmer = "Skimmer";
        public const string other = "Other vehicle";

        public int wounds { get; private set; }
        private int strength;
        private int toughness;
        public int initiative { get; private set; }
        private int attacks;    // only used for close combat
        private int leadership;
        public int weaponSkill { get; private set; }    //melee weapons
        private int ballisticSkill; //ranged weapons
        private int armourValue;
        private int invulnerableSave;

        public Weapon melee { get; private set; }
        public RangedWeapon ranged { get; private set; }

        private bool isSlowed;
        public bool isEngaged { get; set; }
        public bool isEvacuating { get; set; }

        public string coverType { get; set; }
        public Point currentLocation { get; set; }
        public int currentElevation { get; set; }
        public decimal baseRadius { get; private set; }

        public int assaultWoundsInflicted { get; set; }

        public Model(string inName, string inType, int inWounds, int inArmour, int inInvulnerable = 0)
        {
            name = inName;
            modelType = inType;
            setType(inType);
            wounds = inWounds;
            armourValue = inArmour;
            invulnerableSave = inInvulnerable;

            coverType = string.Empty;

            isSlowed = false;
            isEngaged = false;
            isEvacuating = false;
        }

        public Model(string inName, string inType, int inWounds, int inStrength, int inToughness, int inInitiative, int inAttacks, 
            int inLeadership, int inWeaponSkill, int inBallistic, int inArmour, int inInvulnerable = 0) : this(inName, inType, inWounds, inArmour, inInvulnerable)
        {
            strength = inStrength;
            toughness = inToughness;
            initiative = inInitiative;
            attacks = inAttacks;
            leadership = inLeadership;
            weaponSkill = inWeaponSkill;
            ballisticSkill = inBallistic;
        }

        public Model(string inName, string inType, int inWounds, int inStrength, int inToughness, int inInitiative, int inAttacks,
            int inLeadership, int inWeaponSkill, int inBallistic, int inArmour, decimal inBaseRadius, int inInvulnerable = 0)
            : this(inName, inType, inWounds,
                inStrength, inToughness, inInitiative, inAttacks, inLeadership, inWeaponSkill, inBallistic, inArmour, inInvulnerable)
        {
            baseRadius = inBaseRadius;
        }

        public Model clone()
        {
            return new Model(name, modelType, wounds, strength, toughness, initiative, attacks, leadership, weaponSkill, ballisticSkill, armourValue, baseRadius, invulnerableSave);
        }

        private void setType(string modelType)
        {
            isVehicle = false;

            switch (modelType)
            {
                case infantry:
                    baseRadius = Global.Ranges.infantryBaseRadius;
                    break;

                case walker:
                case skimmer:
                case other:
                    isVehicle = true;
                    break;

            }
        }

        public void armMelee(Weapon toArm)
        {
            melee = toArm;
        }

        public void disarmMelee()
        {
            Weapon weapon = melee;
            melee = null;
        }

        public void armRanged(RangedWeapon toArm)
        {
            ranged = toArm;
        }

        public void disarmRanged()
        {
            RangedWeapon weapon = ranged;
            ranged = null;
        }

        public bool isAliveCheck()
        {
            if (wounds <= 0)
            {
                return false;
            }

            return true;
        }

        public bool isInCoverCheck()
        {
            bool inCover = !coverType.Equals(string.Empty);

            return inCover;
        }

        public decimal move(Point destination, decimal movement = Global.Ranges.footMovement)
        {
            movement = difficultTerrainCheck(movement, currentLocation, destination);
            decimal distanceMoved;
            for (distanceMoved = 0; distanceMoved <= movement; distanceMoved += 0.25m)
            {
                throw new NotImplementedException();
            }

            string addon = string.Format(" {0} inches", distanceMoved);
            Announcer.announceModelAction(this, Announcer.ModelActions.move + addon);

            return distanceMoved;
        }

        private decimal difficultTerrainCheck(decimal movement, Point origin, Point destination)
        {
            switch (modelType)
            {
                case infantry:
                case monstrousCreature:
                case artillery:
                case beast:
                case walker:
                    isSlowed = true;
                    break;

                default:
                    break;
            }

            if (isSlowed == false)
            {
                return movement;
            }

            bool passesThroughDifficultTerrain = passesThroughDifficultTerrainCheck(origin, destination);
            if (passesThroughDifficultTerrain == false)
            {
                return movement;
            }

            movement = getDifficultTerrainMovement();
            return movement;
        }

        private bool passesThroughDifficultTerrainCheck(Point origin, Point destination)
        {
            throw new NotImplementedException();
        }

        private int getDifficultTerrainMovement()
        {
            int movement = Global.rollDiceKeepHighest(2);
            return movement;
        }

        private bool dangerousTerrainTestRequired(bool startedEndedInTerrain)
        {
            switch (modelType)
            {
                case bike:
                case other:
                    return true;

                case artillery:
                    return true;

                case jetbike:
                case jumpInfantry:
                case skimmer:
                    return startedEndedInTerrain;

                default:
                    break;
            }

            return false;
        }

        public Model chooseEnemy(List<Model> nearbyEnemies)
        {
            throw new NotImplementedException();
        }

        public Model chooseEnemy(Unit enemySquad)
        {
            List<Model> nearbyEnemies = new List<Model>(enemySquad.allUnitMembers);
            return chooseEnemy(nearbyEnemies);
        }

        public void meleeCombat(Model enemy, Unit squad)
        {
            assaultWoundsInflicted = 0;

            bool isStillAlive = isAliveCheck();
            if (isStillAlive == false)
            {
                return;
            }

            Announcer.announceModelAction(this, Announcer.ModelActions.melee, enemy);

            int targetWeaponSkill = enemy.weaponSkill;
            int meleeToHitScore = Global.getAssaultToHitScore(weaponSkill, targetWeaponSkill);

            int hits = rollForMeleeHits(attacks, meleeToHitScore);

            string addon = string.Format(" {0} hits", hits);
            Announcer.announceAction(Announcer.GeneralActions.score + addon);

            if (hits == 0)
            {
                return;
            }

            int armourPiercingRating = melee.armourPiercingRating;
            int unsavedHits = enemy.rollSaves(hits, armourPiercingRating, rangedCombat : false);

            addon = string.Format(" to avoid {0} hits", unsavedHits);
            Announcer.announceModelAction(enemy, Announcer.GeneralActions.fail + addon);

            int weaponStrength = melee.getWeaponStrength(strength);

            assaultWoundsInflicted = enemy.inflictWounds(unsavedHits, weaponStrength);
            Announcer.announceModelAction(this, Announcer.ModelActions.wound, enemy);
        }

        public bool meleeRangeCheck(Model target, Unit squad)
        {
            Point targetLocation = target.currentLocation;
            decimal targetBaseRadius = target.baseRadius;

            decimal distance = Global.getDistance(currentLocation, targetLocation);
            decimal meleeRange = targetBaseRadius + baseRadius;

            if (distance > meleeRange)
            {
                LinkedList<Model> allSquadMates = squad.allUnitMembers;
                foreach (Model squadMate in allSquadMates)
                {
                    bool squadMateEngaged = squadMate.isEngaged;
                    if (squadMateEngaged == false)
                    {
                        continue;
                    }

                    Point squadMateLocation = squadMate.currentLocation;
                    distance = Global.getDistance(currentLocation, squadMateLocation);
                    if (distance > Global.Ranges.coherency)
                    {
                        continue;
                    }

                    return true;
                }

                return false;
            }

            return true;
        }

        private int rollForMeleeHits(int attacksPerformed, int toHitScore)
        {
            int hits = 0;
            for (int i = 0; i < attacksPerformed; i++)
            {
                int roll = Global.rollDice(1);

                if (roll < toHitScore)
                {
                    continue;
                }

                hits++;
            }
            return hits;
        }

        public void normalRangedAttack(Model target)
        {
            rangedAttack(target, ballisticSkill);
        }

        public void overwatch(Model target)
        {
            rangedAttack(target, 1);
        }

        private void rangedAttack(Model target, int ballisticSkillToUse)
        {
            int shots = ranged.shots;
            int hits = rollForRangedHits(shots, ballisticSkillToUse);
            if (hits == 0)
            {
                return;
            }

            int armourPiercingRating = ranged.armourPiercingRating;
            int unsavedHits = target.rollSaves(hits, armourPiercingRating);

            int weaponStrength = ranged.strength;
            target.inflictWounds(unsavedHits, weaponStrength);
        }

        public bool moraleCheck()
        {
            return moraleCheck(0);
        }

        public bool moraleCheck(int modifier)
        {
            int roll = Global.rollDice(2);
            if (roll == 2)
            {
                return true;
            }

            if (roll > leadership)
            {
                return false;
            }

            return true;
        }

        private int inflictWounds(int hits, int weaponStrength)
        {
            int woundsInflicted = 0;

            for (int i = 0; i < hits; i++)
            {
                bool instantDeath = instantDeathCheck(weaponStrength);
                if (instantDeath == true)
                {
                    woundsInflicted += wounds;
                    wounds = 0;
                    return woundsInflicted;
                }

                int roll = Global.rollDice(1);

                int toWoundScore = Global.getToWoundScore(weaponStrength, toughness);
                if (roll < toWoundScore)
                {
                    continue;
                }

                woundsInflicted++;
                wounds--;
            }

            return woundsInflicted;
        }

        private bool instantDeathCheck(int weaponStrength)
        {
            decimal damageFactor = weaponStrength / toughness;

            if (damageFactor < 2)
            {
                return false;
            }

            return true;
        }

        private int rollSaves(int hits, int apRating, bool rangedCombat = true)
        {
            int unsavedHits = hits;
            for (int i = 0; i < hits; i++)
            {
                bool saveSuccess = rollArmourSave(apRating, rangedCombat);
                if (saveSuccess == false)
                {
                    continue;
                }

                unsavedHits--;
            }

            return unsavedHits;
        }

        private bool rollArmourSave(int apRating, bool rangedCombat)
        {
            int roll = Global.rollDice(1);

            if (apRating >= armourValue)
            {
                if (rangedCombat == false)
                {
                    return rollInvulnerableSave(roll);
                }

                bool coverSave = rollCoverSave(roll);
                if (coverSave == true)
                {
                    return coverSave;
                }

                return rollInvulnerableSave(roll);
            }

            if (roll < armourValue)
            {
                return rollInvulnerableSave(roll);
            }

            return true;
        }

        private bool rollInvulnerableSave(int roll)
        {
            if (invulnerableSave == 0)
            {
                return false;
            }

            if (roll < invulnerableSave)
            {
                return false;
            }

            return true;
        }

        private bool rollCoverSave(int roll)
        {
            int coverSave = Cover.getCoverSave(coverType);
            if (coverSave == 0)
            {
                return false;
            }

            if (roll < coverSave)
            {
                return false;
            }

            return true;
        }

        private int rollForRangedHits(int shotsFired, int ballisticSkillToUse)
        {
            int hits = 0;
            for (int i = 0; i < shotsFired; i++)
            {
                int roll = Global.rollDice(1);
                int toHitScore = 7 - ballisticSkillToUse;

                if (roll == 1)
                {
                    if (ballisticSkillToUse <= 6)
                    {
                        return hits;
                    }

                    hits += rerollForRangedHit();
                }

                if (roll < toHitScore)
                {
                    return hits;
                }

                hits++;
            }
            return hits;
        }

        private int rerollForRangedHit()
        {
            int hits = 0;
            int reroll = Global.rollDice(1);

            if (reroll == 1)
            {
                return hits;
            }

            int toHitScore = 7 - (ballisticSkill - 5);

            if (reroll < toHitScore)
            {
                return hits;
            }

            hits++;
            return hits;
        }
    }
}
