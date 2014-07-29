package Strikeforce;

import java.util.List;

import javax.swing.ImageIcon;

public class Vehicle extends Mover {

	protected boolean invulnerable = false;
	
	private final int MAX_SPEED = 3;
	private final int MIN_SPEED = 1;
	
	public Vehicle(ImageIcon icon) {
		super(icon);
	}

	public Vehicle(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
	}
	
	public List<Projectile> getAllProjectiles() {
		return allProjectiles;
	}
	
	public boolean getInvulnerable() {
		return invulnerable;
	}

	public void accelerate() {
		speed++;
		
		if(speed > MAX_SPEED) {
			speed = MAX_SPEED;
		}
	}

	public void decelerate() {
		speed--;
		
		if(speed > MIN_SPEED) {
			speed = MIN_SPEED;
		}
	}
}
