package Strikeforce;

import java.awt.Rectangle;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;
import static Strikeforce.Global.*;

public class Entity {

	protected String name;
	protected int centerX, centerY;
	protected int direction = 0; // degrees
	protected int altitude = 0;
	protected BufferedImage currentImage;
	protected boolean isSelected;
	protected int opacity = 100;
	protected boolean hasShadow;
	
	public Entity(int startX, int startY, int inWidth, int inHeight) {
		centerX = startX;
		centerY = startY;
	}
	
	public Entity(BufferedImage inImage, int inX, int inY) {
		currentImage = inImage;
		centerX = inX;
		centerY = inY;
		hasShadow = false;
	}
	
	public Entity(String inName, int inX, int inY, int inDirection, int inAltitude) {
		name = inName;
		String extension = "png";
		//ImageIcon icon = resLoader.getImageIcon(name + "." + extension);
		//currentImage = icon.getImage();
		currentImage = loadImage(name + "." + extension);
		centerX = inX;
		centerY = inY;
		direction = inDirection % 360;
		altitude = inAltitude;
	}
	
	protected BufferedImage loadImage(String fileName) {
		File imageFile = resLoader.getFile(fileName);
		BufferedImage image;
		try {
			image = ImageIO.read(imageFile);
		} catch (IOException e) {
			System.out.println("Image could not be read");
			image = null;
		}
		
		return image;
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
	
	public BufferedImage getImage() {
		return currentImage;
	}
	
	public double getScale() {
		return 1;
	}
	
	public boolean getIsSelected() {
		return isSelected;
	}
	
	public void setIsSelected(boolean condition) {
		isSelected = condition;
	}
	
	public float getOpacity() {
		return opacity;
	}
	
	public boolean getHasShadow() {
		return hasShadow;
	}
	
	public void setHasShadow(boolean condition) {
		hasShadow = condition;
	}
	
	public Effect getExplosionAnimation() {
		String explosionSize = "";
		String animationName = chooseExplosionAnimation(explosionSize);
		int frameSpeed = 2;
		Effect explosion = new Effect(animationName, centerX, centerY, direction, altitude, 
				EXPLOSION_ANIMATION_FRAMES, frameSpeed);
		return explosion;
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
		
		int radius = Math.round(Math.min(width, height) / 2);
		return new Circle(centerX, centerY, radius);
	}
	
	public int compareTo (Entity other) {
		int difference = centerX - other.getCenterX();
		
		if(difference != 0) {
			return difference;
		}
		
		difference = centerY - other.getCenterY();
		
		return difference;
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
