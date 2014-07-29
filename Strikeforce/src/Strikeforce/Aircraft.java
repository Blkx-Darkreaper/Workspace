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
	private List<Image> loopImages = new ArrayList<>();
	
	private final int MAX_AIRSPEED = 5;
	private final int STALL_SPEED = 1;
	
	protected int bank = 0;
	protected boolean doLoop = false;
	protected int loop = 0;
	protected int loopSpeed = 4;

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
		
		for(int i = 1; i < 23; i++) {
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
		
		if(loop > 0) {
			Image loopImage = loopImages.get(loop / loopSpeed - 1);
			
			currentImage = loopImage;
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
			doLoop = true;
			invulnerable = true;
			return;
		}
		
		loop++;
		
		if(loop > 20*loopSpeed) {
			loop = 0;
			doLoop = false;
			invulnerable = false;
		}
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
