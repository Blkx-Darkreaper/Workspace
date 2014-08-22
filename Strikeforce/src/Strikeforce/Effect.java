package Strikeforce;

import java.awt.Image;
import java.util.ArrayList;
import java.util.List;
import static Strikeforce.Global.*;
import javax.swing.ImageIcon;

public class Effect extends Entity {
	
	private List<Image> animationImages = new ArrayList<>();
	private int frameSpeed;
	private int count = 0;
	private boolean animationOver = false;

	public Effect(ImageIcon icon, int startX, int startY, List<Image> inImages, int inFrameSpeed, int inAltitude) {
		super(icon, startX, startY, 0, inAltitude);
		animationImages = inImages;
		frameSpeed = inFrameSpeed;
	}
	
	public Effect(String inName, int inX, int inY, int inDirection, int inAltitude, int frames, int inFrameSpeed) {
		super(inName + "1", inX, inY, inDirection, inAltitude);
		for(int i = 2; i <= frames; i++) {
			ImageIcon icon = resLoader.getImageIcon(inName + i + ".png");
			Image image = icon.getImage();
			animationImages.add(image);
		}
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
