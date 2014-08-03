package Strikeforce;

import java.awt.*;
import java.awt.event.*;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.Collections;
import java.util.Iterator;
import java.util.List;
import java.util.ArrayList;

import static Strikeforce.Global.*;
import javax.swing.*;

public class Board extends JPanel implements ActionListener {

	static int BACKGROUND_HEIGHT;
	static int BACKGROUND_WIDTH;
	private View view;
	private Level currentLevel;
	private Player player;
	private List<Aircraft> allBandits;
	private Image background;
	private Timer time;

	public Board() {
		resLoader = new ResLoader(this.getClass().getClassLoader());
		
		ArrayList<Entity> levelSectors = new ArrayList<>();
		
		ImageIcon backgroundIcon = resLoader.getImageIcon("Paris1.png");
		Image background = backgroundIcon.getImage();
		
		BACKGROUND_HEIGHT = background.getHeight(null);
		BACKGROUND_WIDTH = background.getWidth(null);
		int startPositionY = BACKGROUND_HEIGHT / 2;
		
		for(int i = 1; i < 8; i++) {
			backgroundIcon = resLoader.getImageIcon("Paris" + i + ".png");
			Entity sector = new Entity(backgroundIcon, BACKGROUND_WIDTH / 2, startPositionY);
			levelSectors.add(sector);
			startPositionY += BACKGROUND_HEIGHT;
		}
		
		currentLevel = new Level(levelSectors);
		
		ImageIcon viewIcon = resLoader.getImageIcon("view.png");
		view = new View(viewIcon, BACKGROUND_WIDTH / 2, VIEW_HEIGHT / 2);
		//view = new View(BACKGROUND_WIDTH / 2, VIEW_HEIGHT / 2, VIEW_WIDTH, VIEW_HEIGHT);
		
		ImageIcon playerIcon = resLoader.getImageIcon("f18-level.png");
		Aircraft playerCraft = new Aircraft(playerIcon, BACKGROUND_WIDTH / 2, 100);
		List<Weapon> basicWeaponSetup = new ArrayList<>();
		List<Weapon> otherWeaponSetup = new ArrayList<>();
		basicWeaponSetup.add(singleShot);
		otherWeaponSetup.add(splitShot);
		playerCraft.setWeaponSetA(basicWeaponSetup);
		playerCraft.setWeaponSetB(otherWeaponSetup);
		player = new Player(playerCraft);
		
		allBandits = new ArrayList<>();
		
		addKeyListener(new KeyActionListener());
		setFocusable(true);
		
		time = new Timer(TIME_INTERVAL, this);
		time.start();
	}
	
	public View getView() {
		return view;
	}

	public Timer getTime() {
		return time;
	}

	public void actionPerformed(ActionEvent e) {
		String currentPhase = player.getPhase();
		if(currentPhase != "Raid") {
			return;
		}
		
		setViewScrollRate();
		setViewPanRate();
		view.move();
		
		player.getPlayerCraft().move();
		keepPlayerInView();
		//System.out.println("X: " + player.getPlayerCraft().getX() + ", Y: " + player.getPlayerCraft().getY()); //debug
		
		for(Aircraft aBandit : allBandits) {
			aBandit.move();
			
			for(Projectile aProjectile : aBandit.getAllProjectiles()) {
				aProjectile.move();
			}
		}
		
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			aProjectile.move();
		}
		
		repaint();
	}
	
	private void keepPlayerInView() {
		int upperBoundsY = view.getY() + view.getHalfHeight();
		int lowerBoundsY = view.getY() - view.getHalfHeight();
		
		int playerY = player.getPlayerCraft().getY();
		int halfPlayerHeight = player.getPlayerCraft().getHalfHeight();
		
		if((playerY - halfPlayerHeight) < lowerBoundsY) {
			player.getPlayerCraft().setY(lowerBoundsY + halfPlayerHeight);
		}
		
		if((playerY + halfPlayerHeight) > upperBoundsY) {
			player.getPlayerCraft().setY(upperBoundsY - halfPlayerHeight);
		}
	}

	private void setViewPanRate() {
		int panRate = player.getPlayerCraft().getSpeed();
		int panDirection = 0;

		int playerX = player.getPlayerCraft().getX();
		int viewX = view.getX();
		int thirdViewWidth = VIEW_WIDTH / 3;
		
		int displacement = playerX - viewX;
		int distance = Math.abs(displacement);
		
		if(distance > thirdViewWidth) {
			panDirection = distance / displacement;
		}
		
		panRate *= panDirection;
		view.setDeltaX(panRate);
	}
	
	private void setViewScrollRate() {
		int scrollRate = player.getPlayerCraft().getSpeed();
		view.setSpeed(scrollRate);
	}

	public void paint(Graphics g) {
		super.paint(g);
		Graphics2D g2d = (Graphics2D) g;
		
		updateBackground(g2d);
		
		detectCollisions();
		
		updatePlayerProjectiles(g2d);
		updateEnemies(g2d);
		updatePlayer(g2d);
		
		// View bounds
		drawEntity(g2d, view); //debug
	}
	
	private boolean checkWithinView(Entity toCheck) {
		return view.checkWithinBounds(toCheck);
	}
	
	private void detectCollisions () {
		Aircraft playerCraft = player.getPlayerCraft();
		List<Projectile> allFriendlyProjectiles = playerCraft.getAllProjectiles();
		for(Iterator<Aircraft> banditIter = allBandits.iterator(); banditIter.hasNext();) {
			Aircraft aBandit = banditIter.next();

			for(Iterator<Projectile> projectileIter = allFriendlyProjectiles.iterator(); projectileIter.hasNext();) {
				Projectile aProjectile = projectileIter.next();
				
				boolean collision = aBandit.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				int damage = aProjectile.getDamage();
				if(aBandit.criticalDamage(damage) == true) {
					banditIter.remove();
				}
				projectileIter.remove();
			}
			
			if(playerCraft.getInvulnerable() == true) {
				continue;
			}
			
			List<Projectile> allEnemyProjectiles = aBandit.allProjectiles;
			
			boolean collision = playerCraft.checkForCollision(aBandit);
			
			if(collision == true) {
				banditIter.remove();
				//gameover();
				return;
			}
			
			for(Iterator<Projectile> projectileIter = allEnemyProjectiles.iterator(); projectileIter.hasNext();) {
				Projectile aProjectile = projectileIter.next();

				collision = playerCraft.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				projectileIter.remove();
				//gameover();
				return;
			}
		}
	}
	
	private void drawEntity(Graphics2D g2d, Entity toDraw) {
		Image image = toDraw.getImage();
		int positionXRelativeToViewCenter = (toDraw.getX() - toDraw.getHalfWidth()) - view.getX();
		int positionYRelativeToViewCenter = (toDraw.getY() + toDraw.getHalfHeight()) - view.getY();
		
		int absolutePositionX = VIEW_POSITION_X + VIEW_WIDTH / 2 + positionXRelativeToViewCenter;
		int absolutePositionY = VIEW_POSITION_Y + VIEW_HEIGHT / 2 - positionYRelativeToViewCenter;
		
		g2d.drawImage(image, absolutePositionX, absolutePositionY, null);
	}

	private void updatePlayerProjectiles(Graphics2D g2d) {
		for(Iterator<Projectile> bulletIter = player.getPlayerCraft().getAllProjectiles().iterator(); 
				bulletIter.hasNext();) {
			Projectile aProjectile = bulletIter.next();
			
			if(checkWithinView(aProjectile) == false) {
				bulletIter.remove();
				continue;
			}
			
			drawEntity(g2d, aProjectile);
		}
	}

	private void updatePlayer(Graphics2D g2d) {
		drawEntity(g2d, player.getPlayerCraft());
	}
	
	private void updateEnemies(Graphics2D g2d) {
		allBandits.addAll(currentLevel.spawnLine(player.getPlayerCraft().getY()));
		
		for(Iterator<Aircraft> banditIter = allBandits.iterator(); banditIter.hasNext();) {
		Aircraft aBandit = banditIter.next();
		
			for(Iterator<Projectile> bulletIter = aBandit.getAllProjectiles().iterator(); bulletIter.hasNext();) {
				Projectile aProjectile = bulletIter.next();
				
				if(checkWithinView(aProjectile) == false) {
					bulletIter.remove();
					continue;
				}
				
				drawEntity(g2d, aProjectile);
			}
			
			if(checkWithinView(aBandit) == false) {
				continue;
			}
			
			drawEntity(g2d, aBandit);
		}
	}

	private void updateBackground(Graphics2D g2d) {
/*		//System.out.println(background.getWidth(null));
		int vertOffset = BACKGROUND_HEIGHT * (positionY / BACKGROUND_HEIGHT);
		//System.out.println("X: " + player.getX());
		//System.out.println("Offset: " + offset);
		//System.out.println("Mod: " + player.getX() / 512);
		
		g2d.drawImage(background, 0, positionY - (vertOffset - BACKGROUND_HEIGHT), null);
		g2d.drawImage(background, 0, positionY - vertOffset, null);
		g2d.drawImage(background, 0, positionY - (vertOffset + BACKGROUND_HEIGHT), null);*/
		
		for(Entity aSector : currentLevel.getAllSectors()) {
			if(checkWithinView(aSector) == false) {
				continue;
			}
			
			drawEntity(g2d, aSector);
		}
	}
	
	public void gameover() {
		JOptionPane.showMessageDialog(null, "You have been shot down!", "Game Over", 
				JOptionPane.INFORMATION_MESSAGE);
		player.gameover();
	}
	
	private class KeyActionListener extends KeyAdapter {
		
		public void keyReleased (KeyEvent e) {
			player.keyReleased(e);
		}
		
		public void keyPressed (KeyEvent e) {
			player.keyPressed(e);
		}
	}
}

class View extends Mover {
	
	public View (int startX, int startY, int inWidth, int inHeight) {
		super(startX, startY, inWidth, inHeight);
	}
	
	public View (ImageIcon icon, int startX, int startY) {
		super(icon, startX, startY);
	}
	
	public boolean checkWithinBounds(Entity toCheck) {
		Rectangle viewBox = getBounds();
		return toCheck.getBounds().intersects(viewBox);
	}
}

class Level {
	private static final int CELL_HEIGHT = 20;
	private static final int CELL_WIDTH = 20;
	private List<Entity> allSectors = new ArrayList<>();
	private List<String> spawnLines = new ArrayList<>();
	private boolean[] spawnedLines;
	
	public Level (ArrayList allSectorsToAdd) {
		allSectors = allSectorsToAdd;
		
		try(BufferedReader br = new BufferedReader(new FileReader("levels/1.lvl"))) {
			for(String line; (line = br.readLine()) != null; ) {
				spawnLines.add(line);
			}
		} catch (IOException e) {
			System.out.println("Level load error: " + e);
		}
		
		Collections.reverse(spawnLines);
		spawnedLines = new boolean[spawnLines.size()];
	}
	
	public List<Aircraft> spawnLine(int playerY) {
		List<Aircraft> bandits = new ArrayList<>();
		
		int index = playerY / CELL_HEIGHT;
		if(index < spawnLines.size() && !spawnedLines[index]) {
			spawnedLines[index] = true; // Set so we don't spawn these guys again
			
			ImageIcon banditIcon = resLoader.getImageIcon("enemy-jet.png");
			
			// Look through the chars of the level file's current line
			int x = 20;
			for(char c : spawnLines.get(index).toCharArray()) {
				switch(c) {
					case '1': { // Basic bandit
						Aircraft bandit = new Aircraft(banditIcon, x, VIEW_HEIGHT + playerY);
						System.out.println("Spawning basic bandit at: " + bandit.getX() + ", " + bandit.getY());
						bandit.setSpeed(-1);
						bandits.add(bandit);
						break;
					}
				}
				x += CELL_WIDTH;
			}
		}
		
		return bandits;
	}

	public Entity getSectorAtIndex (int index) {
		if(index < 0) {
			return null;
		}
		
		if(index >= allSectors.size()) {
			return null;
		}
		
		return allSectors.get(index);
	}
	
	public List<Entity> getAllSectors () {
		return allSectors;
	}
}