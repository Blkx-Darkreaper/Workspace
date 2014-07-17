package SandBox;

import static SandBox.Global.*;

public class Material {

	private String name;
	private int elevation; //mm
	private int temperature; //K
	private int mass; //g
	private int conductionCoefficient; //W/km*K
	private int heatCapacity; //J/Kg*K
	private int albedo; //%
	
	public Material (String inName, int inElevation, int inTemp) {
		name = inName;
		elevation = inElevation;
		temperature = inTemp;
		
		switch (name) {
		case "air":
			mass = 159;
			conductionCoefficient = 24;
			heatCapacity = 1005;
			albedo = 0;
			break;
			
		case "water":
			mass = 124776;
			conductionCoefficient = 580;
			heatCapacity = 4181;
			albedo = 133;
			break;

		default:
			break;
		}
	}
	
	public String getName() {
		return name;
	}
	
	public int getElevation() {
		return elevation;
	}
	
	public int getTemperature() {
		return temperature;
	}
	
	public void changeTemp(int tempChange) {
		temperature += tempChange;
	}
	
	public int getMass() {
		return mass;
	}
	
	public int getConductionCoef() {
		return conductionCoefficient;
	}
	
	public int getHeatCapacity() {
		return heatCapacity;
	}
	
	public int getAlbedo () {
		return albedo;
	}
	
	public void heat (int energyAdded) {
		int tempChange = energyAdded / heatCapacity;
		changeTemp(tempChange);
	}
	
	public void conductHeat (Material other) {
		int otherTemp = other.getTemperature();
	}
}
