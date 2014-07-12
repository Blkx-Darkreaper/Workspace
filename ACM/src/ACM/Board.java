package ACM;

import java.awt.*;
import java.awt.event.*;
import java.util.List;
import java.util.ArrayList;
import static ACM.Global.*;
import javax.swing.*;

public class Board extends JPanel implements ActionListener {

	static int SCREEN_WIDTH;
	private Player player;
	private List<Aircraft> allBandits;
	private Image background;
	private Timer time;
	private int position;

	public Board() {
		ImageIcon playerIcon = new ImageIcon("I:/Users/Darkreaper/Documents/Projects/ACM/bluefighter-level.png");
		Aircraft playerCraft = new Aircraft(playerIcon);
		player = new Player(playerCraft);
		allBandits = new ArrayList<>();
		
		ImageIcon banditIcon = new ImageIcon("I:/Users/Darkreaper/Documents/Projects/ACM/fighter-red.png");
		Aircraft bandit =  new Aircraft(banditIcon, 100, 150, 2);
		allBandits.add(bandit);
		
		Aircraft bandit2 =  new Aircraft(banditIcon, 300, 50, 3);
		allBandits.add(bandit2);
		
		addKeyListener(new KeyActionListener());
		setFocusable(true);
		ImageIcon backgroundIcon = new ImageIcon("I:/Users/Darkreaper/Documents/Projects/ACM/mountains-background.png");
		background = backgroundIcon.getImage();
		position = 0;
		SCREEN_WIDTH = background.getWidth(null);
		
		time = new Timer(5, this);
		time.start();
	}

	public void actionPerformed(ActionEvent e) {
		scrollHorizontal();
		player.getPlayerCraft().moveVertically();
		
		for(Aircraft aBandit : allBandits) {
			aBandit.moveRelative(player.getPlayerCraft());
			for(Projectile aProjectile : aBandit.allProjectiles) {
				aProjectile.moveRelative(player.getPlayerCraft());
			}
		}
		
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			aProjectile.moveRelative(player.getPlayerCraft());
		}
		repaint();
	}

	private void scrollHorizontal() {
		position += player.getPlayerCraft().getAirspeed();
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
				
				JOptionPane.showMessageDialog(null, "You have been shot down!", "Game Over", JOptionPane.INFORMATION_MESSAGE);
				playerCraft = null;
				aProjectile.destroy();
				allEnemyProjectiles.remove(aProjectile);
			}
		}
		
		
		
	}

	private void paintBallistics(Graphics2D g2d) {
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			g2d.drawImage(aProjectile.getImage(), aProjectile.getX(), aProjectile.getY(), null);
		}
	}

	private void paintPlayer(Graphics2D g2d) {
		g2d.drawImage(player.getPlayerCraft().getImage(), PLAYER_START_POSITION, player.getPlayerCraft().getY(), null);
	}
	
	private void paintEnemies(Graphics2D g2d) {
		for(Aircraft aBandit : allBandits) {
			g2d.drawImage(aBandit.getImage(), aBandit.getX(), aBandit.getY(), null);
			for(Projectile aProjectile : aBandit.allProjectiles) {
				g2d.drawImage(aProjectile.getImage(), aProjectile.getX(), aProjectile.getY(), null);
			}
		}
	}

	private void paintBackground(Graphics2D g2d) {
		//System.out.println(background.getWidth(null));
		int offset = SCREEN_WIDTH * (position / SCREEN_WIDTH);
		//System.out.println("X: " + player.getX());
		//System.out.println("Offset: " + offset);
		//System.out.println("Mod: " + player.getX() / 512);
		
		g2d.drawImage(background, -position + offset, 0, null);
		g2d.drawImage(background, -position + (offset + SCREEN_WIDTH), 0, null);
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
