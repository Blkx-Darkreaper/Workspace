package Strikeforce;

import java.awt.Image;
import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;

public class Effect extends Entity {
	
	private List<Image> animationImages = new ArrayList<>();
	private int frameSpeed;
	private int count = 0;
	private boolean animationOver = false;

	public Effect(ImageIcon icon, int startX, int startY, List<Image> inImages, int inFrameSpeed) {
		super(icon, startX, startY);
		animationImages = inImages;
		frameSpeed = inFrameSpeed;
	}
	
	public boolean getAnimationOver() {
		return animationOver;
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
}
