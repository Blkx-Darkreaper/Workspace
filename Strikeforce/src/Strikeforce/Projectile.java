package Strikeforce;

import javax.swing.ImageIcon;
import static Strikeforce.Board.*;

public class Projectile extends Mover {

	public Projectile(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
		dy = 6;
	}
}