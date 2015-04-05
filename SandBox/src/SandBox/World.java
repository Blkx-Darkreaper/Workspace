package SandBox;

import java.awt.Image;
import java.awt.Point;
import java.util.ArrayList;
import java.util.List;
import static SandBox.Global.*;

public class World {
	
	private int width, length, height;
	private List<Column> allColumns = new ArrayList<>();
	private Image image;
	private Point solarPosition;
	private int solarOutput; //W
	private int illumination; //W/mm2
	
	public World () {
		illumination = 0;
	}
	
	public void buildWorld(int inWidth, int inLength, int inHeight, int groundLevel, int waterLevel) {
		width = inWidth;
		length = inLength;
		height = inHeight;
		
		for(int x = 0; x < width; x++) {
			for(int y = 0; y < length; y++) {
				Column currentColumn = new Column(x, y);
				allColumns.add(currentColumn);
				for(int z = 0; z < height; z++) {
					if(z > waterLevel) {
						int humidityContent = 0;
						int humidityContentCapacity = 0;
						int cloudCover = 0;
						currentColumn.add(new AirPocket(z, STANDARD_TEMPERATURE, humidityContent, 
								humidityContentCapacity, cloudCover));
						continue;
					}
					
					if(z > groundLevel) {
						boolean onSurface = false;
						if(z == waterLevel) {
							onSurface = true;
						}
						
						currentColumn.add(new WaterBody(z, STANDARD_TEMPERATURE, BLOCK_LENGTH, onSurface));
						continue;
					}
					
					String name = "Dirt Block";
					int waterContent = 0;
					int waterContentTemp = STANDARD_TEMPERATURE;
					int waterStorageCapacity = 0;
					int permeability = 0;
					boolean onSurface = false;
					
					if(z == groundLevel) {
						onSurface = true;
					}
					currentColumn.add(new Block(name, z, STANDARD_TEMPERATURE, waterContent, waterContentTemp, 
							waterStorageCapacity, permeability, onSurface));
				}
			}
		}
	}
	
	public int getIllumination(int positionX, int positionY) {
		return 1000;
	}
	
	public void illuminateWorld() {
		for(int x = 0; x < width; x++) {
			for(int y = 0; y < length; y++) {
				int energy = getIllumination(x, y);
				Column illuminatedColumn = getColumn(x, y);
				illuminateColumn(illuminatedColumn, energy);
			}
		}
	}
	
	public void illuminateColumn(Column toIlluminate, int irradiance) {
		boolean moveDown = true;
		int energyRemaining = irradiance;
		int lastIndex = toIlluminate.getSize();
		int nextIndex = lastIndex;
		
		while(energyRemaining > 0) {
			Material next = toIlluminate.getMaterial(nextIndex);
			energyRemaining = next.illuminate(energyRemaining);
			
			if(nextIndex <= 0) {
				moveDown = false;
			}
			
			if(moveDown == true) {
				nextIndex--;
			}
			
			if(nextIndex >= lastIndex) {
				return;
			}
			
			if(moveDown == false) {
				nextIndex++;
			}
		}
	}
	
	public Column getColumn(int positionX, int positionY) {
		int index = width*(positionX - 1) + positionY - 1;
		
		if(index < 0) {
			return null;
		}
		
		if(index >= allColumns.size()) {
			return null;
		}
		
		Column found = allColumns.get(index);
		
		return found;
	}
}
