import java.awt.Color;
import java.awt.image.BufferedImage;
import java.awt.image.WritableRaster;
import java.util.ArrayList;
import java.util.List;


public class Tile implements Comparable<Tile> {
	
	private BufferedImage image;
	private List<Pixel> allPixels;
	
	public Tile(BufferedImage inImage) {
		image = inImage;
		
		int tileSize = image.getWidth();
		allPixels = getPixels(image, 0, 0, tileSize);
	}
	
	public BufferedImage getImage() {
		return image;
	}
	
	private List<Pixel> getPixels(BufferedImage image, int startX, int startY, int tileSize) {
		//WritableRaster raster = image.getRaster();
		//WritableRaster alphaRaster = image.getAlphaRaster();
		List<Pixel> allPixels = new ArrayList<>();

		for (int x = startX; x < (startX + tileSize); x++) {
			for (int y = startY; y < (startY + tileSize); y++) {
				Color pixel = new Color(image.getRGB(x, y));
				int red = pixel.getRed();
				int green = pixel.getGreen();
				int blue = pixel.getBlue();
				int alpha = pixel.getAlpha();
				
/*				int[] pixels = raster.getPixel(x, y, (int[]) null);				
				int red = pixels[0];
				int green = pixels[1];
				int blue = pixels[2];
				int alpha;
				if(alphaRaster == null) {
					alpha = 255;
				} else {
					int[] pixelAlphas = alphaRaster.getPixel(x, y, (int[]) null);
					alpha = pixelAlphas[0];
				}*/

				allPixels.add(new Pixel(red, green, blue, alpha));
			}
		}

		return allPixels;
	}
	
	@Override
	public boolean equals(Object something) {
		Tile other = (Tile) something;
		
		int comparison = compareTo(other);
		
		if(comparison != 0) {
			return false;
		}
		
		return true;
	}

	@Override
	public int compareTo(Tile other) {
		int difference = 0;
		
		for(int i = 0; i < allPixels.size(); i++) {
			Pixel pixel = allPixels.get(i);
			Pixel otherPixel = other.allPixels.get(i);
			
			boolean match = pixel.equals(otherPixel);
			if(match == true) {
				continue;
			}
			
			difference++;
		}
		
		return difference;
	}
}