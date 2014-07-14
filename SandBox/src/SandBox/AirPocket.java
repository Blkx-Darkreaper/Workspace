package SandBox;

import static SandBox.Global.*;

public class AirPocket {

	private Material material = AIR;
	private int absoluteHumidity; //Kg/m3
	private int relativeHumidity; //%
	private int cloudCover; //%
	private float cloudAlbedo = 0.9f;
	
	public AirPocket (int inAbsoluteHumidity) {
		absoluteHumidity = inAbsoluteHumidity;
		update();
	}
	
	public Material getMaterial () {
		return material;
	}
	
	public int getAbsoluteHumidity () {
		return absoluteHumidity;
	}
	
	public int getRelativeHumidity () {
		return relativeHumidity;
	}
	
	public int getCloudCover () {
		return cloudCover;
	}
	
	public void update() {
		int pressure = material.getPressure();
		int temperature = material.getTemperature();
		
		float saturationVapourPressure = (float)((611.21 * Math.exp((17.62*(temperature - 273.15))/(temperature - 30.03))) 
				* (1.0016 + 3.15*Math.pow(10, -8) * pressure - 7.4/pressure));
		relativeHumidity = (int) (100*absoluteHumidity * 461.475 * temperature / (1000*saturationVapourPressure));
		
		if(relativeHumidity < 95) {
			cloudCover = 0;
			return;
		}
		
		cloudCover = 18*(relativeHumidity - 95) + 10;
	}
	
	private int illuminate (int luminousExposure) {
		int solarEnergy = luminousExposure * CROSS_SECTIONAL_AREA;
		
		float albedo = cloudCover * cloudAlbedo / 100;
		
		
		int absorbedEnergy = (int) (solarEnergy * albedo);
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
