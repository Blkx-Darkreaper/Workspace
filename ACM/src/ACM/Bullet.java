package ACM;

import javax.swing.ImageIcon;
import static ACM.Board.*;

public class Bullet extends Projectile {
	
	private Aircraft owner;

	public Bullet(ImageIcon icon, int startingX, int startingY, Aircraft inOwner) {
		super(icon, startingX, startingY);
		super.setAirspeed(6);
		owner = inOwner;
	}
	
	public void outOfRange() {
		boolean outOfRange = checkOutOfRange();
		
		if(outOfRange == true) {
			owner.allProjectiles.remove(this);
		}
	}
	
	public boolean checkOutOfRange () {
		if(x > SCREEN_WIDTH) {
			return true;
		}
		
		return false;
	}
}
