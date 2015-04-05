import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

import javax.imageio.ImageIO;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JScrollPane;

public class driver {
	
	public int frameWidth = 1024;
	public int frameHeight = 768;
	public Panel container;

	public static void main(String[] args) {
		new driver();
	}

	public driver() {
		final JFrame frame = new JFrame();
		//frame.add(new Panel());
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setSize(frameWidth, frameHeight);
		frame.setLocationRelativeTo(null);

		container = new Panel();
		
		JScrollPane pane = new JScrollPane(container, JScrollPane.VERTICAL_SCROLLBAR_ALWAYS, 
				JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);
		
		//int nextRow = container.nextRow;
		//container.setPreferredSize(new Dimension(frameWidth, frameHeight));
		//container.revalidate();
		
		frame.add(pane, BorderLayout.CENTER);
		frame.setVisible(true);
	}

	@SuppressWarnings("serial")
	class Panel extends JPanel implements ActionListener {
		
		public List<List<Tile>> allTileGroupsToDraw;
		public int tileSize = 16;
		public int viewSize = 64;
		public int ratio;
		public int tolerance;
		public int largestGroupSize = 1;
		public String filename;
		public String extension;

		public Panel() {
			setFocusable(true);
			tolerance = tileSize * tileSize / 4;
			//tolerance = 2000000;
			ratio = viewSize / tileSize;
			filename = "germany1";
			extension = "png";

			//saveTileset(filename, extension);
			addKeyListener(new KeyActionListener());
			
			updatePanel();
		}
		
		public void increaseViewSize() {
			viewSize *= 2;
			
			if(viewSize > 256) {
				viewSize = 256;
			}
		}
		
		public void decreaseViewSize() {
			viewSize /= 2;
			
			if(viewSize < 8) {
				viewSize = 8;
			}
		}
		
		public void increaseTileSize() {
			tileSize *= 2;
			
			if(tileSize > 1024) {
				tileSize = 1024;
			}
		}
		
		public void decreaseTileSize() {
			tileSize /= 2;
			
			if(tileSize < 4) {
				tileSize = 4;
			}
		}
		
		public void increaseTolerance() {
			tolerance += 5;
			
			if(tolerance > 100) {
				tolerance = 100;
			}
		}
		
		public void decreaseTolerance() {
			tolerance -= 5;
			
			if(tolerance < 0) {
				tolerance = 0;
			}
		}
		
		public void saveTileset(String filename, String extension) {
			largestGroupSize = largestGroupSize(allTileGroupsToDraw);
			int panelWidth = largestGroupSize * (viewSize + ratio);
			int panelHeight = allTileGroupsToDraw.size() * (viewSize + ratio);
			
			BufferedImage image = new BufferedImage(panelWidth, panelHeight, BufferedImage.TYPE_INT_ARGB);
			Graphics2D g2d = image.createGraphics();
			
			drawTileGroups(g2d);
			
			g2d.dispose();
			
			File output = new File("images\\" + filename + "tileset." + extension);
			try {
				ImageIO.write(image, extension, output);
			} catch (IOException e) {
				System.out.println("Image could not be saved");
				e.printStackTrace();
			}
		}

		public void updatePanel() {
			BufferedImage image = loadImage(filename + "." + extension);
			List<Tile> allTiles = getTiles(image, tileSize);
			
			List<List<Tile>> tileGroups = sortTiles(allTiles);
			
			allTileGroupsToDraw = tileGroups;
			
			repaint();
			
			largestGroupSize = largestGroupSize(allTileGroupsToDraw);
			int panelWidth = largestGroupSize * (viewSize + ratio);
			int panelHeight = allTileGroupsToDraw.size() * (viewSize + ratio);
			
			setPreferredSize(new Dimension(panelWidth, panelHeight));
			revalidate();
		}
		
		private int largestGroupSize(List<List<Tile>> allGroups) {
			int largestGroupSize = 0;
			for(List<Tile> aGroup : allGroups) {
				if(aGroup.size() <= largestGroupSize) {
					continue;
				}
				
				largestGroupSize = aGroup.size();
			}
			
			return largestGroupSize;
		}

		private List<List<Tile>> sortTiles(List<Tile> allTiles) {
			removeDuplicates(allTiles);
			return orderByComparison(allTiles);
		}

		private List<List<Tile>> orderByComparison(List<Tile> allTiles) {
			List<List<Tile>> tileGroups = new ArrayList<>();
			int missedDuplicates = 0;
			
			for(int i = 0; i < allTiles.size(); i++) {
				Tile toCheck = allTiles.get(i);
				
				boolean foundSimilar = false;
				
				for(List<Tile> tileGroup : tileGroups) {
					Tile toCompareTo = tileGroup.get(0);
					
					int comparison = toCheck.compareTo(toCompareTo);
					if(comparison > tolerance) {
						continue;
					}
					
					foundSimilar = true;
					
/*					if(comparison == 0) {
						break;
					}*/
					
					boolean perfectMatch = checkGroupForPerfectMatch(toCheck, tileGroup);
					if(perfectMatch == true) {
						missedDuplicates++;
						break;
					}
					
					tileGroup.add(toCheck);
					break;
				}
				
				if(foundSimilar == true) {
					continue;
				}
				
				List<Tile> newGroup = new ArrayList<>();
				newGroup.add(toCheck);
				tileGroups.add(newGroup);
			}
			
			System.out.println("Missed " + missedDuplicates + " duplicates");
			
			return tileGroups;
		}

		private boolean checkGroupForPerfectMatch(Tile toCheck, List<Tile> tileGroup) {
			for(Tile aTile : tileGroup) {
				int comparison = toCheck.compareTo(aTile);
				
				if(comparison == 0) {
					return true;
				}
			}
			
			return false;
		}

		private void removeDuplicates(List<Tile> allTiles) {
			int size = allTiles.size();
			Set<Tile> freeOfDuplicates = new HashSet<>(size);
			
			freeOfDuplicates.addAll(allTiles);
			
			allTiles.clear();
			
			allTiles.addAll(freeOfDuplicates);
/*			for(Tile aTile : freeOfDuplicates) {
				allTiles.add(aTile);
			}*/
		}

		public void paintComponent(Graphics g) {
			super.paintComponent(g);
			Graphics2D g2d = (Graphics2D) g;
			
			drawTileGroups(g2d);
			
			g2d.dispose();
		}

		private BufferedImage loadImage(String filename) {
			ClassLoader classLoader = this.getClass().getClassLoader();
			URL filePath = classLoader.getResource(filename);
			File fileToLoad = new File(filePath.getPath());
			BufferedImage image;
			try {
				image = ImageIO.read(fileToLoad);
			} catch (IOException e) {
				image = null;
				System.out.println("Image could not be read");
			}
			return image;
		}
		
		private List<Tile> getTiles(BufferedImage image, int tileSize) {
			List<Tile> allTiles = new ArrayList<>();
			
			int tilesWide = image.getWidth() / tileSize;
			int tilesHigh = image.getHeight() / tileSize;
			
			for(int x = 0; x < tilesWide; x++) {
				for(int y = 0; y < tilesHigh; y++) {
					int startX = x * tileSize;
					int startY = y * tileSize;
					
				 	BufferedImage subImage = image.getSubimage(startX, startY, tileSize, tileSize);
				 	
					allTiles.add(new Tile(subImage));
				}
			}
			
			return allTiles;
		}
		
		private void drawTileGroups(Graphics2D g2d) {
			for(int i = 0; i < allTileGroupsToDraw.size(); i++) {
				List<Tile> tileGroup = allTileGroupsToDraw.get(i);
				
				int cornerY = i * (viewSize + ratio);
				drawAllTiles(g2d, tileGroup, cornerY);
				
				if(tileGroup.size() > largestGroupSize) {
					largestGroupSize = tileGroup.size();
				}
			}
		}
		
		private void drawAllTiles(Graphics2D g2d, List<Tile> tileGroup, int cornerY) {
			//int tilesWide = frameWidth / tileSize;
			//int tilesWide = frameWidth / (viewSize + ratio);
			
			for(int i = 0; i < tileGroup.size(); i++) {
				Tile toDraw = tileGroup.get(i);
				
				//int mod = i % tilesWide;
				//int cornerX = mod * viewSize + ratio * mod;
				int cornerX = i * (viewSize + ratio);

				//int div = i / tilesWide;
				//int cornerY = startY + div * viewSize + ratio * div;
				
				drawTile(g2d, toDraw, cornerX, cornerY);
			}
		}
		
		private void drawTile(Graphics2D g2d, Tile toDraw, int cornerX, int cornerY) {
			BufferedImage image = toDraw.getImage();
			
			g2d.drawImage(image, cornerX, cornerY, viewSize, viewSize, null);
			//g2d.dispose();
		}

		@Override
		public void actionPerformed(ActionEvent arg0) {
			updatePanel();
		}
	}
	
	private class KeyActionListener extends KeyAdapter {
		private final int increaseTileSize = KeyEvent.VK_EQUALS;
		private final int decreaseTileSize = KeyEvent.VK_MINUS;
		private final int increaseViewSize = KeyEvent.VK_CLOSE_BRACKET;
		private final int decreaseViewSize = KeyEvent.VK_OPEN_BRACKET;
		private final int increaseTolerance = KeyEvent.VK_PERIOD;
		private final int decreaseTolerance = KeyEvent.VK_COMMA;
		private final int saveTileset = KeyEvent.VK_S;
		private final int exit = KeyEvent.VK_ESCAPE;
		
		public void keyReleased(KeyEvent e) {
			int key = e.getKeyCode();
			switch (key) {
			case saveTileset:
				container.saveTileset(container.filename, container.extension);
				break;
			}
			
			container.updatePanel();
		}
		
		public void keyPressed(KeyEvent e) {
			int key = e.getKeyCode();
			switch (key) {
			case increaseTileSize:
				container.increaseTileSize();
				break;
			case decreaseTileSize:
				container.decreaseTileSize();
				break;
			case increaseViewSize:
				container.increaseViewSize();
				break;
			case decreaseViewSize:
				container.decreaseViewSize();
				break;
			case increaseTolerance:
				container.increaseTolerance();
				break;
			case decreaseTolerance:
				container.decreaseTolerance();
				break;
			case exit:
				System.exit(0);
				break;
			}
			
			container.updatePanel();
		}
	}
}
