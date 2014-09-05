
public class Pixel {

	private int red;
	private int green;
	private int blue;
	private int alpha;
	
	public Pixel(int inRed, int inGreen, int inBlue, int inAlpha) {
		red = inRed;
		green = inGreen;
		blue = inBlue;
		alpha = inAlpha;
	}

	public boolean equals(Pixel other) {
		int difference = red - other.red;
		if(difference != 0) {
			return false;
		}
		
		difference = green - other.green;
		if(difference != 0) {
			return false;
		}
		
		difference = blue - other.blue;
		if(difference != 0) {
			return false;
		}
		
		return true;
	}
}
