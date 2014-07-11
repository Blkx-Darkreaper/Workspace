package ACM;

import java.awt.event.KeyEvent;
import java.security.AllPermission;
import static ACM.Global.*;
import javax.swing.ImageIcon;

public class Player extends Aircraft {
	
	private boolean playerFiring = false;
	
	public Player(ImageIcon icon) {
		super(icon);
	}
	
	public boolean getPlayerFiring () {
		return playerFiring;
	}
	
	public void setPlayerFiring (boolean inCondition) {
		playerFiring = inCondition;
	}

	public void keyPressed(KeyEvent e) {
		int key = e.getKeyCode();

		switch (key) {
		case KeyEvent.VK_LEFT:
			airspeed -= 1;
			//dx = -1;
			break;
		case KeyEvent.VK_RIGHT:
			airspeed += 1;
			//dx = 1;
			break;
		case KeyEvent.VK_UP:
			dy = -1;
			break;
		case KeyEvent.VK_DOWN:
			dy = 1;
			break;
		case KeyEvent.VK_SPACE:
			if(getPlayerFiring() == true) {
				break;
			}
			
			ImageIcon bulletIcon = new ImageIcon("I:/bullet.png");
			int playerX = PLAYER_START_POSITION + image.getWidth(null) - (bulletIcon.getIconWidth() + 5);
			int playerY = getY() + image.getHeight(null) / 2;
			
			Bullet aBullet = new Bullet(bulletIcon, playerX, playerY, this);
			allProjectiles.add(aBullet);
			setPlayerFiring(true);
			break;
		}
	}
	
	public void keyReleased(KeyEvent e) {
		int key = e.getKeyCode();

		switch (key) {
		case KeyEvent.VK_LEFT:
		case KeyEvent.VK_RIGHT:
			airspeed += 0;
			//dx = 0;
			break;
		case KeyEvent.VK_UP:
		case KeyEvent.VK_DOWN:
			dy = 0;
			break;
		case KeyEvent.VK_SPACE:
			setPlayerFiring(false);
			break;
		}
	}
}
