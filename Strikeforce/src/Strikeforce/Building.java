package Strikeforce;

import javax.swing.ImageIcon;

public class Building extends Entity {
	
	private int hitPoints;
	protected boolean covers;
	
	public Building(ImageIcon icon, int startX, int startY, int inDirection, int inAltitude) {
		super(icon, startX, startY, inDirection, inAltitude);
		hitPoints = 1;
	}
	
	public Building(String inName, int inX, int inY, int inDirection, int inAltitude, int inHitPoints) {
		super(inName, inX, inY, inDirection, inAltitude);
		hitPoints = inHitPoints;
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
		
		return true;
	}
}
