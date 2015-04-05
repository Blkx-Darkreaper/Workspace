import java.awt.Color;
import java.awt.Graphics;
import java.awt.Point;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.geom.Line2D;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

import javax.swing.JComponent;
import javax.swing.Timer;

public class Sensor extends JComponent implements ActionListener {

	//private static final int COLOUR_RANGE = 8388608;
	private static final int SENSOR_THRESHOLD = 5;
	private static final int SENSOR_START_X = 0;
	private static int HOR_SCALING_CONSTANT;
	private static double VERT_SCALING_CONSTANT;
	private static int HEADING_INCREMENT;
	private static final int MAX_SIGNAL_INTENSITY = 100;
	private static final int MIN_SIGNAL_INTENSITY = 50;
	private static final int MAX_INTENSITY_VARIANCE = 5;
	private static final int AVG_INTENSITY_VARIANCE = 1;
	private static final int MAX_DISTANCE = 10;
	private static final int TIME_INTERVAL = 120;
	public static final String[] bands = {"TLF Radio", "HF Shortwave Radio", "VHF Radio", 
		"EHF Microwave", "Infrared", "Ultraviolet", "X-Ray", "Gamma"};
	public static int nextNameValue = 65;

	private int width;
	private int height;
	private Point position;
	private int totalHeadings;
	private List<Source> allSources = new ArrayList<>();
	private List<Transmission> allTransmissions = new ArrayList<>();
	public Timer time;
	public boolean running = false;

	public Sensor(int inWidth, int inHeight, int startX, int startY, int inTotalHeadings) {
		width = inWidth;
		height = inHeight;		
		position = new Point(startX, startY);
		totalHeadings = inTotalHeadings;
		HEADING_INCREMENT = 360 / totalHeadings;
		
		HOR_SCALING_CONSTANT = width / totalHeadings;
		VERT_SCALING_CONSTANT = (double) height / MAX_SIGNAL_INTENSITY;
		
		generateTransmissions();
		
		time = new Timer(TIME_INTERVAL, this);
		time.start();
		
		setFocusable(true);
		addKeyListener(new KeyActionListener());
	}

	private void generateTransmissions() {
		for (int i = 0; i < bands.length; i++) {
			String frequency = bands[i];
			Transmission toAdd = new Transmission(frequency, totalHeadings);
			allTransmissions.add(toAdd);
		}
	}
	
	public void addSource() {
		int sourceX = (int) (Math.random() * 2 * MAX_DISTANCE) - MAX_DISTANCE;
		int sourceY = (int) (Math.random() * 2 * MAX_DISTANCE) - MAX_DISTANCE;
		
		List<Emission> profile = generateEMProfile();
		
		String name = Character.toString((char) nextNameValue);
		nextNameValue++;
		
		Source toAdd = new Source(name, sourceX, sourceY, profile);
		allSources.add(toAdd);
	}
	
	public void removeSource() {
		int max = allSources.size();
		
		if(max == 0) {
			return;
		}
		
		int index = (int) (Math.random() * max);
		
		allSources.remove(index);
	}

	private List<Emission> generateEMProfile() {
		List<Emission> emProfile = new ArrayList<>();
		int maxFrequencies = allTransmissions.size();
		int emissions = (int) (Math.random() * maxFrequencies);
		
		for(int i = 0; i < emissions; i++) {
			String frequency = allTransmissions.get(i).getFrequency();
			int intensity = (int) (Math.random() * (MAX_SIGNAL_INTENSITY - MIN_SIGNAL_INTENSITY)) + MIN_SIGNAL_INTENSITY;
			
			Emission toAdd = new Emission(frequency, intensity);
			emProfile.add(toAdd);
		}
		
		return emProfile;
	}
	
	public void actionPerformed(ActionEvent e) {
		if(running == false) {
			return;
		}
		
		scan();
	}
	
	public void togglePause() {
		running = !running;
	}

	public void scan() {
		clearAllTransmissions();

		detectAllTransmissions();

		repaint();
	}
	
	public void toggleFrequency(int index) {
		Transmission transmission = allTransmissions.get(index);
		
		transmission.toggle();
	}
	
	public void enableAllFrequencies() {
		for(Transmission transmission : allTransmissions) {
			transmission.setEnabled(true);
		}
	}
	
	public int getDistance(Point otherPoint) {
		int centerX = position.x;
		int centerY = position.y;

		int pointX = otherPoint.x;
		int pointY = otherPoint.y;

		int distanceX = centerX - pointX;
		int distanceY = centerY - pointY;

		int distance = (int) Math.sqrt(distanceX * distanceX + distanceY
				* distanceY);

		return distance;
	}

	public Heading getHeading(Point otherPoint) {
		int centerX = position.x;
		int centerY = position.y;

		int pointX = otherPoint.x;
		int pointY = otherPoint.y;

		int distanceX = pointX - centerX;
		int distanceY = pointY - centerY;

		int heading = 360;

		if (distanceY == 0) {
			if (distanceX < 0) {
				heading = 270;
				Heading headingToPoint = new Heading(heading);
				return headingToPoint;
			}
			
			if(distanceX == 0) {
				heading = 0;
				boolean omnidirectional = true;
				Heading headingToPoint = new Heading(heading, omnidirectional);
				return headingToPoint;
			}

			heading = 90;
			Heading headingToPoint = new Heading(heading);
			return headingToPoint;
		}

		if (distanceY < 0) {
			heading = 180;
		}

		double value = distanceX;
		double value2 = distanceY;

		heading += (int) Math.toDegrees(Math.atan(value / value2));
		heading %= 360;

		Heading headingToPoint = new Heading(heading);
		return headingToPoint;
	}

	private List<Line2D> generateSensorData(Transmission transmission) {
		List<Line2D> sensorData = new ArrayList<>();

		int endX = SENSOR_START_X;
		int endY = height;
		int strength = 0;
		//int startY = SENSOR_START_Y;
		
		Point startPoint = new Point(0, 0); //dummy
		Point endPoint = new Point(0, 0); //dummy

		List<Signal> allSignals = transmission.getAllSignals();
		Collections.sort(allSignals);

		int totalSignals = allSignals.size();
		for (int i = 0; i < totalSignals; i++) {
			Signal signal = allSignals.get(i);
			strength = signal.getStrength();
			endY = (int) (height - strength * VERT_SCALING_CONSTANT);
			
			endPoint = new Point(endX, endY);
			endX += HOR_SCALING_CONSTANT;
			
			if(i == 0) {
				startPoint = endPoint;
				continue;
			}

			Line2D toAdd = new Line2D.Double(startPoint, endPoint);
			sensorData.add(toAdd);
			startPoint = endPoint;
		}

		return sensorData;
	}
	
	private void getSignalStrength(Emission emission, int distance) {
		
	}
	
	private void detectSignals() {
		
	}

	private void detectAllTransmissions() {
		for (Source source : allSources) {
			Point sourcePosition = source.getPosition();
			Heading sourceHeading = getHeading(sourcePosition);
			int distanceToSource = getDistance(sourcePosition);

			List<Emission> allSourceEmissions = source.getAllEmissions();
			for (Emission emission : allSourceEmissions) {
				String frequency = emission.getFrequency();
				
				boolean enabled = checkEnabledFrequencies(frequency);
				if(enabled == false) {
					continue;
				}
				
				int relativeIntensity = emission.getRelativeIntensity(distanceToSource);
				
				//Randomization
				int randomMax = (int) (Math.random() * 2 * MAX_INTENSITY_VARIANCE - MAX_INTENSITY_VARIANCE);
				int randomAvg = (int) (Math.random() * 2 * AVG_INTENSITY_VARIANCE - AVG_INTENSITY_VARIANCE);
				int randomMin = (int) (Math.random() * 2 * MAX_INTENSITY_VARIANCE - MAX_INTENSITY_VARIANCE);
				relativeIntensity += (randomMax + 2 * randomAvg - randomMin) / 4;
				
				if (relativeIntensity < SENSOR_THRESHOLD) {
					continue;
				}

				int toFind = allTransmissions.indexOf(new Transmission(frequency));
				if (toFind == -1) {
					continue;
				}

				Transmission receivedTransmission = allTransmissions.get(toFind);
				receivedTransmission.detectSignal(sourceHeading, relativeIntensity);
				//receivedTransmission.disperseSignal(sourceHeading, relativeIntensity);
			}
		}
	}

	private boolean checkEnabledFrequencies(String frequency) {
		int toFind = allTransmissions.indexOf(new Transmission(frequency));
		Transmission found = allTransmissions.get(toFind);
		boolean enabled = found.getEnabled();
		
		return enabled;
	}

	private void clearAllTransmissions() {
		for (Transmission transmission : allTransmissions) {
			transmission.clear();
		}
	}

	private Color getFrequencyColour(String frequency) {
		int index = allTransmissions.indexOf(new Transmission(frequency));

		Color frequencyColour;
		switch (index) {
		case 0:
			frequencyColour = Color.RED;
			break;
		case 1:
			frequencyColour = Color.ORANGE;
			break;
		case 2:
			frequencyColour = Color.YELLOW;
			break;
		case 3:
			frequencyColour = Color.GREEN;
			break;
		case 4:
			frequencyColour = Color.CYAN;
			break;
		case 5:
			frequencyColour = Color.BLUE;
			break;
		case 6:
			frequencyColour = Color.MAGENTA;
			break;
		default:
			frequencyColour = Color.WHITE;
			break;
		}
		/*int allColours = allReceivedTransmissions.size();
		int colourValue = (int) (COLOUR_RANGE * (1 - 0.5 * allColours * index));

		Color frequencyColour = new Color(colourValue);*/

		return frequencyColour;
	}

	@Override
	protected void paintComponent(Graphics g) {
		super.paintComponent(g);
		
		//g.setColor(Color.WHITE);
		g.setColor(Color.BLACK);
		g.fillRect(0, 0, width, height);
		
		if(allSources.size() == 0) {
			return;
		}
		
		for (Transmission transmission : allTransmissions) {
			String frequency = transmission.getFrequency();
			Color frequencyColour = getFrequencyColour(frequency);
			g.setColor(frequencyColour);
			
			List<Line2D> sensorData = generateSensorData(transmission);
			
			for (Line2D line : sensorData) {
				int startX = (int) line.getP1().getX();
				int startY = (int) line.getP1().getY();

				int endX = (int) line.getP2().getX();
				int endY = (int) line.getP2().getY();

				g.drawLine(startX, startY, endX, endY);
			}
		}
	}
	
	public void increaseFocus() {
		totalHeadings *= 2;
		
		if(totalHeadings > 72) {
			totalHeadings = 72;
		}
		
		updateFocus();
	}
	
	public void decreaseFocus() {
		totalHeadings /= 2;
		
		if(totalHeadings < 9) {
			totalHeadings = 9;
		}
		
		updateFocus();
	}
	
	private void updateFocus() {
		HEADING_INCREMENT = 360 / totalHeadings;
		HOR_SCALING_CONSTANT = width / totalHeadings;
		
		allTransmissions.clear();
		
		generateTransmissions();
	}
	
	public void atLocation() {
		for(Source source : allSources) {
			Point sourcePosition = source.getPosition();
			boolean atSource = sourcePosition.equals(position);
			
			if(atSource == false) {
				continue;
			}
			
			String location = source.getName();
			String message = "You have arrived at position ";
			driver.infoBox(message + location + ".", "Position Reached");
		}
	}
	
	public void moveUp() {
		int currentX = (int) position.getX();
		int currentY = (int) position.getY();
		currentY++;
		
		position = new Point(currentX, currentY);
	}
	
	public void moveDown() {
		int currentX = (int) position.getX();
		int currentY = (int) position.getY();
		currentY--;
		
		position = new Point(currentX, currentY);
	}
	
	public void moveLeft() {
		int currentX = (int) position.getX();
		int currentY = (int) position.getY();
		currentX--;
		
		position = new Point(currentX, currentY);
	}
	
	public void moveRight() {
		int currentX = (int) position.getX();
		int currentY = (int) position.getY();
		currentX++;
		
		position = new Point(currentX, currentY);
	}
	
	private class KeyActionListener extends KeyAdapter {
		
		private final int moveUpKey = KeyEvent.VK_UP;
		private final int moveDownKey = KeyEvent.VK_DOWN;
		private final int moveLeftKey = KeyEvent.VK_LEFT;
		private final int moveRightKey = KeyEvent.VK_RIGHT;
		
		@Override
		public void keyReleased (KeyEvent e) {
			int key = e.getKeyCode();

			switch (key) {
			case moveLeftKey:
				break;
			case moveRightKey:
				break;
			case moveUpKey:
				break;
			case moveDownKey:
				break;
			}
		}
		
		@Override
		public void keyPressed (KeyEvent e) {
			int key = e.getKeyCode();

			switch (key) {
			case moveLeftKey:
				moveLeft();
				break;
			case moveRightKey:
				moveRight();
				break;
			case moveUpKey:
				moveUp();
				break;
			case moveDownKey:
				moveDown();
				break;
			}
			
			atLocation();
		}
	}
	
	private class Heading {

		private int direction;
		private boolean omni = false;

		public Heading(int inHeading) {
			direction = inHeading;
			direction %= 360;
		}
		
		public Heading(int inHeading, boolean isOmni) {
			direction = inHeading;
			omni = isOmni;
		}

		public int getDirection() {
			return direction;
		}
		
		public boolean getOmni() {
			return omni;
		}

		public void alterHeading(int alteration) {
			direction += alteration;
			direction %= 360;
		}
	}

	private class Source {

		private String name;
		private Point position;
		private List<Emission> allEmissions;

		public Source(int startX, int startY, List<Emission> EMProfile) {
			position = new Point(startX, startY);
			allEmissions = EMProfile;
		}
		
		public Source(String inName, int startX, int startY, List<Emission> EMProfile) {
			name = inName;
			position = new Point(startX, startY);
			allEmissions = EMProfile;
		}
		
		public String getName() {
			return name;
		}

		public Point getPosition() {
			return position;
		}

		public List<Emission> getAllEmissions() {
			return allEmissions;
		}
	}
	
	private class Emission {
		
		private String frequency;
		private int intensity;
		
		private Emission(String inFrequency, int inIntensity) {
			frequency = inFrequency;
			
			if(inIntensity > MAX_SIGNAL_INTENSITY) {
				intensity = MAX_SIGNAL_INTENSITY;
				return;
			}
			
			intensity = inIntensity;
		}
		
		public String getFrequency() {
			return frequency;
		}
		
		public int getIntensity() {
			return intensity;
		}
		
		public int getRelativeIntensity(int distance) {
			if(distance == 0) {
				return intensity;
			}
			
			int relativeIntensity = intensity / distance;

			return relativeIntensity;
		}
	}

	private class Transmission implements Comparable<Transmission> {

		private static final double ATTENUATION_COEF = 0.5;
		private String frequency;
		private List<Signal> allSignals;
		private boolean enabled = true;

		// dummy
		public Transmission(String inFrequency) {
			frequency = inFrequency;
		}

		public Transmission(String inFrequency, int totalHeadings) {
			frequency = inFrequency;
			allSignals = new ArrayList<>();

			generateAllSignalHeadings(totalHeadings);
		}

		private void generateAllSignalHeadings(int totalHeadings) {
			int headingIncrement = 360 / totalHeadings;
			int headingDirection = 0;

			for (int i = 0; i < totalHeadings; i++) {
				Signal toAdd = new Signal(headingDirection);
				allSignals.add(toAdd);

				headingDirection += headingIncrement;
			}
		}

		public String getFrequency() {
			return frequency;
		}

		public List<Signal> getAllSignals() {
			return allSignals;
		}

		public boolean getEnabled() {
			return enabled;
		}
		
		public void setEnabled(boolean condition) {
			enabled = condition;
		}

		public void toggle() {
			enabled = !enabled;
		}

		@Override
		public boolean equals(Object something) {
			Transmission other = (Transmission) something;

			String otherFrequency = other.getFrequency();

			if (frequency != otherFrequency) {
				return false;
			}

			return true;
		}

		@Override
		public int compareTo(Transmission other) {
			List<String> frequencies = Arrays.asList(bands);
			
			int frequencyIndex = frequencies.indexOf(frequency);
			int otherFrequencyIndex = frequencies.indexOf(other.getFrequency());
			
			int comparison = frequencyIndex - otherFrequencyIndex;
			return comparison;
		}

		public void clear() {
			allSignals.clear();
			generateAllSignalHeadings(totalHeadings);
			
/*			for (Signal signal : allSignals) {
				signal.setStrength(0);
			}*/
		}

		public void detectSignal(Heading sourceHeading, int relativeIntensity) {
			int direction = sourceHeading.getDirection();
			
			// Converts signal direction to available sensor headings 
			int adjustedDirection = direction / HEADING_INCREMENT;
			adjustedDirection *= HEADING_INCREMENT;
			
			int toFind = allSignals.indexOf(new Signal(adjustedDirection));

			if (toFind == -1) {
				/*Signal toAdd = new Signal(direction);
				allSignals.add(toAdd);
				toFind = allSignals.indexOf(toAdd);*/
				return;
			}

			Signal signal = allSignals.get(toFind);
			signal.addToSignal(relativeIntensity);
		}

		private void disperseSignal(Heading sourceHeading, int relativeIntensity) {
			double spread = ATTENUATION_COEF;
			
			boolean omnidirectional = sourceHeading.getOmni();
			if(omnidirectional == true) {
				spread = 0.75;
			}
			
			int weakeningSignal = (int) (relativeIntensity * spread);
			
			int direction = sourceHeading.getDirection();
			int adjustedDirection = direction / HEADING_INCREMENT;
			adjustedDirection *= HEADING_INCREMENT;
			
			Heading leftHeading = new Heading(adjustedDirection - HEADING_INCREMENT);
			Heading rightHeading = new Heading(adjustedDirection + HEADING_INCREMENT);

			while (weakeningSignal > SENSOR_THRESHOLD) {
				detectSignal(leftHeading, weakeningSignal);
				// leftHeading.addSignal(frequency, weakeningSignal);
				leftHeading = new Heading(leftHeading.getDirection() - HEADING_INCREMENT);

				detectSignal(rightHeading, weakeningSignal);
				// rightHeading.addSignal(frequency, weakeningSignal);
				rightHeading = new Heading(rightHeading.getDirection() + HEADING_INCREMENT);

				weakeningSignal *= spread;
			}
		}
	}

	private class Signal implements Comparable<Signal> {

		private final int direction;
		private int strength;

		private Signal(int inDirection) {
			direction = inDirection;
			strength = 0;
		}

		private int getDirection() {
			return direction;
		}

		public int getStrength() {
			return strength;
		}

		public void setStrength(int value) {
			if (value < 0) {
				strength = 0;
				return;
			}
			
			if(value > MAX_SIGNAL_INTENSITY) {
				strength = MAX_SIGNAL_INTENSITY;
				return;
			}

			strength = value;
		}

		@Override
		public boolean equals(Object something) {
			Signal other = (Signal) something;

			int otherDirection = other.getDirection();

			if (direction != otherDirection) {
				return false;
			}

			return true;
		}

		@Override
		public int compareTo(Signal other) {
			int signalDirection = direction;
			if(signalDirection < 180) {
				signalDirection += 360;
			}
			signalDirection -= 180;
			
			int otherDirection = other.getDirection();
			if(otherDirection < 180) {
				otherDirection += 360;
			}
			otherDirection -= 180;
			
			int comparison = signalDirection - otherDirection;
			
			return comparison;
		}

		public void addToSignal(int relativeIntensity) {
			if (relativeIntensity < 0) {
				return;
			}

			strength += relativeIntensity;
		}
	}
}
