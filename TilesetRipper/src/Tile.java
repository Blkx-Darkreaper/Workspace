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
				int integerPixel = image.getRGB(x, y);
				int red = (integerPixel >> 16) & 0xff;
				int green = (integerPixel >> 8) & 0xff;
				int blue = integerPixel & 0xff;
				int alpha = (integerPixel >> 24) & 0xff;
				
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
	public int hashCode() {
		int hashCode;
		long sum = 0;
		
		for(Pixel pixel : allPixels) {
			int integerPixel = pixel.getIntegerPixel();
			sum += integerPixel;
		}
		
		int size = allPixels.size();
		hashCode = (int) (sum / size);
		
		return hashCode;
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
		
		List<Integer> allDifferences = difference(other);
		
		for(Integer number : allDifferences) {
			if(number == 0) {
				continue;
			}
			
			difference++;
		}
		
/*		for(Integer number : allDifferences) {
			difference += Math.abs(number.intValue());
		}
		
		int totalPixels = allPixels.size();
		difference /= totalPixels;*/
		
		return difference;
	}
	
	public List<Integer> difference(Tile other) {
		List<Integer> allDifferences = new ArrayList<>();
		
		for(int i = 0; i < allPixels.size(); i++) {
			Pixel pixel = allPixels.get(i);
			int integerPixel = pixel.getIntegerPixel();
			
			Pixel otherPixel = other.allPixels.get(i);
			int otherIntegerPixel = otherPixel.getIntegerPixel();
			
			int difference = integerPixel - otherIntegerPixel;
			allDifferences.add(new Integer(difference));
		}
		
		return allDifferences;
	}
}