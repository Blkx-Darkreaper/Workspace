package Strikeforce;

import static Strikeforce.Global.resLoader;

import java.util.List;

import javax.swing.ImageIcon;

public class Vehicle extends Mover {

	protected int hitPoints;
	
	protected int firingDirection = 0;
	protected Entity turret;
	protected List<Weapon> weaponSetA;
	protected List<Weapon> weaponSetB;
	
	protected boolean invulnerable = false;
	
	private final int MAX_SPEED = 3;
	private final int MIN_SPEED = 1;

	public Vehicle(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
		hitPoints = 1;
	}
	
	public int getHitPoints() {
		return hitPoints;
	}
	
	public int getFiringDirection() {
		return firingDirection;
	}
	
	public void setFiringDirection(int inDirection) {
		firingDirection = inDirection;
	}
	
	public Entity getTurret() {
		if(turret == null) {
			return null;
		}
		
		turret.setDirection(firingDirection);
		return turret;
	}
	
	public void setTurret(Entity turretToAdd) {
		turret = turretToAdd;
	}

	public void setWeaponSetA (List<Weapon> allWeaponsToAdd) {
		weaponSetA = allWeaponsToAdd;
	}
	
	public void setWeaponSetB (List<Weapon> allWeaponsToAdd) {
		weaponSetB = allWeaponsToAdd;
	}
	
	public boolean getInvulnerable() {
		return invulnerable;
	}

	public void accelerate() {
		speed++;
		
		if(speed > MAX_SPEED) {
			speed = MAX_SPEED;
		}
		
		updateVectors();
	}

	public void decelerate() {
		speed--;
		
		if(speed > MIN_SPEED) {
			speed = MIN_SPEED;
		}
		
		updateVectors();
	}
	
	public void stop() {
		speed = 0;
		updateVectors();
	}
	
	public void turnLeft() {
		direction--;
		updateVectors();
	}
	
	public void turnRight() {
		direction++;
		updateVectors();
	}
	
	public void rotateTurretLeft() {
		firingDirection--;
	}
	
	public void rotateTurretRight() {
		firingDirection++;
	}
	
	public void fireWeaponSetA() {
		if(weaponSetA.isEmpty() == true) {
			return;
		}
		
		int originX = getCenterX();
		int originY = getCenterY();
		for(Weapon aWeapon : weaponSetA) {
			List<Projectile> firedShots = aWeapon.openFire(originX, originY, firingDirection);
			allProjectiles.addAll(firedShots);
		}
	}
	
	public void fireWeaponSetB() {
		if(weaponSetB.isEmpty() == true) {
			return;
		}
		
		int originX = getCenterX();
		int originY = getCenterY();
		for(Weapon aWeapon : weaponSetB) {
			List<Projectile> firedShots = aWeapon.openFire(originX, originY, firingDirection);
			allProjectiles.addAll(firedShots);
		}
	}
	
	public void dealDamage(int damageDealt) {
		hitPoints -= damageDealt;
	}
	
	public boolean criticalDamage() {
		if(hitPoints > 0) {
			return false;
		}
		
		return true;
	}
}
