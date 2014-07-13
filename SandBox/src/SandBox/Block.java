package SandBox;

import static SandBox.Global.*;

import java.awt.Image;

public class Block {

	private Image image;
	private int elevation;
	private Material material;
	private boolean onSurface;
	
	private int groundWaterVolume;
	private int groundWaterTemp;
	
	public Block (int inElevation, Material inMaterial, int inGroundWaterVolume, int inGroundWaterTemp) {
		elevation = inElevation;
		material = inMaterial;
		groundWaterVolume = inGroundWaterVolume;
		groundWaterTemp = inGroundWaterTemp;
	}
	
	public boolean getOnSurface () {
		return onSurface;
	}
	
	public void setOnSurface (boolean condition) {
		onSurface = condition;
	}
	
	public void calculate () {

	}
	
	public void updateImage () {
		
	}
	
	private int heatSurface (int solarEnergy) {
		int albedo = material.getAlbedo();
		int heatCapacity = material.getHeatCapacity();
		
		int absorbedEnergy = solarEnergy / albedo;
		int tempChange = absorbedEnergy / heatCapacity;
		
		int reflectedEnergy = solarEnergy - absorbedEnergy;
		
		return reflectedEnergy;
	}
	
	private void evaporate () {
		int temperature = material.getTemperature();
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
