package Strikeforce;

import static Strikeforce.Global.resLoader;

import java.awt.Image;
import java.awt.image.BufferedImage;

import javax.swing.ImageIcon;

public class Building extends Entity {
	
	private int hitPoints;
	protected boolean covers;
	
	protected BufferedImage imageDestroyed;
	
	public Building(String inName, int inX, int inY, int inDirection, int inAltitude, int inHitPoints) {
		super(inName, inX, inY, inDirection, inAltitude);
		hitPoints = inHitPoints;
		
		String extension = "png";
		imageDestroyed = loadImage(inName + "destroyed" + "." + extension);
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
