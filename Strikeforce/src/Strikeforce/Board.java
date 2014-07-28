package Strikeforce;

import java.awt.*;
import java.awt.event.*;
import java.io.InputStream;
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
	private int positionY;

	public Board() {
		resLoader = new ResLoader(this.getClass().getClassLoader());
		view = new View();
		
		ImageIcon playerIcon = resLoader.getImageIcon("f18-level.png");
		Aircraft playerCraft = new Aircraft(playerIcon);
		player = new Player(playerCraft);
		allBandits = new ArrayList<>();
		
		ImageIcon banditIcon = resLoader.getImageIcon("enemy-jet.png");
		Aircraft bandit =  new Aircraft(banditIcon, 120, 450);
		allBandits.add(bandit);
		
		Aircraft bandit2 =  new Aircraft(banditIcon, 220, 350);
		allBandits.add(bandit2);
		
		addKeyListener(new KeyActionListener());
		setFocusable(true);
		
		ImageIcon backgroundIcon = resLoader.getImageIcon("starfield.png");
		background = backgroundIcon.getImage();
		BACKGROUND_HEIGHT = background.getHeight(null);
		BACKGROUND_WIDTH = background.getWidth(null);
		
		ArrayList<Entity> levelSectors = new ArrayList<>();
		int startPositionY = 0;
		for(int i = 1; i < 8; i++) {
			backgroundIcon = resLoader.getImageIcon("Paris" + i + ".png");
			Mover sector = new Mover(backgroundIcon, backgroundIcon.getIconWidth() / 2, startPositionY);
			sector.setAirspeed(0);
			levelSectors.add(sector);
			startPositionY += backgroundIcon.getIconHeight();
		}
		currentLevel = new Level(levelSectors);
		
		positionY = 0;
		
		time = new Timer(TIME_INTERVAL, this);
		time.start();
	}
	
	public View getView() {
		return view;
	}

	private int getScrollRate() {
		return player.getPlayerCraft().getAirspeed();
	}

	public void actionPerformed(ActionEvent e) {
		scrollVertical();
		player.getPlayerCraft().move();
		System.out.println("X: " + player.getPlayerCraft().getX() + ", Y: " + player.getPlayerCraft().getY()); //debug
		
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			aProjectile.move();
		}
		
		repaint();
	}
	
	private void panHorizontal() {
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			aProjectile.move(getScrollRate(), 0);
		}
		
		for(Aircraft aBandit : allBandits) {
			aBandit.move(getScrollRate(), 0);
			for(Projectile aProjectile : aBandit.allProjectiles) {
				aProjectile.move(getScrollRate(), 0);
			}
		}
		
		for(Mover aSector : currentLevel.getAllSectors()) {
			aSector.move(getScrollRate(), 0);
		}
	}

	private void scrollVertical() {
		for(Aircraft aBandit : allBandits) {
			aBandit.move(0, -getScrollRate());
			for(Projectile aProjectile : aBandit.allProjectiles) {
				aProjectile.move(0, -getScrollRate());
			}
		}
		
		for(Mover aSector : currentLevel.getAllSectors()) {
			aSector.move(0, -getScrollRate());
		}
		
		positionY += getScrollRate();
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
		g2d.setColor(Color.red); //debu
		g2d.draw(view.getViewBox()); //debug
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
				
				banditIter.remove();
				aProjectile.destroy();
				projectileIter.remove();
			}
			
			List<Projectile> allEnemyProjectiles = aBandit.allProjectiles;
			
			boolean collision = playerCraft.checkForCollision(aBandit);
			
			if(collision == true) {
				aBandit.destroy();
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
				
				aProjectile.destroy();
				projectileIter.remove();
				//gameover();
				return;
			}
		}
	}
	
	private void redrawEntity(Graphics2D g2d, Entity toDraw) {
		g2d.drawImage(toDraw.getImage(), toDraw.getX() - toDraw.getImage().getWidth(null) / 2 + VIEW_POSITION_X, 
				VIEW_HEIGHT - toDraw.getY() + toDraw.getImage().getHeight(null) / 2 + VIEW_POSITION_Y, null);
	}

	private void updatePlayerProjectiles(Graphics2D g2d) {
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			
			if(checkWithinView(aProjectile) == false) {
				aProjectile.outOfRange();
				return;
			}
			
			redrawEntity(g2d, aProjectile);
		}
	}

	private void updatePlayer(Graphics2D g2d) {
		redrawEntity(g2d, player.getPlayerCraft());
	}
	
	private void updateEnemies(Graphics2D g2d) {
		for(Aircraft aBandit : allBandits) {
			
			redrawEntity(g2d, aBandit);
			
			for(Projectile aProjectile : aBandit.allProjectiles) {
				
				if(checkWithinView(aProjectile) == false) {
					aProjectile.outOfRange();
					return;
				}
				
				redrawEntity(g2d, aProjectile);
			}
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
			redrawEntity(g2d, aSector);
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

class View {
	
	private Rectangle viewBox;
	
	public View () {
		viewBox = new Rectangle(VIEW_POSITION_X, VIEW_POSITION_Y, VIEW_WIDTH, VIEW_HEIGHT);
	}
	
	public boolean checkWithinBounds(Entity toCheck) {
		return toCheck.getBounds().intersects(viewBox);
	}
	
	public Rectangle getViewBox() {
		return viewBox;
	}
}

class Level {

	private List<Projectile> allSectors = new ArrayList<>();
	
	public Level (ArrayList allSectorsToAdd) {
		allSectors = allSectorsToAdd;
	}
	
	public Projectile getSectorAtIndex (int index) {
		if(index < 0) {
			return null;
		}
		
		if(index >= allSectors.size()) {
			return null;
		}
		
		return allSectors.get(index);
	}
	
	public List<Projectile> getAllSectors () {
		return allSectors;
	}
}