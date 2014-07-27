package Strikeforce;
import javax.swing.*;

public class ResLoader {
	private ClassLoader allImageResources;
	
	public ResLoader(ClassLoader inClassLoader) {
		allImageResources = inClassLoader;
	}
	
	public ImageIcon getImageIcon(String filename) {
		return new ImageIcon(allImageResources.getResource(filename));
	}
}
