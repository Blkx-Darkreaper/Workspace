package SandBox;

public class Global {

	static final int BLOCK_LENGTH = 500;
	static final int CROSS_SECTIONAL_AREA = 250000;
	static final int BLOCK_VOLUME = 125000000;
	
	private int getWaterViscocity (int waterTemp) {
		int fluidViscosity = (int) (2.414*Math.pow(10.0, -5.0) * Math.pow(10, 247.8/(waterTemp - 140)));
		
		return fluidViscosity;
	}
}
