package Strikeforce;

import java.awt.Image;
import java.awt.Rectangle;
import javax.swing.ImageIcon;
import static Strikeforce.Board.*;

public class Entity {

	protected int x, y;
	protected Image currentImage;
	protected Entity master;
	
	public Entity (ImageIcon icon) {
		currentImage = icon.getImage();
		x = 200 - currentImage.getWidth(null) / 2;
		y = 100 + currentImage.getHeight(null) / 2;
	}
	
	public Entity(ImageIcon icon, int startX, int startY) {
		currentImage = icon.getImage();
		x = startX;
		y = startY;
	}
	
	public int getX() {
		return x;
	}

	public int getY() {
		return y;
	}
	
	public Image getImage() {
		return currentImage;
	}
	
	public Entity getOwner() {
		return master;
	}
	
	public void setOwner(Entity inOwner) {
		master = inOwner;
	}
	
	public void destroy() {
		master = null;
	}
	
	public Rectangle getBounds() {
		return new Rectangle(x, y, currentImage.getWidth(null), currentImage.getHeight(null));
	}
	
	public boolean checkForCollision(Entity other) {
		return other.getBounds().intersects(getBounds());
	}
}
