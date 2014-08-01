package Strikeforce;

import static Strikeforce.Global.resLoader;

import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

public abstract class Weapon {
	
	private String name;
	private String description;
	
	public Weapon (String inName, String inDescription) {
		name = inName;
		description = inDescription;
	}

	public List<Projectile> openFire(int originX, int originY) {
		List<Projectile> allShots = new ArrayList<>();
		
		ImageIcon bulletIcon = resLoader.getImageIcon("bullet.png");
		int startX = originX;
		int startY = originY + bulletIcon.getIconHeight() / 2;
		
		allShots.add(new Projectile(bulletIcon, startX, startY, 0, 6, 1));
		return allShots;
	}
}
