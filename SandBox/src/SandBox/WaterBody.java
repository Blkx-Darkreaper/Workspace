package SandBox;

import static SandBox.Global.*;

public class WaterBody extends Fluid {

	private String name = "Water Body";
	private boolean surface;
	private int depth; //mm
	private int turbulence;
	private int viscocity; //g/km*s

	public WaterBody(int inElevation, int inTemp, int inDepth, boolean onSurface) {
		super("water", inElevation, inTemp);
		depth = inDepth;
		turbulence = 0;
		viscocity = 1002;
		surface = onSurface;
	}
	
	public boolean getOnSurface() {
		return surface;
	}
	
	public void setOnSurface (boolean condition) {
		surface = condition;
	}
	
	public int getDepth() {
		return depth;
	}
	
	public int getTurbulence() {
		return turbulence;
	}
	
	public int getViscocity() {
		return viscocity;
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
		int otherDepth = other.getDepth();
		
		if(depth != otherDepth) {
			gravitationalFlow(other, depth-otherDepth);
		}
		
		int netNSFlow = getFlowVelocityNS() + other.getFlowVelocityNS();
		if(netNSFlow != 0) {
			momentumFlow(netNSFlow);
		}
		
		int netEWFlow = getFlowVelocityEW() + other.getFlowVelocityEW();
	}
	
	private void gravitationalFlow (WaterBody other, int depthDifference) {
		int crossSectionalArea = depthDifference * BLOCK_LENGTH;
		
		// Flow from this to other
		if(depthDifference > 0) {
			int pressure = getPressure();
			int force = pressure * crossSectionalArea;
			int acceleratedMass = getMass() * depthDifference / depth;
			int deltaVelocity = (force / acceleratedMass) * TIME / 1000;
		} 
		
		// Flow from other to this
		else {
			int otherPressure = other.getPressure();
		}
	}
	
	private void momentumFlow (int netFlow) {
		
	}
	
	public void waves () {
		
	}
}
