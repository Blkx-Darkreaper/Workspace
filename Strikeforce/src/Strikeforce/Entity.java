package Strikeforce;

import java.awt.Image;
import java.awt.Rectangle;
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
	
	public boolean checkForCollision(Entity other) {
		return other.getBounds().intersects(getBounds());
	}
}
