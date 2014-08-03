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

	public Aircraft(ImageIcon icon, int startingX, int startingY) {
		super(icon, startingX, startingY);
		
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
		dy = speed;
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
		
		halfWidth = currentImage.getWidth(null) / 2;
		halfHeight = currentImage.getHeight(null) / 2;
		
		return currentImage;
	}
	
	@Override
	public void move() {
		if(doLoop == true) {
			doLoop();
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
		
		updateVectors();
	}
	
	@Override
	public void decelerate() {
		speed--;
		
		if(speed > STALL_SPEED) {
			speed = STALL_SPEED;
		}
		
		updateVectors();
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
		bank = 0;
		updateVectors();
	}
	
	public void moveUp() {
		dy = speed + 1;
	}
	
	public void moveDown() {
		dy = -speed;
	}
	
	public void cruise() {
		updateVectors();
	}

	public void setY(int position) {
		centerY = position;
	}
}
