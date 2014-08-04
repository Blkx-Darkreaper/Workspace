package Strikeforce;

import java.awt.Image;
import java.awt.Rectangle;
import java.awt.geom.Ellipse2D;

import javax.swing.ImageIcon;

import static Strikeforce.Global.*;

public class Entity {

	protected int centerX, centerY;
	protected Image currentImage;
	protected int halfWidth;
	protected int halfHeight;
	
	public Entity(int startX, int startY, int inWidth, int inHeight) {
		centerX = startX;
		centerY = startY;
		halfWidth = inWidth / 2;
		halfHeight = inHeight / 2;
	}
	
	public Entity(ImageIcon icon, int startX, int startY) {
		currentImage = icon.getImage();
		halfWidth = currentImage.getWidth(null) / 2;
		halfHeight = currentImage.getHeight(null) / 2;
		centerX = startX;
		centerY = startY;
	}
	
	public int getX() {
		return centerX;
	}

	public int getY() {
		return centerY;
	}
	
	public Image getImage() {
		return currentImage;
	}
	
	public int getHalfWidth() {
		return halfWidth;
	}
	
	public int getHalfHeight() {
		return halfHeight;
	}
	
	public Rectangle getBounds() {
		int cornerX = centerX - halfWidth;
		int cornerY = centerY - halfHeight;
		return new Rectangle(cornerX, cornerY, halfWidth * 2, halfHeight * 2);
	}
	
	public Circle getCircularHitBox () {
		int radius = Math.min(halfWidth * 2, halfHeight * 2);
		return new Circle(centerX, centerY, radius);
	}
	
	public boolean checkForCollision(Entity other) {
		//return other.getBounds().intersects(getBounds());
		return other.getCircularHitBox().intersects(getCircularHitBox());
	}
	
	class Circle {
		int centerX;
		int centerY;
		int radius;
		
		public Circle (int inCenterX, int inCenterY, int inRadius) {
			centerX = inCenterX;
			centerY = inCenterY;
			radius = inRadius;
		}
		
		private int getCenterX() {
			return centerX;
		}
		
		private int getCenterY() {
			return centerY;
		}
		
		private int getRadius() {
			return radius;
		}
		
		public boolean intersects(Circle other) {
			int rise = centerY - other.getCenterY();
			int run = centerX - other.getCenterX();
			int distanceBetweenFoci = (int) Math.sqrt(rise * rise + run * run);
			
			if(distanceBetweenFoci >= (radius + other.getRadius())) {
				return false;
			}
			
			return true;
		}
	}
}
