package Strikeforce;

import java.awt.Image;
import java.awt.Rectangle;
import javax.swing.ImageIcon;
import static Strikeforce.Board.*;

public class Entity {

	protected int x, y;
	protected Image image;
	protected Entity master;
	
	public Entity (ImageIcon icon) {
		image = icon.getImage();
		x = 200 - image.getWidth(null) / 2;
		y = 500 - image.getHeight(null) / 2;
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
		return master;
	}
	
	public void setOwner(Entity inOwner) {
		master = inOwner;
	}
	
	public void destroy() {
		master = null;
	}
	
	public Rectangle getBounds() {
		return new Rectangle(x, y, image.getWidth(null), image.getHeight(null));
	}
	
	public boolean checkForCollision(Entity other) {
		return other.getBounds().intersects(getBounds());
	}
}
