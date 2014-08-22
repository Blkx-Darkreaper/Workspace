package Strikeforce;

import java.awt.Image;
import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

import static Strikeforce.Global.*;

public class Aircraft extends Vehicle {

	private Image imageLevelFlight;
	private Image imageBankLeft;
	private Image imageBankLeftHard;
	private Image imageBankRight;
	private Image imageBankRightHard;
	
	private Image boostLevelFlight;
	private Image boostBankLeft;
	private Image boostBankLeftHard;
	private Image boostBankRight;
	private Image boostBankRightHard;
	
	private Image currentLoopImage;
	private List<Image> loopImages = new ArrayList<>();
	
	protected final int CRUISING_SPEED = 2;
	protected final int STALL_SPEED = 1;
	protected int climbRate = 1;
	protected final int MAX_ALTITUDE = 100;
	protected final int CRUISING_ALTITUDE = 50;
	protected final int BOOST_DURATION = 20;
	
	protected int roll = 0;
	protected boolean doLoop = false;
	protected int loop = 0;
	protected boolean boosting = false;
	protected int boost = 0;

	public Aircraft(ImageIcon icon, int startX, int startY, int inDirection, int inAltitude) {
		super(icon, startX, startY, inDirection, inAltitude);
		
		imageLevelFlight = icon.getImage();
		
		ImageIcon resourceIcon = resLoader.getImageIcon("f18-bankleft.png");
		imageBankLeft = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-banklefthard.png");
		imageBankLeftHard = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-bankright.png");
		imageBankRight = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-bankrighthard.png");
		imageBankRightHard = resourceIcon.getImage();
		
		resourceIcon = resLoader.getImageIcon("f18-boost.png");
		boostLevelFlight = resourceIcon.getImage();
		
		resourceIcon = resLoader.getImageIcon("f18-boostbankleft.png");
		boostBankLeft = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-boostbanklefthard.png");
		boostBankLeftHard = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-boostbankright.png");
		boostBankRight = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-boostbankrighthard.png");
		boostBankRightHard = resourceIcon.getImage();
		
		for(int i = 1; i < 16; i++) {
			resourceIcon = resLoader.getImageIcon("f18-loop" + i + ".png");
			loopImages.add(resourceIcon.getImage());
		}
		
		speed = 1;
		updateVectors();
	}
	
	public Aircraft(String inName, int inX, int inY, int inDirection, int inAltitude, int inSpeed, int inHitPoints) {
		super(inName, inX, inY, inDirection, inAltitude, inSpeed, inHitPoints);
		String extension = "png";
		
		ImageIcon icon = resLoader.getImageIcon(inName + "." + extension);
		imageLevelFlight = icon.getImage();
		
		icon = resLoader.getImageIcon(inName + "-bankleft" + "." + extension);
		imageBankLeft = icon.getImage();
		
		icon = resLoader.getImageIcon(inName + "-banklefthard" + "." + extension);
		imageBankLeftHard = icon.getImage();

		icon = resLoader.getImageIcon(inName + "-bankright" + "." + extension);
		imageBankRight = icon.getImage();

		icon = resLoader.getImageIcon(inName + "-bankrighthard" + "." + extension);
		imageBankRightHard = icon.getImage();
		
		icon = resLoader.getImageIcon(inName + "-boost" + "." + extension);
		boostLevelFlight = icon.getImage();
		
		icon = resLoader.getImageIcon(inName + "-boostbankleft" + "." + extension);
		boostBankLeft = icon.getImage();

		icon = resLoader.getImageIcon(inName + "-boostbanklefthard" + "." + extension);
		boostBankLeftHard = icon.getImage();

		icon = resLoader.getImageIcon(inName + "-boostbankright" + "." + extension);
		boostBankRight = icon.getImage();

		icon = resLoader.getImageIcon(inName + "-boostbankrighthard" + "." + extension);
		boostBankRightHard = icon.getImage();
		
		for(int i = 1; i < 16; i++) {
			icon = resLoader.getImageIcon(inName + "-loop" + i + "." + extension);
			loopImages.add(icon.getImage());
		}
		
		turnSpeed = 5;
		MAX_SPEED = 5;
	}
	
	@Override
	public Image getImage() {
		//System.out.println(bank); //debug
		
		if(roll == 0) {
			currentImage = imageLevelFlight;
			if(boosting == true) {
				currentImage = boostLevelFlight;
			}
		}
		
		if(roll > 0) {
			currentImage = imageBankRight;
			if(boosting == true) {
				currentImage = boostBankRight;
			}
		}
		
		if(roll > HARD_BANK_ANGLE) {
			currentImage = imageBankRightHard;
			if(boosting == true) {
				currentImage = boostBankRightHard;
			}
		}
		
		if(roll < 0) {
			currentImage = imageBankLeft;
			if(boosting == true) {
				currentImage = boostBankLeft;
			}
		}
		
		if(roll < -HARD_BANK_ANGLE) {
			currentImage = imageBankLeftHard;
			if(boosting == true) {
				currentImage = boostBankLeftHard;
			}
		}
		
		if(doLoop == true) {
			currentImage = currentLoopImage;
		}
		
		return currentImage;
	}
	
	public boolean getDoLoop() {
		return doLoop;
	}
	
	@Override
	public void move() {
		if(doLoop == true) {
			doLoop();
		}
		
		if(boosting == true) {
			boost();
		}
		
		super.move();
	}
	
	public void doLoop() {
		if(doLoop == false) {
			if(speed == STALL_SPEED) {
				return;
			}
			
			loop = 0;
			doLoop = true;
			invulnerable = true;
			return;
		}
		
		switch (loop) {
		case 0:
			centerY -= 2;
			currentLoopImage = boostLevelFlight;
			break;
		case 2:
			centerY += 7;
			break;
		case 4:
			centerY += 18;
			currentLoopImage = loopImages.get(0);
			break;
		case 6:
			centerY += 5;
			break;
		case 8:
			centerY += 5;
			break;
		case 10:
			centerY += 7;
			currentLoopImage = loopImages.get(1);
			break;
		case 12:
			centerY -= 6;
			currentLoopImage = loopImages.get(2);
			break;
		case 14:
			centerY -= 5;
			currentLoopImage = loopImages.get(3);
			break;
		case 16:
			centerY -= 1;
			currentLoopImage = loopImages.get(4);
			break;
		case 18:
			centerY -= 19;
			currentLoopImage = loopImages.get(5);
			break;
		case 20:
			centerY -= 6;
			currentLoopImage = loopImages.get(6);
			break;
		case 22:
			centerY -= 9;
			break;
		case 24:
			centerY -= 24;
			currentLoopImage = loopImages.get(8);
			break;
		case 26:
			centerY += 2;
			currentLoopImage = loopImages.get(9);
			break;
		case 28:
			centerY -= 2;
			currentLoopImage = loopImages.get(10);
			break;
		case 30:
			centerY += 3;
			currentLoopImage = loopImages.get(11);
			break;
		case 32:
			centerY += 3;
			break;
		case 34:
			centerY += 9;
			currentLoopImage = loopImages.get(12);
			break;
		case 36:
			centerY += 3;
			currentLoopImage = loopImages.get(13);
			break;
		case 38:
			centerY += 4;
			break;
		case 40:
			centerY += 4;
			currentLoopImage = loopImages.get(14);
			break;
		case 42:
			centerY += 4;
			break;
		case 44:
			currentLoopImage = imageLevelFlight;
			break;
		case 46:
			doLoop = false;
			invulnerable = false;
			speed--;
			return;
		}
		loop++;
	}
	
	public void boost() {
		if(boosting == false) {
			boost = 0;
			boostMultiplier = 1;
			
			int offsetX = (int) (Math.sin(Math.toRadians(direction)) * 3 / 2);
			int offsetY = (int) (Math.cos(Math.toRadians(direction)) * 3 / 2);
			centerX -= offsetX;
			centerY -= offsetY;
			
			boosting = true;
			return;
		}
		
		boost++;
		boostMultiplier = (int) Math.round(boost / REVS_PER_ACCELERATION) + 1;
		
		accelerate(MAX_SPEED);
		
		if(boost < BOOST_DURATION) {
			return;
		}
		
		int offsetX = (int) Math.sin(Math.toRadians(direction)) * 3 / 2;
		int offsetY = (int) Math.cos(Math.toRadians(direction)) * 3 / 2;
		centerX += offsetX;
		centerY += offsetY;
		
		boosting = false;
	}
	
	public void bankLeft() {
		dx = -speed;
		
		if(roll == -MAX_BANK_ANGLE) {
			return;
		}
		
		roll--;
	}
	
	public void bankRight() {
		dx = speed;
		
		if(roll == MAX_BANK_ANGLE) {
			return;
		}
		
		roll++;
	}
	
	public void levelOff() {
		roll = 0;
		updateVectors();
	}
	
	public void moveUp() {
		dy = speed * 2;
	}
	
	public void moveDown() {
		dy = 0;
	}
	
	public void cruise() {
		updateVectors();
	}
	
	@Override
	public void turnLeft(int desiredTurn) {
		for(int i = 0; i < Math.max(turnSpeed * speed, 1); i++) {
			if(i == desiredTurn) {
				break;
			}
			
			direction--;
		}
		
		direction %= 360;
		
		updateVectors();
	}
	
	@Override
	public void turnRight(int desiredTurn) {
		for(int i = 0; i < Math.max(turnSpeed * speed, 1); i++) {
			if(i == desiredTurn) {
				break;
			}
			
			direction++;
		}
		
		direction %= 360;
		
		updateVectors();
	}
	
	public void climb() {
		for(int i = 0; i < climbRate; i++) {
			altitude++;
		}
		
		if(altitude > MAX_ALTITUDE) {
			altitude = MAX_ALTITUDE;
		}
	}
	
	public void dive() {
		altitude--;
		
		if(altitude < 0) {
			altitude = 0;
		}
	}

	public void setY(int position) {
		centerY = position;
	}

	public Effect getExplosionAnimation() {
		String animationName = chooseExplosionAnimation();
		int frameSpeed = 2;
		Effect explosion = new Effect(animationName, centerX, centerY, direction, altitude, 
				EXPLOSION_ANIMATION_FRAMES, frameSpeed);
		return explosion;
	}
}
