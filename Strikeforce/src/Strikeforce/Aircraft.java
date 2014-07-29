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
	
	private final int MAX_AIRSPEED = 5;
	private final int STALL_SPEED = 1;
	
	protected int bank = 0;
	protected boolean doLoop = false;
	protected int loop = 0;

	public Aircraft(ImageIcon icon) {
		super(icon);
		
		imageLevelFlight = icon.getImage();
		
		ImageIcon resourceIcon = resLoader.getImageIcon("f18-bankleft.png");
		imageBankLeft = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-banklefthard.png");
		imageBankLeftHard = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-bankright.png");
		imageBankRight = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-bankrighthard.png");
		imageBankRightHard = resourceIcon.getImage();
		
		resourceIcon = resLoader.getImageIcon("f18-boostlevel.png");
		boostLevelFlight = resourceIcon.getImage();
		
		resourceIcon = resLoader.getImageIcon("f18-boostbankleft.png");
		imageBankLeft = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-boostbanklefthard.png");
		imageBankLeftHard = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-boostbankright.png");
		imageBankRight = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-boostbankrighthard.png");
		imageBankRightHard = resourceIcon.getImage();
		
		for(int i = 1; i < 16; i++) {
			resourceIcon = resLoader.getImageIcon("f18-loop" + i + ".png");
			loopImages.add(resourceIcon.getImage());
		}
		
		speed = 1;
		allProjectiles = new ArrayList<>();
	}
	
	public Aircraft(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
		
		imageLevelFlight = icon.getImage();
		
		ImageIcon resourceIcon = resLoader.getImageIcon("f18-bankleft.png");
		imageBankLeft = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-banklefthard.png");
		imageBankLeft = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-bankright.png");
		imageBankLeft = resourceIcon.getImage();

		resourceIcon = resLoader.getImageIcon("f18-bankrighthard.png");
		imageBankLeft = resourceIcon.getImage();
		
		speed = 1;
		allProjectiles = new ArrayList<>();
	}
	
	@Override
	public Image getImage() {
		//System.out.println(bank); //debug
		
		if(bank == 0) {
			currentImage = imageLevelFlight;
		}
		
		if(bank > 0) {
			currentImage = imageBankRight;
		}
		
		if(bank > HARD_BANK_ANGLE) {
			currentImage = imageBankRightHard;
		}
		
		if(bank < 0) {
			currentImage = imageBankLeft;
		}
		
		if(bank < -HARD_BANK_ANGLE) {
			currentImage = imageBankLeftHard;
		}
		
		if(doLoop == true) {
			currentImage = currentLoopImage;
		}
		
		return currentImage;
	}
	
	@Override
	public void move() {
		if(doLoop == true) {
			doLoop();
			return;
		}
		
		super.move();
				
		if(x < LOWER_BOUNDS_X) {
			x = LOWER_BOUNDS_X;
		}
		if(x > UPPER_BOUNDS_X) {
			x = UPPER_BOUNDS_X;
		}

		if(y < LOWER_BOUNDS_Y) {
			y = LOWER_BOUNDS_Y;
		}
		if(y > UPPER_BOUNDS_Y) {
			y = UPPER_BOUNDS_Y;
		}
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
			y -= 3;
			currentLoopImage = boostLevelFlight;
			break;
		case 2:
			y += 7;
			break;
		case 4:
			y += 16;
			currentLoopImage = loopImages.get(0);
			break;
		case 6:
			y += 5;
			break;
		case 8:
			y += 5;
			break;
		case 10:
			y += 9;
			currentLoopImage = loopImages.get(1);
			break;
		case 12:
			y += 4;
			currentLoopImage = loopImages.get(2);
			break;
		case 14:
			y -= 9;
			currentLoopImage = loopImages.get(3);
			break;
		case 16:
			y -= 20;
			currentLoopImage = loopImages.get(4);
			break;
		case 18:
			y -= 35;
			currentLoopImage = loopImages.get(5);
			break;
		case 20:
			y -= 20;
			currentLoopImage = loopImages.get(6);
			break;
		case 22:
			y -= 9;
			break;
		case 24:
			y -= 17;
			currentLoopImage = loopImages.get(8);
			break;
		case 26:
			y += 11;
			currentLoopImage = loopImages.get(9);
			break;
		case 28:
			y += 12;
			currentLoopImage = loopImages.get(10);
			break;
		case 30:
			y += 23;
			currentLoopImage = loopImages.get(11);
			break;
		case 32:
			y += 3;
			break;
		case 34:
			y += 10;
			currentLoopImage = loopImages.get(12);
			break;
		case 36:
			y -= 1;
			currentLoopImage = loopImages.get(13);
			break;
		case 38:
			y += 4;
			break;
		case 40:
			y -= 1;
			currentLoopImage = loopImages.get(14);
			break;
		case 42:
			y += 4;
			break;
		case 44:
			y += 3;
			currentLoopImage = imageLevelFlight;
			break;
		case 46:
			doLoop = false;
			invulnerable = false;
			//speed--;
			return;
		}
		loop++;
	}

	@Override
	public void accelerate() {
		speed++;
		
		if(speed > MAX_AIRSPEED) {
			speed = MAX_AIRSPEED;
		}
	}
	
	@Override
	public void decelerate() {
		speed--;
		
		if(speed > STALL_SPEED) {
			speed = STALL_SPEED;
		}
	}
	
	public void bankLeft() {
		dx = -speed;
		
		if(bank == -MAX_BANK_ANGLE) {
			return;
		}
		
		bank--;
	}
	
	public void bankRight() {
		dx = speed;
		
		if(bank == MAX_BANK_ANGLE) {
			return;
		}
		
		bank++;
	}
	
	public void levelOff() {
		dx = 0;
		bank = 0;
	}
	
	public void openFire() {		
		ImageIcon bulletIcon = resLoader.getImageIcon("bullet.png");
		int startX = getX();
		int startY = getY() + bulletIcon.getIconHeight();
		
		Projectile aBullet = new Projectile(bulletIcon, startX, startY);
		allProjectiles.add(aBullet);
	}
}
