package SandBox;

import java.awt.Image;
import java.sql.Time;
import java.util.ArrayList;
import java.util.List;

public class Grid {
	
	private List<Material> allMaterials = new ArrayList<>();
	private String weather;
	private Image image;
	private int latitude;
	private Time sunrise;
	private Time sunset;
	private int illumination; //W/mm2
	
	public Grid (int inLatitude) {
		latitude = inLatitude;
		illumination = 0;
	}
}
