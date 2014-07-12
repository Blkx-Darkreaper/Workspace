package ACM;

import java.awt.Image;
import java.awt.Rectangle;

import javax.swing.ImageIcon;

public class Entity {

	protected int x, y;
	protected Image image;
	protected Entity owner;
	
	public Entity (ImageIcon icon) {
		image = icon.getImage();
		x = 0;
		y = 200;
	}
	
	public Entity(ImageIcon icon, int startX, int startY) {
		image = icon.getImage();
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
		return image;
	}
	
	public Entity getOwner() {
		return owner;
	}
	
	public void setOwner(Entity inOwner) {
		owner = inOwner;
	}
	
	public void destroy() {
		owner = null;
	}
	
	public Rectangle getBounds() {
		return new Rectangle(x, y, image.getWidth(null), image.getHeight(null));
	}
	
	public boolean checkForCollision(Entity other) {
		return other.getBounds().intersects(getBounds());
	}
}
