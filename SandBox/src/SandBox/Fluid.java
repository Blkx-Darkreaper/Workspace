package SandBox;

public class Fluid extends Material {
	
	private int pressure; //Pa
	private int convectionCoef; //
	private int flowVelocityNS; //mm/s
	private int flowVelocityEW; //mm/s
	private int flowVelocityVert; //mm/s

	public Fluid(String inName, int inElevation, int inTemp) {
		super(inName, inElevation, inTemp);
	}

	public int getPressure() {
		return pressure;
	}
	
	public int getConvectionCoef() {
		return convectionCoef;
	}
	
	public int getFlowVelocityNS() {
		return flowVelocityNS;
	}
	
	public int getFlowVelocityEW() {
		return flowVelocityEW;
	}
	
	public int getFlowVelocityVert() {
		return flowVelocityVert;
	}
	
	public void circulateHeat (Fluid other) {
		int otherTemp = other.getTemperature();
	}
}
