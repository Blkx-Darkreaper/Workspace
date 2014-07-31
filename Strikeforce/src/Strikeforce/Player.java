package Strikeforce;

import java.awt.Image;
import java.awt.event.KeyEvent;

import static Strikeforce.Global.*;

import javax.swing.ImageIcon;

public class Player {
	
	private Aircraft craft;
	private boolean playerFiring = false;
	private boolean playerBoosting = false;
	
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
	
	public boolean getPlayerBoosting() {
		return playerBoosting;
	}
	
	public void setPlayerBoosting (boolean condition) {
		playerBoosting = condition;
	}

	public void keyPressed(KeyEvent e) {
		int key = e.getKeyCode();

		switch (key) {
/*		case KeyEvent.VK_F:
			craft.doLoop();
			break;*/
		case KeyEvent.VK_LEFT:
			craft.bankLeft();
			break;
		case KeyEvent.VK_RIGHT:
			craft.bankRight();
			break;
		case KeyEvent.VK_UP:
			craft.moveUp();
			break;
		case KeyEvent.VK_DOWN:
			craft.moveDown();
			break;
		case KeyEvent.VK_SPACE:
			if(getPlayerFiring() == true) {
				break;
			}
			
			craft.openFire();
			playerFiring = true;
			break;
		case KeyEvent.VK_D:
			if(getPlayerBoosting() == true) {
				break;
			}
			
			craft.accelerate();
			playerBoosting = true;
			break;
		}
	}
	
	public void keyReleased(KeyEvent e) {
		int key = e.getKeyCode();

		switch (key) {
		case KeyEvent.VK_F:
			craft.doLoop();
			break;
		case KeyEvent.VK_LEFT:
		case KeyEvent.VK_RIGHT:
			craft.levelOff();
			break;
		case KeyEvent.VK_UP:
		case KeyEvent.VK_DOWN:
			craft.cruise();
			break;
		case KeyEvent.VK_SPACE:
			setPlayerFiring(false);
			break;
		case KeyEvent.VK_D:
			setPlayerBoosting(false);
			break;
		}
	}

	public void gameover() {
		craft = null;		
	}
}
