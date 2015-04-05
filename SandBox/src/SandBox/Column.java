package SandBox;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class Column {
	
	private int centerX, centerY;
	private List<Material> allMaterials =  new ArrayList<>();
	
	public Column(int positionX, int positionY) {
		centerX = positionX;
		centerY = positionY;
	}

	public void add(Material toAdd) {
		allMaterials.add(toAdd);
		Collections.sort(allMaterials);
	}
	
	public Material getMaterial(int index) {
		if(index >= allMaterials.size()) {
			return null;
		}
		
		if(index < 0) {
			return null;
		}
		
		Material toGet = allMaterials.get(index);
		return toGet;
	}
	
	public Material getMaterialAtElevation(int elevation) {
		Material dummy = new Material("dummy", elevation, 0);
		
		int size = allMaterials.size();
		
		int max = size;
		int min = 0;
		int middle = (max - min) / 2 + min;

		Material toCheck = allMaterials.get(middle);
		Material closestMatch = allMaterials.get(middle);
		
		while(min < max) {
			int comparison = toCheck.compareTo(dummy);
			
			if(comparison == 0) {
				return toCheck;
			}
			
			if(comparison > 0) {
				max = middle;
			}
			
			if(comparison < 0) {
				min = middle + 1;
			}
			
			closestMatch = toCheck;
			
			middle = (max - min) / 2 + min;
			if(middle >= size) {
				return closestMatch;
			}
			
			toCheck = allMaterials.get(middle);
		}
		
		return closestMatch;
	}
	
	public int getSize() {
		int size = allMaterials.size();
		return size;
	}
}
