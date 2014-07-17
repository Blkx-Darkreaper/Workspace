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
	
	public void surfaceFlow (WaterBody other) {
		
	}
	
	public void waves () {
		
	}
}
