package SandBox;

public class Layer extends Material {

	private int depth; //mm
	
	public Layer(String inName, int inElevation, int inTemp, int inDepth) {
		super(inName, inElevation, inTemp);
		depth = inDepth;
	}

}
