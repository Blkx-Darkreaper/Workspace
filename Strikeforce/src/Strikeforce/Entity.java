package Strikeforce;

import java.awt.Image;
import java.awt.Rectangle;
import javax.swing.ImageIcon;
import static Strikeforce.Global.*;

public class Entity {

	protected String name;
	protected int centerX, centerY;
	protected int direction = 0; // degrees
	protected int altitude = 0;
	protected Image currentImage;
	
	public Entity(int startX, int startY, int inWidth, int inHeight) {
		centerX = startX;
		centerY = startY;
	}
	
	public Entity(ImageIcon icon, int startX, int startY, int inDirection, int inAltitude) {
		currentImage = icon.getImage();
		centerX = startX;
		centerY = startY;
		direction = inDirection % 360;
		altitude = inAltitude;
	}
	
	public Entity(String inName, int inX, int inY, int inDirection, int inAltitude) {
		name = inName;
		String extension = "png";
		ImageIcon icon = resLoader.getImageIcon(name + "." + extension);
		currentImage = icon.getImage();
		centerX = inX;
		centerY = inY;
		direction = inDirection % 360;
		altitude = inAltitude;
	}
	
	public int getCenterX() {
		return centerX;
	}

	public int getCenterY() {
		return centerY;
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
	}
	
	public int getAltitude() {
		return altitude;
	}
	
	public void setAltitude(int inAltitude) {
		altitude = inAltitude;
		
		if(altitude > MAX_ALTITUDE_SKY) {
			altitude = MAX_ALTITUDE_SKY;
		}
	}
	
	public Image getImage() {
		return currentImage;
	}
	
	public Rectangle getBounds() {
		int width = currentImage.getWidth(null);
		int height = currentImage.getHeight(null);
		int cornerX = centerX - width / 2;
		int cornerY = centerY - height / 2;
		return new Rectangle(cornerX, cornerY, width, height);
	}
	
	public Circle getCircularHitBox() {
		int width = currentImage.getWidth(null);
		int height = currentImage.getHeight(null);
		
		int radius = Math.min(width, height);
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
