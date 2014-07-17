package SandBox;

import static SandBox.Global.*;
import java.util.ArrayList;
import java.util.List;

public class AirPocket extends Fluid {

	private int humidityContent; //g/mm3
	private int humidityContentCapacity; //g/mm3
	private int relativeHumidity; //%
	private int cloudCover; //%
	private int cloudAlbedo = 80; 
	private List<Precipitation> precipitation;
	
	public AirPocket (int inElevation, int inTemp, int inHumidityContent, int inHumidityContentCapacity, int inCloudCover) {
		super("air", inElevation, inTemp);
		humidityContent = inHumidityContent;
		humidityContentCapacity = inHumidityContentCapacity;
		cloudCover = inCloudCover;
		precipitation = new ArrayList<>();
		update();
	}
	
	public int getHumidityContent () {
		return humidityContent;
	}
	
	public int getRelativeHumidity () {
		return relativeHumidity;
	}
	
	public int getCloudCover () {
		return cloudCover;
	}
	
	public void update() {
		int pressure = getPressure();
		int temperature = getTemperature();
		
		float saturationVapourPressure = (float)((611.21 * Math.exp((17.62*(temperature - 273.15))/(temperature - 30.03))) 
				* (1.0016 + 3.15*Math.pow(10, -8) * pressure - 7.4/pressure));
		relativeHumidity = (int) (100*humidityContent * 461.475 * temperature / (1000*saturationVapourPressure));
		
		if(relativeHumidity < 95) {
			cloudCover = 0;
			return;
		}
		
		cloudCover = 18*(relativeHumidity - 95) + 10;
	}
	
	public int illuminate (int luminousExposure) {
		int solarEnergy = luminousExposure * CROSS_SECTIONAL_AREA;
		
		float albedo = cloudCover / 100 * cloudAlbedo / 100;
		
		
		int absorbedEnergy = (int) (solarEnergy * albedo);
		heat(absorbedEnergy);
		
		int remainingEnergy = solarEnergy - absorbedEnergy;
		return remainingEnergy;
	}
	
	public void cool () {
		
	}
	
	public void airFlow (AirPocket other) {
		
	}
	
	public Precipitation precipitate () {
		return null;
	}
}
