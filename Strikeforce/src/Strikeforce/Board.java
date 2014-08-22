package Strikeforce;

import java.awt.*;
import java.awt.event.*;
import java.awt.geom.AffineTransform;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.Collections;
import java.util.Deque;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.ArrayList;
import java.util.Queue;

import static Strikeforce.Global.*;

import javax.swing.*;

public class Board extends JPanel implements ActionListener {

	private int windowScale = 1;
	private static View view;
	static Level currentLevel;
	private static Player player;
	private List<Effect> allEffects;
	private List<Aircraft> allBandits;
	private List<Vehicle> allGroundVehicles;
	private List<Building> allCoverBuildings;
	private List<Building> allSurfaceBuildings;
	private Image background;
	private Timer time;

	public Board() {
		resLoader = new ResLoader(this.getClass().getClassLoader());
		
		currentLevel = new Level("1");
		
		ImageIcon viewIcon = resLoader.getImageIcon("view.png");
		int viewX = currentLevel.getWidth() / 2;
		int viewY = VIEW_HEIGHT / 2;
		int direction = 0;
		int altitude = 0;
		view = new View(viewIcon, viewX, viewY, direction, altitude);
		//view = new View(BACKGROUND_WIDTH / 2, VIEW_HEIGHT / 2, VIEW_WIDTH, VIEW_HEIGHT);
		
		ImageIcon playerIcon = resLoader.getImageIcon("f18.png");
		int startX = currentLevel.getWidth() / 2;
		int startY = 100;
		direction = 0;
		altitude = 50;
		Aircraft playerCraft = new Aircraft(playerIcon, startX, startY, direction, altitude);
		playerCraft.setSpeed(1);
		List<Weapon> basicWeaponSetup = new ArrayList<>();
		List<Weapon> otherWeaponSetup = new ArrayList<>();
		basicWeaponSetup.add(singleShot);
		otherWeaponSetup.add(splitShot);
		playerCraft.setWeaponSetA(basicWeaponSetup);
		playerCraft.setWeaponSetB(otherWeaponSetup);
		player = new Player(playerCraft);
		
		allEffects = new ArrayList<>();
		allGroundVehicles = new ArrayList<>();
		allBandits = new ArrayList<>();
		allSurfaceBuildings = new ArrayList<>();
		allCoverBuildings = new ArrayList<>();
		
		String name = "airstrip";
		startX = 150;
		startY = 2000;
		direction = 180;
		altitude = 0;
		int hitPoints = 1;
		Airstrip runway = new Airstrip(name, startX, startY, direction, altitude, hitPoints);
		allSurfaceBuildings.add(runway);
		
		String testJetName = "f18";
		Deque<Bandit> hangarAircraft = new LinkedList<>();
		startX = 0;
		startY = 0;
		direction = 0;
		altitude = 0;
		int speed = 0;
		hitPoints = 1;
		Bandit bandit1 = new Bandit(testJetName, startX, startY, direction, altitude, 
				speed, hitPoints);
		Bandit bandit2 = new Bandit(testJetName, startX, startY, direction, altitude, 
				speed, hitPoints);
		
		bandit1.setWeaponSetA(basicWeaponSetup);
		List<Bandit> formationMembers = new ArrayList<>();
		formationMembers.add(bandit1);
		//formationMembers.add(bandit2);
		
		String formationType = "line";
		Formation formation = new Formation(formationType, formationMembers);
		
		hangarAircraft.push(bandit1);
		//hangarAircraft.push(bandit2);
		String hangarName = "hangar";
		startX = 60;
		startY = 2150;
		direction = 90;
		altitude = 0;
		hitPoints = 10;
		Hangar hangar = new Hangar(hangarName, startX, startY, direction, altitude, hitPoints, hangarAircraft, this);
		Queue<Point> patrolPath = new LinkedList<>();
		patrolPath.offer(new Point(150, 800));
		patrolPath.offer(new Point(50, 600));
		hangar.setPatrolPath(patrolPath);
		hangar.setNearestRunway(runway);
		allCoverBuildings.add(hangar);
		
/*		ImageIcon tankIcon = resLoader.getImageIcon("tank-body.png");
		Vehicle tank = new Vehicle(tankIcon, currentLevel.getWidth() / 2, 300);
		ImageIcon turretIcon = resLoader.getImageIcon("tank-turret.png");
		Entity turret = new Entity(turretIcon, currentLevel.getWidth() / 2, 300);
		tank.setTurret(turret);
		tank.setDirection(180);
		tank.setFiringDirection(180);
		allGroundVehicles.add(tank);*/
		
		addKeyListener(new KeyActionListener());
		setFocusable(true);
		
		time = new Timer(TIME_INTERVAL, this);
		time.start();
	}
	
	public View getView() {
		return view;
	}
	
	public static Player getPlayer() {
		return player;
	}
	
	public List<Vehicle> getAllGroundVehicles() {
		return allGroundVehicles;
	}

	public Timer getTime() {
		return time;
	}
	
	public void despawn(Vehicle despawnee) {
		for(Iterator<Vehicle> groundIter = allGroundVehicles.iterator(); groundIter.hasNext();) {
			Vehicle aGroundTarget = groundIter.next();
			
			if(aGroundTarget != despawnee) {
				continue;
			}
			
			groundIter.remove();
		}
			
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
		
		List<Building> allBuildings = new ArrayList<>();
		allBuildings.addAll(allCoverBuildings);
		allBuildings.addAll(allSurfaceBuildings);
		
		for(Building aBuilding : allBuildings) {
			aBuilding.spawn();
			aBuilding.takeoffsAndLandings();
		}
		
		for(Vehicle aGroundVehicle : allGroundVehicles) {
			aGroundVehicle.sortie();
			aGroundVehicle.move();
			
			for(Projectile aProjectile : aGroundVehicle.getAllProjectiles()) {
				aProjectile.move();
			}
		}
		
		for(Aircraft aBandit : allBandits) {
			aBandit.move();
			
			for(Projectile aProjectile : aBandit.getAllProjectiles()) {
				aProjectile.move();
			}
		}
		
		for(Effect anEffect : allEffects) {
			anEffect.animate();
		}
		
		for(Projectile aProjectile : player.getPlayerCraft().getAllProjectiles()) {
			aProjectile.move();
		}
		
		repaint();
	}
	
	private void keepPlayerInView() {
		boolean looping = player.getPlayerCraft().getDoLoop();
		if(looping == true) {
			return;
		}
		
		int upperBoundsY = view.getCenterY() + VIEW_HEIGHT / 2;
		int lowerBoundsY = view.getCenterY() - VIEW_HEIGHT / 2;
		
		int playerY = player.getPlayerCraft().getCenterY();
		int altitude = player.getPlayerCraft().getAltitude();
		double scale = windowScale + (double) altitude / MAX_ALTITUDE_SKY;
		int halfPlayerHeight = (int) Math.round(player.getPlayerCraft().getImage().getHeight(null) * scale / 2);
		
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

		int playerX = player.getPlayerCraft().getCenterX();
		int viewX = view.getCenterX();
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

	public void paintComponent(Graphics g) {
		super.paintComponent(g);
		Graphics2D g2d = (Graphics2D) g;
		
		updateBackground(g2d);
		
		detectCollisions();
		
		updateSurfaceBuildings(g2d);
		updateGroundVehicles(g2d);
		updateCoverBuildings(g2d);
		updatePlayerProjectiles(g2d);
		updateAirEnemies(g2d);
		updateEffects(g2d);
		updatePlayer(g2d);
		
		// View bounds
		drawEntity(g2d, view); //debug
	}

	public static boolean checkWithinView(Entity toCheck) {
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
				aBandit.dealDamage(damage);
				if(aBandit.criticalDamage() == true) {
					allEffects.add(aBandit.getExplosionAnimation());
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
				int damage = 5;
				aBandit.dealDamage(damage);
				if(aBandit.criticalDamage() == true) {
					allEffects.add(aBandit.getExplosionAnimation());
					banditIter.remove();
				}
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
		AffineTransform defaultOrientation = g2d.getTransform();
		
		Image image = toDraw.getImage();
		
		int altitude = toDraw.getAltitude();
		double scale = windowScale + (double) altitude / MAX_ALTITUDE_SKY;
		
		int width = image.getWidth(null);
		int height = image.getHeight(null);
		int scaledWidth = (int) Math.round(width * scale);
		int scaledHeight = (int) Math.round(height * scale);
		
		int direction = toDraw.getDirection();
		
		int positionXRelativeToViewCenter = (toDraw.getCenterX() - scaledWidth / 2) - view.getCenterX();
		int positionYRelativeToViewCenter = (toDraw.getCenterY() + scaledHeight / 2) - view.getCenterY();
		
		int absolutePositionX = VIEW_POSITION_X + VIEW_WIDTH / 2 + positionXRelativeToViewCenter;
		int absolutePositionY = VIEW_POSITION_Y + VIEW_HEIGHT / 2 - positionYRelativeToViewCenter;
		
		int rotationPointX = absolutePositionX + scaledWidth / 2;
		int rotationPointY = absolutePositionY + scaledHeight / 2;
		g2d.rotate(Math.toRadians(direction), rotationPointX, rotationPointY);

		//g2d.drawImage(image, absolutePositionX, absolutePositionY, null);
		g2d.drawImage(image, absolutePositionX, absolutePositionY, scaledWidth, scaledHeight, null);
		
		g2d.setTransform(defaultOrientation);
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

	private void updateGroundVehicles(Graphics2D g2d) {
		for(Iterator<Vehicle> groundIter = allGroundVehicles.iterator(); groundIter.hasNext();) {
			Vehicle aGroundTarget = groundIter.next();
			
			for(Iterator<Projectile> bulletIter = aGroundTarget.getAllProjectiles().iterator(); bulletIter.hasNext();) {
				Projectile aProjectile = bulletIter.next();
				
				if(checkWithinView(aProjectile) == false) {
					bulletIter.remove();
					continue;
				}
				
				drawEntity(g2d, aProjectile);
			}
			
			if(checkWithinView(aGroundTarget) == false) {
				continue;
			}
			
			drawEntity(g2d, aGroundTarget);
			
			Entity turret = aGroundTarget.getTurret();
			if(turret == null) {
				continue;
			}
			
			drawEntity(g2d, turret);
		}
	}
	
	private void updateSurfaceBuildings(Graphics2D g2d) {
		for(Iterator<Building> buildingIter = allSurfaceBuildings.iterator(); buildingIter.hasNext();) {
			Building aBuilding = buildingIter.next();
			
			if(checkWithinView(aBuilding) == false) {
				continue;
			}
			
			drawEntity(g2d, aBuilding);
		}
	}
	
	private void updateCoverBuildings(Graphics2D g2d) {
		for(Iterator<Building> buildingIter = allCoverBuildings.iterator(); buildingIter.hasNext();) {
			Building aBuilding = buildingIter.next();
			
			if(checkWithinView(aBuilding) == false) {
				continue;
			}
			
			drawEntity(g2d, aBuilding);
		}
	}
	
	private void updateAirEnemies(Graphics2D g2d) {
		allBandits.addAll(currentLevel.spawnLine(player.getPlayerCraft().getCenterY()));
		
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
	
	private void updateEffects(Graphics2D g2d) {
		for(Iterator<Effect> effectIter = allEffects.iterator(); effectIter.hasNext();) {
			Effect anEffect = effectIter.next();
			
			if(anEffect.getAnimationOver() == true) {
				continue;
			}
			
			drawEntity(g2d, anEffect);
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
	
	public View (ImageIcon icon, int startX, int startY, int inDirection, int inAltitude) {
		super(icon, startX, startY, inDirection, inAltitude);
	}
	
	public boolean checkWithinBounds(Entity toCheck) {
		Rectangle viewBox = getBounds();
		return toCheck.getBounds().intersects(viewBox);
	}
}

class Level {
	
	private static final int CELL_HEIGHT = 16;
	private static final int CELL_WIDTH = 16;
	
	private List<Entity> allSectors = new ArrayList<>();
	private List<String> spawnLines = new ArrayList<>();
	private boolean[] spawnedLines;
	private int BACKGROUND_HEIGHT;
	private int BACKGROUND_WIDTH;
	
	public Level (String levelName) {
		try(BufferedReader br = new BufferedReader(new FileReader("levels/" + levelName + ".lvl"))) {
			for(String line; (line = br.readLine()) != null; ) {
				spawnLines.add(line);
			}
		} catch (IOException e) {
			System.out.println("Level load error: " + e);
		}
		
		Collections.reverse(spawnLines);
		spawnedLines = new boolean[spawnLines.size()];
		
		
		ImageIcon backgroundIcon = resLoader.getImageIcon("Germany1.png");
		Image background = backgroundIcon.getImage();
		
		BACKGROUND_HEIGHT = background.getHeight(null);
		BACKGROUND_WIDTH = background.getWidth(null);
		int startPositionX = BACKGROUND_WIDTH / 2;
		int startPositionY = BACKGROUND_HEIGHT / 2;
		int direction = 0;
		int altitude = 0;
		
		for(int i = 1; i < 9; i++) {
			backgroundIcon = resLoader.getImageIcon("Germany" + i + ".png");
			Entity sector = new Entity(backgroundIcon, startPositionX, startPositionY, direction, altitude);
			allSectors.add(sector);
			startPositionY += backgroundIcon.getIconHeight();
		}
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
						int y = VIEW_HEIGHT + playerY;
						int direction = 180;
						int altitude = 50;
						Aircraft bandit = new Aircraft(banditIcon, x, y, direction, altitude);
						//System.out.println("Spawning basic bandit at: " + bandit.getX() + ", " + bandit.getY());
						//bandit.setSpeed(-1);
						bandit.setSpeed(1);
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
	
	public int getWidth() {
		return BACKGROUND_WIDTH;
	}

	public int getHeight() {
		return BACKGROUND_HEIGHT;
	}
}
