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

	private String currentPhase = "Planning";
	private int windowScale = 1;
	private static View view;
	static Level currentLevel;
	private static Fighter playerFighter;
	private static Cursor playerCursor;
	private List<Effect> allEffects;
	private List<Vehicle> allVehicles;
	private static List<Building> allBuildings;
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
		
		String name = "cursor";
		int cursorX = CELL_SIZE / 2;
		int cursorY = CELL_SIZE / 2;
		int levelTop = LEVEL_HEIGHT;
		playerCursor = new Cursor(name, cursorX, cursorY, levelTop);
		
		String playerName = "f18";
		int startX = currentLevel.getWidth() / 2;
		int startY = 100;
		direction = 0;
		altitude = 50;
		int speed = 1;
		int hitPoints = 1;
		playerFighter = new Fighter(playerName, startX, startY, direction, altitude, speed, hitPoints);
		List<Weapon> basicWeaponSetup = new ArrayList<>();
		List<Weapon> otherWeaponSetup = new ArrayList<>();
		basicWeaponSetup.add(singleShot);
		basicWeaponSetup.add(splitShot);
		otherWeaponSetup.add(dumbBomb);
		playerFighter.setWeaponSetA(basicWeaponSetup);
		playerFighter.setWeaponSetB(otherWeaponSetup);
		
		allEffects = new ArrayList<>();
		allVehicles = new ArrayList<>();
		allBuildings = new ArrayList<>();
		
		name = "airstrip";
		startX = 150;
		startY = 2000;
		direction = 180;
		altitude = 0;
		hitPoints = 1;
		Airstrip runway = new Airstrip(name, startX, startY, direction, altitude, hitPoints);
		allBuildings.add(runway);
		
		String testJetName = "f18";
		Deque<Bandit> hangarAircraft = new LinkedList<>();
		startX = 0;
		startY = 0;
		direction = 0;
		altitude = 0;
		speed = 0;
		hitPoints = 1;
		Bandit bandit1 = new Bandit(testJetName, startX, startY, direction, altitude, 
				speed, hitPoints);
		Bandit bandit2 = new Bandit(testJetName, startX, startY, direction, altitude, 
				speed, hitPoints);
		
		bandit1.setWeaponSetA(basicWeaponSetup);
		List<Bandit> formationMembers = new ArrayList<>();
		formationMembers.add(bandit1);
		formationMembers.add(bandit2);
		
		String formationType = "line";
		Formation formation = new Formation(formationType, formationMembers);
		bandit1.setFormation(formation);
		bandit2.setFormation(formation);
		
		hangarAircraft.push(bandit1);
		hangarAircraft.push(bandit2);
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
		hangar.setCovers(true);
		allBuildings.add(hangar);
		
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
	
	public String getPhase () {
		return currentPhase;
	}
	
	public void setPhase(String inPhase) {
		currentPhase = inPhase;
	}
	
	public View getView() {
		return view;
	}
	
	public static Fighter getPlayer() {
		return playerFighter;
	}
	
	public List<Vehicle> getAllVehicles() {
		return allVehicles;
	}

	public Timer getTime() {
		return time;
	}
	
	public void despawn(Vehicle despawnee) {
		for(Iterator<Vehicle> vehicleIter = allVehicles.iterator(); vehicleIter.hasNext();) {
			Vehicle aVehicle = vehicleIter.next();
			
			if(aVehicle != despawnee) {
				continue;
			}
			
			vehicleIter.remove();
		}
			
	}

	public void actionPerformed(ActionEvent e) {
		switch(currentPhase) {
		case "Planning":
			planningMode();
			break;
		case "Build":
			break;
		case "Raid":
			raidMode();
			break;
		}
		
		repaint();
	}
	
	private void planningMode() {
		view.update();
		playerCursor.update();
		keepPlayerInView();
	}
	
	private void raidMode() {
		setViewScrollRate();
		setViewPanRate();
		view.update();
		
		playerFighter.update();
		keepPlayerInView();
		//System.out.println("X: " + player.getPlayerCraft().getX() + ", Y: " + player.getPlayerCraft().getY()); //debug
		
		for(Building aBuilding : allBuildings) {
			aBuilding.spawn();
			aBuilding.takeoffsAndLandings();
		}
		
		for(Vehicle aVehicle : allVehicles) {
			aVehicle.sortie();
			aVehicle.update();
			
			for(Iterator<Projectile> projectileIter = aVehicle.getAllProjectiles().iterator(); projectileIter.hasNext();) {
				Projectile aProjectile = projectileIter.next();
				aProjectile.update();
				
				boolean detonate = aProjectile.getDetonate();
				if(detonate == false) {
					continue;
				}
				
				allEffects.add(aProjectile.getExplosionAnimation());
				projectileIter.remove();
			}
		}
		
		for(Iterator<Effect> effectIter = allEffects.iterator(); effectIter.hasNext();) {
			Effect anEffect = effectIter.next();
			anEffect.animate();
			
			boolean animationOver = anEffect.getAnimationOver();
			if(animationOver == false) {
				continue;
			}
			
			effectIter.remove();
		}
		
		for(Iterator<Projectile> projectileIter = playerFighter.getAllProjectiles().iterator(); projectileIter.hasNext();) {
			Projectile aProjectile = projectileIter.next();
			aProjectile.update();
			
			boolean detonate = aProjectile.getDetonate();
			if(detonate == false) {
				continue;
			}
			
			allEffects.add(aProjectile.getExplosionAnimation());
			projectileIter.remove();
		}
	}
	
	private void keepPlayerInView() {
		int upperBoundsY;
		int lowerBoundsY;
		
		int playerX;
		int playerY;
		
		switch (currentPhase) {
		case "Raid":
			boolean looping = playerFighter.getLooping();
			if(looping == true) {
				return;
			}
			
			upperBoundsY = view.getCenterY() + VIEW_HEIGHT / 2;
			lowerBoundsY = view.getCenterY() - VIEW_HEIGHT / 2;
			
			playerY = playerFighter.getCenterY();
			int altitude = playerFighter.getAltitude();
			double scale = windowScale + (double) altitude / MAX_ALTITUDE_SKY;
			int halfPlayerHeight = (int) Math.round(playerFighter.getImage().getHeight(null) * scale / 2);
			
			if((playerY - halfPlayerHeight) < lowerBoundsY) {
				playerFighter.setY(lowerBoundsY + halfPlayerHeight);
			}
			
			if((playerY + halfPlayerHeight) > upperBoundsY) {
				playerFighter.setY(upperBoundsY - halfPlayerHeight);
			}
			break;
			
		case "Planning":
			upperBoundsY = view.getCenterY() + VIEW_HEIGHT / 2;
			lowerBoundsY = view.getCenterY() - VIEW_HEIGHT / 2;
			
			playerX = playerCursor.getCenterX();
			playerY = playerCursor.getCenterY();
			
			if((playerY - CELL_SIZE / 2) < lowerBoundsY) {
				view.moveDown();
			}
			
			if((playerY + CELL_SIZE / 2) > upperBoundsY) {
				view.moveUp();
			}
			break;
		}
	}

	private void setViewPanRate() {
		int panRate = playerFighter.getSpeed();
		int panDirection = 0;

		int playerX = playerFighter.getCenterX();
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
		int scrollRate = playerFighter.getSpeed();
		view.setSpeed(scrollRate);
	}

	public void paintComponent(Graphics g) {
		super.paintComponent(g);
		Graphics2D g2d = (Graphics2D) g;
		
		updateBackground(g2d);
		
		detectCollisions();
		
		updateBuildings(g2d);
		updateGroundVehicles(g2d);
		updateBuildingCover(g2d);
		updatePlayerProjectiles(g2d);
		updateAirEnemies(g2d);
		updatePlayer(g2d);
		updateEffects(g2d);
		
		// View bounds
		drawEntity(g2d, view); //debug
	}

	public static boolean checkWithinView(Entity toCheck) {
		return view.checkWithinBounds(toCheck);
	}
	
	private void detectCollisions () {
		Aircraft playerCraft = playerFighter;
		List<Projectile> allFriendlyProjectiles = playerCraft.getAllProjectiles();
		for(Iterator<Vehicle> banditIter = allVehicles.iterator(); banditIter.hasNext();) {
			Vehicle aBandit = banditIter.next();
			boolean airborne = aBandit.getAirborne();

			for(Iterator<Projectile> projectileIter = allFriendlyProjectiles.iterator(); projectileIter.hasNext();) {
				Projectile aProjectile = projectileIter.next();
				
				boolean live = aProjectile.getLive();
				if(live == false) {
					continue;
				}
				
				boolean hitsGround = aProjectile.getHitsGround();
				boolean hitsAir = aProjectile.getHitsAir();
				if(airborne == true) {
					
					if(hitsAir == false) {
						continue;
					}

				} else {
					
					if(hitsGround == false) {
						continue;
					}
				}
				
				boolean collision = aBandit.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				int damage = aProjectile.getDamage();
				aBandit.dealDamage(damage);
				if(aBandit.criticalDamage() == true) {
					allEffects.add(aBandit.getExplosionAnimation());
					
					if(airborne == true) {
						banditIter.remove();
					}
				}
				projectileIter.remove();
			}
			
			List<Effect> effectsToAdd = new ArrayList<>();
			
			for(Iterator<Effect> effectIter = allEffects.iterator(); effectIter.hasNext();) {
				Effect anEffect = effectIter.next();
				
				boolean hitsAir = anEffect.getHitsAir();
				boolean hitsGround = anEffect.getHitsGround();
				if(airborne == true) {
					
					if(hitsAir == false) {
						continue;
					}

				} else {
					
					if(hitsGround == false) {
						continue;
					}
				}
				
				boolean collision = aBandit.checkForCollision(anEffect);
				
				if(collision == false) {
					continue;
				}
				
				int damage = anEffect.getDamage();
				aBandit.dealDamage(damage);
				if(aBandit.criticalDamage() == true) {
					effectsToAdd.add(aBandit.getExplosionAnimation());
					
					if(airborne == true) {
						banditIter.remove();
					}
				}
			}
			
			allEffects.addAll(effectsToAdd);
			
			if(playerCraft.getInvulnerable() == true) {
				continue;
			}
			
			List<Projectile> allEnemyProjectiles = aBandit.allProjectiles;
			
			for(Iterator<Projectile> projectileIter = allEnemyProjectiles.iterator(); projectileIter.hasNext();) {
				Projectile aProjectile = projectileIter.next();

				boolean collision = playerCraft.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				int damage = aProjectile.getDamage();
				playerFighter.dealDamage(damage);
				if(playerFighter.criticalDamage() == true) {
					allEffects.add(playerFighter.getExplosionAnimation());
					//gameover();
					return;
				}
				projectileIter.remove();
			}
			
			if(airborne == false) {
				continue;
			}
			
			boolean collision = playerCraft.checkForCollision(aBandit);
			
			if(collision == true) {
				int damage = 5;
				aBandit.dealDamage(damage);
				if(aBandit.criticalDamage() == true) {
					allEffects.add(aBandit.getExplosionAnimation());
					
					if(airborne == true) {
						banditIter.remove();
					}
				}
				//gameover();
				return;
			}
		}
		
		for(Iterator<Building> buildingIter = allBuildings.iterator(); buildingIter.hasNext();) {
			Building aBuilding = buildingIter.next();

			for(Iterator<Projectile> projectileIter = allFriendlyProjectiles.iterator(); projectileIter.hasNext();) {
				Projectile aProjectile = projectileIter.next();
				
				boolean live = aProjectile.getLive();
				if(live == false) {
					continue;
				}
				
				boolean hitsGround = aProjectile.getHitsGround();
				if(hitsGround == false) {
					continue;
				}
				
				boolean collision = aBuilding.checkForCollision(aProjectile);
				
				if(collision == false) {
					continue;
				}
				
				int damage = aProjectile.getDamage();
				aBuilding.dealDamage(damage);
				if(aBuilding.criticalDamage() == true) {
					allEffects.add(aBuilding.getExplosionAnimation());
				}
				projectileIter.remove();
			}
			
			List<Effect> effectsToAdd = new ArrayList<>();
			
			for(Iterator<Effect> effectIter = allEffects.iterator(); effectIter.hasNext();) {
				Effect anEffect = effectIter.next();
				
				boolean hitsGround = anEffect.getHitsGround();
				if(hitsGround == false) {
					continue;
				}
				
				boolean collision = aBuilding.checkForCollision(anEffect);
				
				if(collision == false) {
					continue;
				}
				
				int damage = anEffect.getDamage();
				aBuilding.dealDamage(damage);
				if(aBuilding.criticalDamage() == true) {
					effectsToAdd.add(aBuilding.getExplosionAnimation());
				}
			}
			
			allEffects.addAll(effectsToAdd);
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
		if(currentPhase != "Raid") {
			return;
		}
		
		for(Iterator<Projectile> bulletIter = playerFighter.getAllProjectiles().iterator(); 
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
		switch (currentPhase) {
		case "Planning":
			drawEntity(g2d, playerCursor);
			break;
		case "Build":
			break;
		case "Raid":
			drawEntity(g2d, playerFighter);
			break;
		}
	}

	private void updateGroundVehicles(Graphics2D g2d) {
		for(Iterator<Vehicle> groundIter = allVehicles.iterator(); groundIter.hasNext();) {
			Vehicle aVehicle = groundIter.next();
			boolean airborne = aVehicle.getAirborne();
			
			if(airborne == true) {
				continue;
			}
			
			for(Iterator<Projectile> bulletIter = aVehicle.getAllProjectiles().iterator(); bulletIter.hasNext();) {
				Projectile aProjectile = bulletIter.next();
				
				if(checkWithinView(aProjectile) == false) {
					bulletIter.remove();
					continue;
				}
				
				drawEntity(g2d, aProjectile);
			}
			
			if(checkWithinView(aVehicle) == false) {
				continue;
			}
			
			drawEntity(g2d, aVehicle);
			
			Entity turret = aVehicle.getTurret();
			if(turret == null) {
				continue;
			}
			
			drawEntity(g2d, turret);
		}
	}
	
	private void updateBuildings(Graphics2D g2d) {
		for(Iterator<Building> buildingIter = allBuildings.iterator(); buildingIter.hasNext();) {
			Building aBuilding = buildingIter.next();
			boolean cover = aBuilding.getCovers();
			
			if(cover == true) {
				continue;
			}
			
			if(checkWithinView(aBuilding) == false) {
				continue;
			}
			
			drawEntity(g2d, aBuilding);
		}
	}
	
	private void updateBuildingCover(Graphics2D g2d) {
		for(Iterator<Building> buildingIter = allBuildings.iterator(); buildingIter.hasNext();) {
			Building aBuilding = buildingIter.next();
			boolean cover = aBuilding.getCovers();
			
			if(cover == false) {
				continue;
			}
			
			if(checkWithinView(aBuilding) == false) {
				continue;
			}
			
			drawEntity(g2d, aBuilding);
		}
	}
	
	private void updateAirEnemies(Graphics2D g2d) {
		if(currentPhase != "Raid") {
			return;
		}
		
		allVehicles.addAll(currentLevel.spawnLine(playerFighter.getCenterY()));
		
		for(Iterator<Vehicle> airborneIter = allVehicles.iterator(); airborneIter.hasNext();) {
			Vehicle aBandit = airborneIter.next();
			boolean airborne = aBandit.getAirborne();
			
			if(airborne == false) {
				continue;
			}
		
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
		if(currentPhase != "Raid") {
			return;
		}
		
		for(Iterator<Effect> effectIter = allEffects.iterator(); effectIter.hasNext();) {
			Effect anEffect = effectIter.next();
			
			if(anEffect.getAnimationOver() == true) {
				continue;
			}
			
			drawEntity(g2d, anEffect);
		}
	}

	private void updateBackground(Graphics2D g2d) {
		for(Entity aSector : currentLevel.getAllSectors()) {
			if(checkWithinView(aSector) == false) {
				continue;
			}
			
			drawEntity(g2d, aSector);
		}
	}
	
	public static Building selectBuildingInArea(Rectangle areaToCheck) {
		for(Building aBuilding : allBuildings) {
			Rectangle hitBox = aBuilding.getBounds();
			boolean inArea = areaToCheck.intersects(hitBox);
			
			if(inArea == false) {
				continue;
			}
			
			return aBuilding;
		}
		
		return null;
	}
	
	public void gameover() {
		JOptionPane.showMessageDialog(null, "You have been shot down!", "Game Over", 
				JOptionPane.INFORMATION_MESSAGE);
		playerFighter.gameover();
	}
	
	private class KeyActionListener extends KeyAdapter {
		
		public void keyReleased (KeyEvent e) {
			String phase = getPhase();
			
			switch(phase) {
			case "Planning":
				playerCursor.keyReleased(e);
				break;
			case "Build":
				break;
			case "Raid":
				playerFighter.keyReleased(e);
				break;
			}
		}
		
		public void keyPressed (KeyEvent e) {
			String phase = getPhase();
			
			switch(phase) {
			case "Planning":
				playerCursor.keyPressed(e);
				break;
			case "Build":
				break;
			case "Raid":
				playerFighter.keyPressed(e);
				break;
			}
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
	
	@Override
	public void moveUp() {
		super.moveUp();
		
/*		int topEdge = LEVEL_HEIGHT - VIEW_HEIGHT / 2;
		if(centerY > topEdge) {
			centerY = topEdge;
		}*/
	}
	
	@Override
	public void moveDown() {
		super.moveDown();
		
		int bottomEdge = VIEW_HEIGHT / 2;
		if(centerY < bottomEdge) {
			centerY = bottomEdge;
		}
	}
	
	@Override
	public void moveLeft() {
		super.moveLeft();
		
		int leftEdge = VIEW_WIDTH / 2;
		if(centerX < leftEdge) {
			centerX = leftEdge;
		}
	}
	
	@Override
	public void moveRight() {
		super.moveRight();
		
		int rightEdge = LEVEL_WIDTH - VIEW_WIDTH / 2;
		if(centerX > rightEdge) {
			centerX = rightEdge;
		}
	}
}

class Level {
	
	private static final int CELL_HEIGHT = CELL_SIZE;
	private static final int CELL_WIDTH = CELL_SIZE;
	
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
		
		int levelHeight = BACKGROUND_HEIGHT;
		
		for(int i = 1; i < 9; i++) {
			backgroundIcon = resLoader.getImageIcon("Germany" + i + ".png");
			Entity sector = new Entity(backgroundIcon, startPositionX, startPositionY, direction, altitude);
			allSectors.add(sector);
			startPositionY += backgroundIcon.getIconHeight();
			levelHeight += backgroundIcon.getIconHeight();
		}
		
		LEVEL_WIDTH = BACKGROUND_WIDTH;
		LEVEL_HEIGHT = levelHeight;
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
