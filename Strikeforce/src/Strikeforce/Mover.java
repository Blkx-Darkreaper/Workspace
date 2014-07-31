package Strikeforce;

import static Strikeforce.Board.*;
import java.util.List;

import javax.swing.ImageIcon;

public class Mover extends Entity {
	
	protected int dx, dy;
	protected int speed;
	
	protected List<Projectile> allProjectiles;
	
	public Mover(int startX, int startY, int inWidth, int inHeight) {
		super(startX, startY, inWidth, inHeight);
	}
	
	public Mover(ImageIcon icon, int startX, int startY) {
		super(icon, startX, startY);
	}
	
	public int getDeltaX() {
		return dx;
	}
	
	public void setDeltaX(int inValue) {
		dx = inValue;
	}
	
	public int getSpeed() {
		return speed;
	}
	
	public void setSpeed(int inValue) {
		speed = inValue;
		dy = speed;
	}

	public void move() {
		centerX += dx;
		centerY += dy;
		
		int lowerBoundsX = 0 + halfWidth;
		if(centerX < lowerBoundsX) {
			centerX = lowerBoundsX;
		}
		int upperBoundsX = BACKGROUND_WIDTH - halfWidth;
		if(centerX > upperBoundsX) {
			centerX = upperBoundsX;
		}

		int lowerBoundsY = 0 + halfHeight;
		if(centerY < lowerBoundsY) {
			centerY = lowerBoundsY;
		}
	}
}
