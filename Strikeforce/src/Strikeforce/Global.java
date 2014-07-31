package Strikeforce;

import static Strikeforce.Global.resLoader;

import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

public class Global {
	public static ResLoader resLoader;
	
	public static final int FRAME_WIDTH = 800;
	public static final int FRAME_HEIGHT = 600;
	
	public static final int TIME_INTERVAL = 20;
	
	public static final int VIEW_WIDTH = 250;
	public static final int VIEW_HEIGHT = 500;
	public static final int VIEW_POSITION_X = 100;
	public static final int VIEW_POSITION_Y = 50;
	
	public static final int LEVEL_WIDTH = 400;

	public static final int UPPER_BOUNDS_X = LEVEL_WIDTH - 10;
	public static final int LOWER_BOUNDS_X = 10;
	public static final int UPPER_BOUNDS_Y = VIEW_HEIGHT - 20;
	public static final int LOWER_BOUNDS_Y = 20;
	
	public static final int HARD_BANK_ANGLE = 1;
	public static final int MAX_BANK_ANGLE = 2;
	
	public static Weapon singleShot = new Weapon("Single shot", "Basic gun") {
		public List<Projectile> openFire(int originX, int originY) {
			List<Projectile> allShots = new ArrayList<>();

			ImageIcon bulletIcon = resLoader.getImageIcon("bullet.png");
			int startX = originX;
			int startY = originY + bulletIcon.getIconHeight() / 2;
			
			allShots.add(new Projectile(bulletIcon, startX, startY, 0, 6));
			
			return allShots;
		}
	};
	
	public static Weapon splitShot = new Weapon("Split shot", "Fires two shots away at an angle") {
		public List<Projectile> openFire(int originX, int originY) {
			List<Projectile> allShots = new ArrayList<>();
			
			ImageIcon bulletIcon = resLoader.getImageIcon("bullet2.png");
			int startX = originX;
			int startY = originY + bulletIcon.getIconHeight() / 2;
			
			allShots.add(new Projectile(bulletIcon, startX, startY, -2, 6));
			allShots.add(new Projectile(bulletIcon, startX, startY, 2, 6));
			
			return allShots;
		}
	};
}
