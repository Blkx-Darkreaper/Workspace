package ACM;

import java.awt.Image;
import java.awt.event.KeyEvent;
import java.security.AllPermission;

import static ACM.Global.*;

import javax.swing.ImageIcon;

public class Player extends Character {
	
	private boolean playerFiring = false;
	
	public Player(Aircraft inPlane) {
		super(inPlane);
	}
	
	public Aircraft getPlayerCraft() {
		return (Aircraft) craft;
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
			craft.airspeed -= 1;
			//dx = -1;
			break;
		case KeyEvent.VK_RIGHT:
			craft.airspeed += 1;
			//dx = 1;
			break;
		case KeyEvent.VK_UP:
			craft.dy = -1;
			break;
		case KeyEvent.VK_DOWN:
			craft.dy = 1;
			break;
		case KeyEvent.VK_SPACE:
			if(getPlayerFiring() == true) {
				break;
			}
			
			ImageIcon bulletIcon = new ImageIcon("I:/bullet.png");
			Image image = craft.getImage();
			int playerX = PLAYER_START_POSITION + image.getWidth(null) - (bulletIcon.getIconWidth() + 5);
			int playerY = craft.getY() + image.getHeight(null) / 2;
			
			Bullet aBullet = new Bullet(bulletIcon, playerX, playerY, craft);
			craft.allProjectiles.add(aBullet);
			setPlayerFiring(true);
			break;
		}
	}
	
	public void keyReleased(KeyEvent e) {
		int key = e.getKeyCode();

		switch (key) {
		case KeyEvent.VK_LEFT:
		case KeyEvent.VK_RIGHT:
			craft.airspeed += 0;
			//dx = 0;
			break;
		case KeyEvent.VK_UP:
		case KeyEvent.VK_DOWN:
			craft.dy = 0;
			break;
		case KeyEvent.VK_SPACE:
			setPlayerFiring(false);
			break;
		}
	}
}
