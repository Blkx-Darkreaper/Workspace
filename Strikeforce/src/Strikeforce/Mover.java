package Strikeforce;

import java.util.List;

import javax.swing.ImageIcon;

public class Mover extends Entity {
	
	protected int dx, dy;
	protected int speed;
	
	protected List<Projectile> allProjectiles;

	public Mover(ImageIcon icon) {
		super(icon);
	}
	
	public Mover(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
	}
	
	public int getAirspeed() {
		return speed;
	}
	
	public void setAirspeed(int inValue) {
		speed = inValue;
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
