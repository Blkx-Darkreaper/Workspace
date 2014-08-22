package Strikeforce;

import javax.swing.ImageIcon;

public class Building extends Entity {
	
	private int hitPoints;
	
	public Building(ImageIcon icon, int startX, int startY, int inDirection, int inAltitude) {
		super(icon, startX, startY, inDirection, inAltitude);
		hitPoints = 1;
	}
	
	public Building(String inName, int inX, int inY, int inDirection, int inAltitude, int inHitPoints) {
		super(inName, inX, inY, inDirection, inAltitude);
		hitPoints = inHitPoints;
	}
	
	public void spawn() {
		return;
	}
	
	public void takeoffsAndLandings() {
		return;
	}
}