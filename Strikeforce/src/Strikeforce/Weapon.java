package Strikeforce;

import static Strikeforce.Global.resLoader;

import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

public abstract class Weapon {
	
	private String name;
	private String description;
	
	protected int muzzleVelocity;
	protected int damage;
	
	public Weapon (String inName, String inDescription, int inMuzzleVelocity, int inDamage) {
		name = inName;
		description = inDescription;
		muzzleVelocity = inMuzzleVelocity;
		damage = inDamage;
	}

	public List<Projectile> openFire(int originX, int originY, int direction) {
		List<Projectile> allShots = new ArrayList<>();
		
		ImageIcon bulletIcon = resLoader.getImageIcon("bullet.png");
		int startX = originX;
		int startY = originY + bulletIcon.getIconHeight() / 2;
		
		int dx = (int) (muzzleVelocity * Math.sin(Math.toRadians(direction)));
		int dy = (int) (muzzleVelocity * Math.cos(Math.toRadians(direction)));
		
		allShots.add(new Projectile(bulletIcon, startX, startY, dx, dy, damage));
		return allShots;
	}
}
