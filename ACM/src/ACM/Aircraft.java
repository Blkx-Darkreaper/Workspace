package ACM;

import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

public class Aircraft extends Projectile {

	private final int MAX_AIRSPEED = 5;
	private final int STALL_SPEED = 1;
	
	private int pitch = 0;
	private int bank = 0;

	public Aircraft(ImageIcon icon) {
		super(icon);
		airspeed = 1;
		allProjectiles = new ArrayList<>();
	}
	
	public Aircraft(ImageIcon icon, int startingAirspeed) {
		super(icon);
		airspeed = startingAirspeed;
		allProjectiles = new ArrayList<>();
	}
	
	public Aircraft(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
		allProjectiles = new ArrayList<>();
	}
	
	public Aircraft(ImageIcon icon, int startingX, int startingY, int startingAirspeed) {
		super(icon, startingX, startingY);
		airspeed = startingAirspeed;
		allProjectiles = new ArrayList<>();
	}
	
	public List<Projectile> getAllProjectiles() {
		return allProjectiles;
	}
	
	@Override
	public void move() {
		if(airspeed < STALL_SPEED) {
			airspeed = STALL_SPEED;
		}
		if(airspeed > MAX_AIRSPEED) {
			airspeed = MAX_AIRSPEED;
		}
		
		super.move();

		if(y < 10) {
			y = 10;
		}
		if(y > 220) {
			y = 220;
		}
	}
	
	@Override
	public void moveVertically() {
		if(airspeed < STALL_SPEED) {
			airspeed = STALL_SPEED;
		}
		if(airspeed > MAX_AIRSPEED) {
			airspeed = MAX_AIRSPEED;
		}
		
		super.moveVertically();

		if(y < 10) {
			y = 10;
		}
		if(y > 220) {
			y = 220;
		}
	}
	
	@Override
	public void moveRelative (Projectile other) {
		if(airspeed < STALL_SPEED) {
			airspeed = STALL_SPEED;
		}
		if(airspeed > MAX_AIRSPEED) {
			airspeed = MAX_AIRSPEED;
		}
		
		super.moveRelative(other);

		if(y < 10) {
			y = 10;
		}
		if(y > 220) {
			y = 220;
		}
	}
}
