package SandBox;

import static SandBox.Global.*;

public class Material implements Comparable<Material> {

	private String name;
	private int positionX, positionY;
	private int elevation; //mm
	private int crossSectionalArea; //mm2
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
			crossSectionalArea = BLOCK_AREA;
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
	
	public int getCrossSectionalArea () {
		return crossSectionalArea;
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
	
	@Override
	public int compareTo(Material other) {
		int comparison;
		
		int elevation = getElevation();
		int otherElevation = other.getElevation();
		
		comparison = elevation - otherElevation;
		
		return comparison;
	}
	
	public int illuminate(int energy) {
		int energyAbsorbed = albedo * energy;
		heat(energyAbsorbed);
		
		int energyRemaining = energy - energyAbsorbed;
		return energyRemaining;
	}
	
	public void heat (int energyAdded) {
		int tempChange = energyAdded / heatCapacity;
		changeTemp(tempChange);
	}
	
	public void conductHeat (Material other) {
		int otherTemp = other.getTemperature();
		int otherConductionCoef = other.getConductionCoef();
		int otherArea = other.getCrossSectionalArea();
		
		int coefficient = Math.max(conductionCoefficient, otherConductionCoef);
		int contactArea = Math.min(crossSectionalArea, otherArea);
		int length = 2 * BLOCK_LENGTH;
		
		int heatTransfer = (-coefficient * contactArea * (temperature - otherTemp) / length * TIME) / 1000000; //J
		
		if(heatTransfer < 0) {
			heat(heatTransfer);
			other.heat(-heatTransfer);
		}
		
		if(heatTransfer > 0) {
			heat(-heatTransfer);
			other.heat(heatTransfer);
		}
	}
}
