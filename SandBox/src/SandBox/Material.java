package SandBox;

import static SandBox.Global.*;

public class Material {

	private String name;
	private int mass;
	private int volumetricTemperatureExpansionCoefficient;
	private int heatCapacity;
	private int heatTransferCoefficient;
	private int albedo;
	private int permeability;
	
	private int temperature;
	private int volume;
	private int pressure;
	private int density;
	
	public Material (String inName) {
		name = inName;
		switch (name) {
		case "":
			break;
		default:
			break;
		}
	}
	
	public String getName() {
		return name;
	}
	
	public int getHeatCapacity() {
		return heatCapacity;
	}
	
	public int getHeatTransferCoefficient () {
		return heatTransferCoefficient;
	}
	
	public int getAlbedo () {
		return albedo;
	}

	public int getPermeability () {
		return permeability;
	}
	
	public int getTemperature() {
		return temperature;
	}

	public void changeTemp(int tempChange) {
		updateValues(tempChange, 0, 0);		
	}
	
	public int getVolume () {
		return volume;
	}
	
	public int getPressure () {
		return pressure;
	}
	
	public int getDensity () {
		return density;
	}

	private void updateValues(int tempChange, int volumeChange, int pressureChange) {
		int initialTemperature = temperature;
		int initialVolume = volume;
		int initialPressure = pressure;
		int intialDensity = density;
		
		int finalTemperature = temperature + tempChange;
		int finalDensity = intialDensity / (1 + volumetricTemperatureExpansionCoefficient*tempChange);
		int finalVolume = mass / finalDensity;
		int finalPressure = initialPressure * initialVolume * finalTemperature / (initialTemperature * finalVolume);
		
		temperature = finalTemperature;
		volume = finalVolume;
		pressure = finalPressure;
		density = finalDensity;
	}
}
