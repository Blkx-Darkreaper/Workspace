package Strikeforce;

import javax.swing.ImageIcon;
import static Strikeforce.Board.*;

public class Projectile extends Mover {
	
	protected Mover master;

	public Projectile(ImageIcon icon, int startingX, int startingY, Mover inMaster) {
		super(icon, startingX, startingY);
		dy = 6;
		master = inMaster;
	}
	
	public void outOfRange() {
		master.allProjectiles.remove(this);
		destroy();
	}
	
	public void destroy () {
		master = null;
		super.destroy();
	}
}
