package SandBox;

import static SandBox.Global.*;

import java.awt.Image;

public class Block extends Material {

	private boolean surface;
	
	private int waterContent; //mm3
	private int waterContentTemperature; //K
	private int waterStorageCapacity; //mm3/mm3
	private int permeability; //mm2
	
	public Block (String inName, int inElevation, int inTemp, int inWaterContent, int inWaterContentTemp, 
			int inWaterStorageCapacity, int inPermeability) {
		super(inName, inElevation, inTemp);
		surface = true;
		waterContent = inWaterContent;
		waterContentTemperature = inWaterContentTemp;
		waterStorageCapacity = inWaterStorageCapacity;
		permeability = inPermeability;
	}
	
	public boolean getOnSurface () {
		return surface;
	}
	
	public void setOnSurface (boolean condition) {
		surface = condition;
	}
	
	public int getWaterContent() {
		return waterContent;
	}
	
	public int getWaterContentTemp() {
		return waterContentTemperature;
	}
	
	public int getWaterStorageCapacity() {
		return waterStorageCapacity;
	}
	
	public int getPermeability() {
		return permeability;
	}
	
	@Override
	public void heat(int energyAdded) {
		if(surface == true) {
			heatSurface(energyAdded);
			return;
		}
	}
	
	private int heatSurface (int energyAdded) {
		int albedo = getAlbedo();
		int heatCapacity = getHeatCapacity();
		
		int absorbedEnergy = energyAdded * albedo;
		int tempChange = absorbedEnergy / heatCapacity;
		changeTemp(tempChange);
		
		int reflectedEnergy = energyAdded - absorbedEnergy;
		
		return reflectedEnergy;
	}
	
	public void evaporate () {
		if(surface == false) {
			return;
		}
	}
	
	public void groundWaterFlow (Block other) {
		
	}
	
/*	private int getDischargeRate (Chunk other) {
		//Darcy's law
		int permeability = material.getPermeability();
		int pressure;
		int otherPressure = other;
		int deltaPressure = otherPressure - pressure;
		int waterViscosity = getWaterViscocity(groundWaterTemp);
		
		int totalDischarge = -permeability * CROSS_SECTIONAL_AREA * deltaPressure / (waterViscosity * CHUNK_LENGTH);
		
		return totalDischarge;
	}*/
	

}
