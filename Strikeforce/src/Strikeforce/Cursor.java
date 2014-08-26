package Strikeforce;

import java.awt.Rectangle;
import java.awt.event.KeyEvent;

import static Strikeforce.Global.*;
import static Strikeforce.Board.*;

public class Cursor extends Entity {
	
	private final int moveUpKey = KeyEvent.VK_UP;
	private final int moveDownKey = KeyEvent.VK_DOWN;
	private final int moveLeftKey = KeyEvent.VK_LEFT;
	private final int moveRightKey = KeyEvent.VK_RIGHT;
	private final int selectKey = KeyEvent.VK_A;
	private final int cancelKey = KeyEvent.VK_S;
	
	private Entity selectedEntity;
	private int levelTop;

	public Cursor(String inName, int inX, int inY, int inLevelTop) {
		super(inName, inX, inY, 0, 0);
		levelTop = inLevelTop;
	}
	
	public void setLevelTop(int inLevelTop) {
		levelTop = inLevelTop;
	}
	
	public void moveLeft() {
		centerX -= CELL_SIZE;
		
		int leftEdge = CELL_SIZE / 2;
		if(centerX < leftEdge) {
			centerX = leftEdge;
		}
	}
	
	public void moveRight() {
		centerX += CELL_SIZE;
		
		int rightEdge = LEVEL_WIDTH - CELL_SIZE / 2;
		if(centerX > rightEdge) {
			centerX = rightEdge;
		}
	}
	
	public void moveUp() {
		centerY += CELL_SIZE;
		
		int topEdge = levelTop - CELL_SIZE / 2;
		if(centerY > topEdge) {
			centerY = topEdge;
		}
	}
	
	public void moveDown() {
		centerY -= CELL_SIZE;
		
		int bottomEdge = CELL_SIZE / 2;
		if(centerY < bottomEdge) {
			centerY = bottomEdge;
		}
	}
	
	public void select() {
		Rectangle cursorBox = getBounds();
		selectedEntity = selectBuildingInArea(cursorBox);
	}
	
	public void deselect() {
		selectedEntity = null;
	}
	
	public void update() {
		return;
	}
	
	public void keyPressed(KeyEvent e) {
		int key = e.getKeyCode();

		switch (key) {
		case moveLeftKey:
			moveLeft();
			break;
		case moveRightKey:
			moveRight();
			break;
		case moveUpKey:
			moveUp();
			break;
		case moveDownKey:
			moveDown();
			break;
		case selectKey:
			if(selectedEntity != null) {
				deselect();
			}
			
			select();
			break;
		case cancelKey:
			deselect();
			break;
		}
	}
	
	public void keyReleased(KeyEvent e) {
		int key = e.getKeyCode();

		switch (key) {
		default:
			break;
		}
	}
}
