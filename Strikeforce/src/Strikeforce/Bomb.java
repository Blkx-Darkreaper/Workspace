package Strikeforce;

import static Strikeforce.Global.EXPLOSION_ANIMATION_FRAMES;
import static Strikeforce.Global.chooseExplosionAnimation;
import static Strikeforce.Global.resLoader;

import java.awt.Image;
import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

import Strikeforce.Entity.Circle;

public class Bomb extends Projectile {
	
	protected List<Image> animationImages = new ArrayList<>();
	protected boolean animationOver = false;
	protected int count = 0;
	protected int frameSpeed;
	protected int fuseDelay;
	protected boolean falls;
	protected String blastRadius;

	public Bomb(String inName, int inX, int inY, int inDirection,
			int inAltitude, int inSpeed, int inDamage, boolean inHitsAir, boolean inHitsGround, boolean inLive, 
			int inFuseDelay, boolean inFalls, String inBlastRadius, int frames, int inFrameSpeed) {
		super(inName + "1", inX, inY, inDirection, inAltitude, inSpeed, inDamage, inHitsAir, inHitsGround, inLive);
		
		for(int i = 2; i <= frames; i++) {
			ImageIcon icon = resLoader.getImageIcon(inName + i + ".png");
			Image image = icon.getImage();
			animationImages.add(image);
		}
		frameSpeed = inFrameSpeed;
		fuseDelay = inFuseDelay;
		falls = inFalls;
		blastRadius = inBlastRadius;
	}
	
	public int getFuseDelay() {
		return fuseDelay;
	}
	
	public boolean getFalls() {
		return falls;
	}
	
	@Override
	public Effect getExplosionAnimation() {
		
		String animationName = chooseExplosionAnimation(blastRadius);
		int frameSpeed = 2;
		Effect explosion = new Effect(animationName, centerX, centerY, direction, altitude, 
				EXPLOSION_ANIMATION_FRAMES, frameSpeed, damage, hitsAir, hitsGround);
		return explosion;
	}
	
	public void animate() {
		if(animationOver == true) {
			return;
		}
		
		int frame = count / frameSpeed;
		
		currentImage = animationImages.get(frame);
		count++;
		
		if(frame == (animationImages.size() - 1)) {
			animationOver = true;
		}
	}
	
	@Override
	public void update() {
		animate();
		
		fuseDelay--;
		
		if(falls == true) {
			fall();
		}
		
		super.update();
		
		detonate();
	}
	
	public void fall() {
		altitude--;
		
		if(altitude < 0) {
			altitude = 0;
		}
	}
	
	public void detonate() {
		if(fuseDelay > 0) {
			return;
		}
		
		detonate = true;
		live = true;
	}
}
