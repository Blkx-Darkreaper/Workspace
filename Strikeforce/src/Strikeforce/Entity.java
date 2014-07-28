package Strikeforce;

import java.awt.Image;
import java.awt.Rectangle;
import javax.swing.ImageIcon;
import static Strikeforce.Board.*;
import static Strikeforce.Global.*;

public class Entity {

	protected int x, y;
	protected Image currentImage;
	
	public Entity (ImageIcon icon) {
		currentImage = icon.getImage();
		x = VIEW_WIDTH / 2;
		y = 100;
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
	
	public Rectangle getBounds() {
		return new Rectangle(x, y, currentImage.getWidth(null), currentImage.getHeight(null));
	}
	
	public boolean checkForCollision(Entity other) {
		return other.getBounds().intersects(getBounds());
	}
}
