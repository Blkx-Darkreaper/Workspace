package Strikeforce;

import static Strikeforce.Global.resLoader;

import java.awt.Image;

import javax.swing.ImageIcon;

public class Building extends Entity {
	
	private int hitPoints;
	protected boolean covers;
	
	protected Image imageDestroyed;
	
	public Building(ImageIcon icon, int startX, int startY, int inDirection, int inAltitude) {
		super(icon, startX, startY, inDirection, inAltitude);
		hitPoints = 1;
	}
	
	public Building(String inName, int inX, int inY, int inDirection, int inAltitude, int inHitPoints) {
		super(inName, inX, inY, inDirection, inAltitude);
		hitPoints = inHitPoints;
		
		String extension = "png";
		ImageIcon icon = resLoader.getImageIcon(inName + "destroyed" + "." + extension);
		imageDestroyed = icon.getImage();
	}
	
	public boolean getCovers() {
		return covers;
	}
	
	public void setCovers(boolean condition) {
		covers = condition;
	}
	
	public void spawn() {
		return;
	}
	
	public void takeoffsAndLandings() {
		return;
	}
	
	public void dealDamage(int damageDealt) {
		hitPoints -= damageDealt;
	}
	
	public boolean criticalDamage() {
		if(hitPoints > 0) {
			return false;
		}
		
		currentImage = imageDestroyed;
		return true;
	}
}
