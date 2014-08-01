package Strikeforce;

import javax.swing.ImageIcon;
import static Strikeforce.Board.*;

public class Projectile extends Mover {
	
	protected int damage;

	public Projectile(ImageIcon icon, int startingX, int startingY, int deltaX, int deltaY, int inDamage) {
		super(icon, startingX, startingY);
		dy = deltaY;
		dx = deltaX;
		damage = inDamage;
	}
	
	public int getDamage() {
		return damage;
	}
}
