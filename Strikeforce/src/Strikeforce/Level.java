package Strikeforce;

import java.awt.Image;
import java.util.ArrayList;
import java.util.List;

public class Level {

	private List<Image> allImages = new ArrayList<>();
	private int nextIndex = 0;
	
	public Level (ArrayList allImagesToAdd) {
		allImages = allImagesToAdd;
	}
	
	public Image getNextImage () {
		Image nextImage = allImages.get(nextIndex);
		nextIndex++;
		
		if(nextIndex > allImages.size()) {
			nextIndex = 0;
		}
		
		return nextImage;
	}
}
