package Strikeforce;

import javax.swing.ImageIcon;

import static Strikeforce.Board.*;

public class Projectile extends Mover {
	
	protected int damage;

	public Projectile(ImageIcon icon, int startingX, int startingY, int inSpeed, int inDamage, int inDirection, int inAltitude) {
		super(icon, startingX, startingY, inDirection, inAltitude);
		speed = inSpeed;
		damage = inDamage;
		updateVectors();
	}
	
	public Projectile(String inName, int inX, int inY, int inDirection, int inAltitude, int inSpeed, int inDamage) {
		super(inName, inX, inY, inDirection, inAltitude, inSpeed);
		damage = inDamage;
	}
	
	public int getDamage() {
		return damage;
	}
}
