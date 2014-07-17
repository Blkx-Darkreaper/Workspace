package SandBox;

public class WaterBody extends Fluid {

	private boolean surface;
	private int depth; //mm
	private int turbulence;
	private int viscocity; //g/km*s

	public WaterBody(int inElevation, int inTemp, int inDepth) {
		super("water", inElevation, inTemp);
		depth = inDepth;
		turbulence = 0;
		viscocity = 1002;
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
	
	public void surfaceFlow (WaterBody other) {
		
	}
	
	public void waves () {
		
	}
}
