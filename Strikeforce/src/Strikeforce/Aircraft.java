package Strikeforce;

import java.util.ArrayList;
import java.util.List;
import javax.swing.ImageIcon;
import static Strikeforce.Global.*;

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
		super.move();
		
		if(x < LOWER_BOUNDS_X) {
			x = LOWER_BOUNDS_X;
		}
		if(x > UPPER_BOUNDS_X) {
			x = UPPER_BOUNDS_X;
		}

		if(y < LOWER_BOUNDS_Y) {
			y = LOWER_BOUNDS_Y;
		}
		if(y > UPPER_BOUNDS_Y) {
			y = UPPER_BOUNDS_Y;
		}
	}
}
