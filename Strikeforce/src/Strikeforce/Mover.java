package Strikeforce;

import static Strikeforce.Board.*;

import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

public class Mover extends Entity {
	
	protected int dx, dy;
	protected int direction = 0;
	protected int speed = 0;
	
	protected List<Projectile> allProjectiles;
	
	public Mover(int startX, int startY, int inWidth, int inHeight) {
		super(startX, startY, inWidth, inHeight);
		allProjectiles = new ArrayList<>();
	}
	
	public Mover(ImageIcon icon, int startX, int startY) {
		super(icon, startX, startY);
		allProjectiles = new ArrayList<>();
	}
	
	public int getDeltaX() {
		return dx;
	}
	
	public void setDeltaX(int inValue) {
		dx = inValue;
	}
	
	public int getDeltaY() {
		return dy;
	}
	
	public void updateVectors() {
		dx = (int) (speed * Math.sin(Math.toRadians(direction)));
		dy = (int) (speed * Math.cos(Math.toRadians(direction)));
	}
	
	public int getDirection() {
		return direction;
	}
	
	public void setDirection(int inDirection) {
		if(inDirection < 0) {
			return;
		}
		
		if(inDirection > 360) {
			return;
		}
		
		direction = inDirection;
		updateVectors();
	}
	
	public int getSpeed() {
		return speed;
	}
	
	public void setSpeed(int inValue) {
		speed = inValue;
		updateVectors();
	}
	
	public List<Projectile> getAllProjectiles() {
		return allProjectiles;
	}

	public void move() {
		centerX += dx;
		centerY += dy;
		
		int lowerBoundsX = 0 + halfWidth;
		if(centerX < lowerBoundsX) {
			centerX = lowerBoundsX;
		}
		int upperBoundsX = currentLevel.getWidth() - halfWidth;
		if(centerX > upperBoundsX) {
			centerX = upperBoundsX;
		}

		int lowerBoundsY = 0 + halfHeight;
		if(centerY < lowerBoundsY) {
			centerY = lowerBoundsY;
		}
	}
}
