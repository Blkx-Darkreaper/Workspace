package SandBox;

public class Global {

	static final int BLOCK_LENGTH = 5;
	static final int CROSS_SECTIONAL_AREA = 25;
	static final int BLOCK_VOLUME = 125;
	
	static final Material WATER = new Material("water");
	static final Material AIR = new Material("air");
	
	private int getWaterViscocity (int waterTemp) {
		int fluidViscosity = (int) (2.414*Math.pow(10.0, -5.0) * Math.pow(10, 247.8/(waterTemp - 140)));
		
		return fluidViscosity;
	}
}
