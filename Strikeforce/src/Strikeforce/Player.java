package Strikeforce;

import java.awt.Image;
import java.awt.event.KeyEvent;

import static Strikeforce.Global.*;

import javax.swing.ImageIcon;

public class Player {
	
	private Aircraft craft;
	private boolean playerFiring = false;
	
	public Player(Aircraft inCraft) {
		craft = inCraft;
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
			craft.dx = -craft.getAirspeed();
			
			if(craft.bank == -MAX_BANK_ANGLE) {
				break;
			}
			
			craft.bank--;
			break;
		case KeyEvent.VK_RIGHT:
			craft.dx = craft.getAirspeed();
			
			if(craft.bank == MAX_BANK_ANGLE) {
				break;
			}
			
			craft.bank++;
			break;
		case KeyEvent.VK_UP:
			craft.dy = craft.getAirspeed();
			break;
		case KeyEvent.VK_DOWN:
			craft.dy = -craft.getAirspeed();
			break;
		case KeyEvent.VK_SPACE:
			if(getPlayerFiring() == true) {
				break;
			}
			
			ImageIcon bulletIcon = resLoader.getImageIcon("bullet.png");
			Image image = craft.getImage();
			int playerX = craft.getX() - image.getWidth(null) / 2;
			int playerY = craft.getY() - bulletIcon.getIconHeight() + 5;
			
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
			craft.dx = 0;
			craft.bank = 0;
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
