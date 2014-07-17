package Strikeforce;

import javax.swing.ImageIcon;
import static Strikeforce.Board.*;

public class Bullet extends Projectile {
	
	protected Projectile master;

	public Bullet(ImageIcon icon, int startingX, int startingY, Projectile inMaster) {
		super(icon, startingX, startingY);
		dy = 6;
		master = inMaster;
	}
	
	public void outOfRange() {
		boolean outOfRange = checkOutOfRange();
		
		if(outOfRange == true) {
			master.allProjectiles.remove(this);
		}
	}
	
	public boolean checkOutOfRange () {
		if(y > SCREEN_HEIGHT) {
			return true;
		}
		
		return false;
	}
	
	public void destroy () {
		master = null;
		super.destroy();
	}
}
