package Strikeforce;

import java.awt.Image;
import java.awt.event.KeyEvent;

import static Strikeforce.Global.*;

import javax.swing.ImageIcon;

public class Player {
	
	private Aircraft craft;
	private boolean playerFiring = false;
	private boolean playerBoosting = false;
	
	private final int moveUpKey = KeyEvent.VK_UP;
	private final int moveDownKey = KeyEvent.VK_DOWN;
	private final int moveLeftKey = KeyEvent.VK_LEFT;
	private final int moveRightKey = KeyEvent.VK_RIGHT;
	private final int fireAKey = KeyEvent.VK_A;
	private final int fireBKey = KeyEvent.VK_S;
	private final int boostKey = KeyEvent.VK_D;
	private final int doLoopKey = KeyEvent.VK_F;
	
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
		case moveLeftKey:
			craft.bankLeft();
			break;
		case moveRightKey:
			craft.bankRight();
			break;
		case moveUpKey:
			craft.moveUp();
			break;
		case moveDownKey:
			craft.moveDown();
			break;
		case fireAKey:
			if(getPlayerFiring() == true) {
				break;
			}
			
			craft.openFire();
			playerFiring = true;
			break;
		case boostKey:
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
		case doLoopKey:
			craft.doLoop();
			break;
		case moveLeftKey:
		case moveRightKey:
			craft.levelOff();
			break;
		case moveUpKey:
		case moveDownKey:
			craft.cruise();
			break;
		case fireAKey:
			setPlayerFiring(false);
			break;
		case boostKey:
			setPlayerBoosting(false);
			break;
		}
	}

	public void gameover() {
		craft = null;		
	}
}
