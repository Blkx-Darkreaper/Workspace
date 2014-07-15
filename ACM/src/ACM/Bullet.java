package ACM;

import javax.swing.ImageIcon;
import static ACM.Board.*;

public class Bullet extends Projectile {
	
	protected Projectile master;

	public Bullet(ImageIcon icon, int startingX, int startingY, Projectile inMaster) {
		super(icon, startingX, startingY);
		super.setAirspeed(6);
		master = inMaster;
	}
	
	public void outOfRange() {
		boolean outOfRange = checkOutOfRange();
		
		if(outOfRange == true) {
			master.allProjectiles.remove(this);
		}
	}
	
	public boolean checkOutOfRange () {
		if(x > SCREEN_WIDTH) {
			return true;
		}
		
		return false;
	}
	
	public void destroy () {
		master = null;
		super.destroy();
	}
}
