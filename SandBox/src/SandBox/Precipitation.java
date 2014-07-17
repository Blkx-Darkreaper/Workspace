package SandBox;

public class Precipitation extends Material {
	
	private int rate; //g/s
	private String state;

	public Precipitation(String inName, int inElevation, int inTemp, int inRate, String inState) {
		super(inName, inElevation, inTemp);
		rate = inRate;
		state = inState;
	}

	public void precipitateFalls() {
		
	}
}
