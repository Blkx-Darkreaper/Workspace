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
	private Level level;
	private Player player;
	private List<Aircraft> allBandits;
	private Image background;
	private Timer time;
	private int position;
	private int scrollRate;

	public Board() {
		resLoader = new ResLoader(this.getClass().getClassLoader());
		view = new View();
		
		ImageIcon playerIcon = resLoader.getImageIcon("f18-level.png");
		Aircraft playerCraft = new Aircraft(playerIcon);
		player = new Player(playerCraft);
		allBandits = new ArrayList<>();
		
		ImageIcon banditIcon = resLoader.getImageIcon("enemy-jet.png");
		Aircraft bandit =  new Aircraft(banditIcon, 100, 450);
		allBandits.add(bandit);
		
		Aircraft bandit2 =  new Aircraft(banditIcon, 200, 350);
		allBandits.add(bandit2);
		
		addKeyListener(new KeyActionListener());
		setFocusable(true);
		
		ImageIcon backgroundIcon = resLoader.getImageIcon("starfield.png");
		background = backgroundIcon.getImage();
		BACKGROUND_HEIGHT = background.getHeight(null);
		BACKGROUND_WIDTH = background.getWidth(null);
		
		ArrayList<Image> backgroundImages = new ArrayList<>();
		for(int i = 1; i < 8; i++) {
			backgroundIcon = resLoader.getImageIcon("Paris" + i + ".png");
			backgroundImages.add(backgroundIcon.getImage());
		}
		level = new Level(backgroundImages);
		
		position = 0;
		scrollRate = 1;
		
		time = new Timer(TIME_INTERVAL, this);
		time.start();
	}

	public void actionPerformed(ActionEvent e) {
		scrollVertical();
		player.getPlayerCraft().move();
		
		for(Aircraft aBandit : allBandits) {
			aBandit.move();
			for(Projectile aProjectile : aBandit.allProjectiles) {
				aProjectile.move();
			}
		}
		
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			aProjectile.move();
		}
		repaint();
	}

	private void scrollVertical() {
		position += scrollRate;
	}

	public void paint(Graphics g) {
		super.paint(g);
		Graphics2D g2d = (Graphics2D) g;
		
		paintBackground(g2d);
		
		detectCollisions();
		
		paintPlayerBallistics(g2d);
		paintEnemies(g2d);
		paintPlayer(g2d);
		
		// View bounds
		g2d.setColor(Color.red); //debu
		g2d.draw(view.getViewBox()); //debug
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
			for(Iterator<Projectile> projectileIter = allEnemyProjectiles.iterator(); projectileIter.hasNext();) {
				Projectile aProjectile = projectileIter.next();

				boolean collision = playerCraft.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				JOptionPane.showMessageDialog(null, "You have been shot down!", "Game Over", 
						JOptionPane.INFORMATION_MESSAGE);
				playerCraft = null;
				aProjectile.destroy();
				projectileIter.remove();
			}
		}
	}
	
	private void paintEntity(Graphics2D g2d, Entity toDraw) {
		if(view.checkWithinBounds(toDraw) == false) {
			return;
		}
		
		g2d.drawImage(toDraw.getImage(), toDraw.getX(), 
				FRAME_HEIGHT - toDraw.getY() + toDraw.getImage().getHeight(null), null);
	}

	private void paintPlayerBallistics(Graphics2D g2d) {
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			paintEntity(g2d, aProjectile);
		}
	}

	private void paintPlayer(Graphics2D g2d) {
		g2d.drawImage(player.getPlayerCraft().getImage(), player.getPlayerCraft().getX(), 
				FRAME_HEIGHT - player.getPlayerCraft().getY(), null);
	}
	
	private void paintEnemies(Graphics2D g2d) {
		for(Aircraft aBandit : allBandits) {
			paintEntity(g2d, aBandit);
			for(Projectile aProjectile : aBandit.allProjectiles) {
				paintEntity(g2d, aProjectile);
			}
		}
	}

	private void paintBackground(Graphics2D g2d) {
		//System.out.println(background.getWidth(null));
		int vertOffset = BACKGROUND_HEIGHT * (position / BACKGROUND_HEIGHT);
		//System.out.println("X: " + player.getX());
		//System.out.println("Offset: " + offset);
		//System.out.println("Mod: " + player.getX() / 512);
		
		g2d.drawImage(background, 0, position - (vertOffset - BACKGROUND_HEIGHT), null);
		g2d.drawImage(background, 0, position - vertOffset, null);
		g2d.drawImage(background, 0, position - (vertOffset + BACKGROUND_HEIGHT), null);
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

	private List<Image> allImages = new ArrayList<>();
	private int nextIndex = 0;
	
	public Level (ArrayList allImagesToAdd) {
		allImages = allImagesToAdd;
	}
	
	public Image getNextImage () {
		Image nextImage = allImages.get(nextIndex);
		nextIndex++;
		
		if(nextIndex >= allImages.size()) {
			nextIndex = 0;
		}
		
		return nextImage;
	}
}