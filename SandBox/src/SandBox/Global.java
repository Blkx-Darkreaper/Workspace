package SandBox;

public class Global {

	static final int BLOCK_LENGTH = 500; //mm
	static final int BLOCK_AREA = 250000; //mm2
	static final int BLOCK_VOLUME = 125000000; //mm3
	
	static final int SECOND = 1; //s
	static final int MINUTE = SECOND * 60; //s
	static final int HOUR = MINUTE * 60; //s
	static final int TIME = MINUTE; //s
	static final int GRAVITATIONAL_ACCELERATION = 9807; //mm/s2
	
	static final int ENVIRONMENTAL_LAPSE_RATE = -65; //K/10000m
	static final int DRY_ADIABATIC_LAPSE_RATE = -98; //K/10000m
	static final int WET_ADIABATIC_LAPSE_RATE = -50; //K/10000m
	
	private int getWaterViscocity (int waterTemp) {
		int fluidViscosity = (int) (2.414*Math.pow(10.0, -5.0) * Math.pow(10, 247.8/(waterTemp - 140)));
		
		return fluidViscosity;
	}
}
