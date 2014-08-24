package Strikeforce;

import javax.swing.ImageIcon;

import static Strikeforce.Board.*;

public class Projectile extends Mover {
	
	protected int damage;
	protected boolean hitsGround;
	protected boolean live;
	protected boolean detonate = false;

	public Projectile(ImageIcon icon, int startingX, int startingY, int inSpeed, int inDamage, int inDirection, 
			int inAltitude) {
		super(icon, startingX, startingY, inDirection, inAltitude);
		speed = inSpeed;
		damage = inDamage;
		updateVectors();
	}
	
	public Projectile(String inName, int inX, int inY, int inDirection, int inAltitude, int inSpeed, int inDamage, 
			boolean inHitsGround, boolean inLive) {
		super(inName, inX, inY, inDirection, inAltitude, inSpeed);
		damage = inDamage;
		hitsGround = inHitsGround;
		live = inLive;
	}
	
	public int getDamage() {
		return damage;
	}
	
	public boolean getHitsGround() {
		return hitsGround;
	}
	
	public boolean getLive() {
		return live;
	}
	
	public boolean getDetonate() {
		return detonate;
	}
	
	public Effect getExplosionAnimation() {
		return null;
	}
}
