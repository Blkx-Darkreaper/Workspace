package SandBox;

public class Layer extends Material {

	private int depth; //mm
	
	public Layer(String inName, int inElevation, int inTemp, int inDepth) {
		super(inName, inElevation, inTemp);
		depth = inDepth;
	}

	@Override
	public void heat(int energyAdded) {
		heatSurface(energyAdded);
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
}
