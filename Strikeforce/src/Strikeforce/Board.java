package Strikeforce;

import java.awt.*;
import java.awt.event.*;
import java.io.InputStream;
import java.util.List;
import java.util.ArrayList;

import static Strikeforce.Global.*;

import javax.swing.*;

public class Board extends JPanel implements ActionListener {

	static int BACKGROUND_HEIGHT;
	static int BACKGROUND_WIDTH;
	private Player player;
	private List<Aircraft> allBandits;
	private Image background;
	private Timer time;
	private int position;
	private int scrollRate;

	public Board() {
		resLoader = new ResLoader(this.getClass().getClassLoader());
		
		ImageIcon playerIcon = resLoader.getImageIcon("f18-level.png");
		Aircraft playerCraft = new Aircraft(playerIcon);
		player = new Player(playerCraft);
		allBandits = new ArrayList<>();
		
		ImageIcon banditIcon = resLoader.getImageIcon("f18-level.png");
		Aircraft bandit =  new Aircraft(banditIcon, 100, 150);
		allBandits.add(bandit);
		
		Aircraft bandit2 =  new Aircraft(banditIcon, 300, 350);
		allBandits.add(bandit2);
		
		addKeyListener(new KeyActionListener());
		setFocusable(true);
		ImageIcon backgroundIcon = resLoader.getImageIcon("starfield.png");
		background = backgroundIcon.getImage();
		position = 0;
		BACKGROUND_HEIGHT = background.getHeight(null);
		BACKGROUND_WIDTH = background.getWidth(null);
		scrollRate = 1;
		
		time = new Timer(5, this);
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
		
		paintBallistics(g2d);
		paintEnemies(g2d);
		paintPlayer(g2d);
	}
	
	private void detectCollisions () {
		Aircraft playerCraft = player.getPlayerCraft();
		List<Projectile> allFriendlyProjectiles = playerCraft.getAllProjectiles();
		for(Aircraft aBandit : allBandits) {
			for(Projectile aProjectile : allFriendlyProjectiles) {
				boolean collision = aBandit.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				allBandits.remove(aBandit);
				aProjectile.destroy();
				allFriendlyProjectiles.remove(aProjectile);
			}
			
			List<Projectile> allEnemyProjectiles = aBandit.allProjectiles;
			for(Projectile aProjectile : allEnemyProjectiles) {
				boolean collision = playerCraft.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				JOptionPane.showMessageDialog(null, "You have been shot down!", "Game Over", 
						JOptionPane.INFORMATION_MESSAGE);
				playerCraft = null;
				aProjectile.destroy();
				allEnemyProjectiles.remove(aProjectile);
			}
		}
	}

	private void paintBallistics(Graphics2D g2d) {
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			g2d.drawImage(aProjectile.getImage(), aProjectile.getX(), SCREEN_HEIGHT - aProjectile.getY(), null);
		}
	}

	private void paintPlayer(Graphics2D g2d) {
		g2d.drawImage(player.getPlayerCraft().getImage(), player.getPlayerCraft().getX(), 
				SCREEN_HEIGHT - player.getPlayerCraft().getY(), null);
	}
	
	private void paintEnemies(Graphics2D g2d) {
		for(Aircraft aBandit : allBandits) {
			g2d.drawImage(aBandit.getImage(), aBandit.getX(), SCREEN_HEIGHT - aBandit.getY(), null);
			for(Projectile aProjectile : aBandit.allProjectiles) {
				g2d.drawImage(aProjectile.getImage(), aProjectile.getX(), SCREEN_HEIGHT - aProjectile.getY(), null);
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
