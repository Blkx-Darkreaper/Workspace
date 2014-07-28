package Strikeforce;

import java.util.List;

import javax.swing.ImageIcon;

public class Projectile extends Entity {
	
	protected int dx, dy;
	protected int airspeed;
	
	protected List<Projectile> allProjectiles;

	public Projectile(ImageIcon icon) {
		super(icon);
	}
	
	public Projectile(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
	}
	
	public int getAirspeed() {
		return airspeed;
	}
	
	public void setAirspeed(int inValue) {
		airspeed = inValue;
	}

	public void move() {
		x += dx;
		y += dy;
	}
	
	public void move(int panRate, int scrollRate) {
		x += dx + panRate;
		y += dy + scrollRate;
	}
}
