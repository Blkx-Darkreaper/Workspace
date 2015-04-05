import java.awt.Color;


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
	
	public int getIntegerPixel() {
		Color colour = new Color(red, green, blue);
		int integerPixel = colour.getRGB();
		
		return integerPixel;
	}

	public boolean equals(Pixel other) {
		int integerPixel = getIntegerPixel();
		int otherIntegerPixel = other.getIntegerPixel();
		
		int difference = integerPixel - otherIntegerPixel;
		if(difference != 0) {
			return false;
		}
				
/*		int difference = red - other.red;
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
		}*/
		
		return true;
	}
}
