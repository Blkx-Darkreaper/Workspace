package SandBox;

import static SandBox.Global.*;

public class Pocket {

	private Material material = AIR;
	private int absoluteHumidity;
	private int relativeHumidity;
	private int cloudCover;
	private int cloudAlbedo;
	
	public Pocket () {
		
	}
	
	private int illuminate (int luminousExposure) {
		int solarEnergy = luminousExposure * CROSS_SECTIONAL_AREA;
		
		int albedo = cloudCover * cloudAlbedo;
		
		
		int absorbedEnergy = solarEnergy / albedo;
		heatAir(absorbedEnergy);
		
		int remainingEnergy = solarEnergy - absorbedEnergy;
		return remainingEnergy;
	}
	
	private void heatAir (int energy) {
		int heatCapacity = material.getHeatCapacity();
		
		int tempChange = energy / heatCapacity;
		material.changeTemp(tempChange);
	}
}
