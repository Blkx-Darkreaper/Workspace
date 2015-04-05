import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JOptionPane;
import javax.swing.JPanel;

public class driver {
	public static int width = 800;
	public static int height = 600;
	public static Sensor hud;
	public static int startX = 0;
	public static int startY = 0;
	public static int totalHeadings = 36;

	public static void main(String[] args) {
		JFrame testFrame = new JFrame();
		testFrame.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
		
		hud = new Sensor(width, height, startX, startY, totalHeadings);
		hud.setPreferredSize(new Dimension(width, height));
		testFrame.getContentPane().add(hud, BorderLayout.CENTER);
		
		JPanel buttonsPanel = new JPanel();
		JButton removeSourceButton = new JButton("Remove Source");
		JButton pauseButton = new JButton("Start/Pause");
		JButton addSourceButton = new JButton("Add Source");
		JButton increaseFocusButton = new JButton("+");
		JButton decreaseFocusButton = new JButton("-");
		buttonsPanel.add(increaseFocusButton);
		buttonsPanel.add(addSourceButton);
		buttonsPanel.add(pauseButton);
		buttonsPanel.add(removeSourceButton);
		buttonsPanel.add(decreaseFocusButton);
		testFrame.getContentPane().add(buttonsPanel, BorderLayout.SOUTH);
		
		JPanel frequenciesPanel = new JPanel();
		JButton enabledAllButton = new JButton("Enable All");
		JButton redButton = new JButton("Toggle Red");
		JButton orangeButton = new JButton("Toggle Orange");
		JButton yellowButton = new JButton("Toggle Yellow");
		JButton greenButton = new JButton("Toggle Green");
		JButton cyanButton = new JButton("Toggle Cyan");
		JButton blueButton = new JButton("Toggle Blue");
		JButton magentaButton = new JButton("Toggle Magenta");
		JButton whiteButton = new JButton("Toggle White");
		frequenciesPanel.add(enabledAllButton);
		frequenciesPanel.add(redButton);
		frequenciesPanel.add(orangeButton);
		frequenciesPanel.add(yellowButton);
		frequenciesPanel.add(greenButton);
		frequenciesPanel.add(cyanButton);
		frequenciesPanel.add(blueButton);
		frequenciesPanel.add(magentaButton);
		frequenciesPanel.add(whiteButton);
		testFrame.getContentPane().add(frequenciesPanel, BorderLayout.NORTH);
		
		removeSourceButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.removeSource();
				hud.scan();
				hud.grabFocus();
			}
		});
		
		addSourceButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.addSource();
				hud.scan();
				hud.grabFocus();
			}
		});
		
		pauseButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.togglePause();
				hud.grabFocus();
			}
		});
		
		increaseFocusButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.increaseFocus();
				hud.scan();
				hud.grabFocus();
			}
		});
		
		decreaseFocusButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.decreaseFocus();
				hud.scan();
				hud.grabFocus();
			}
		});
		
		enabledAllButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.enableAllFrequencies();
				hud.scan();
				hud.grabFocus();
			}
		});
		
		redButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(0);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		orangeButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(1);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		yellowButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(2);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		greenButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(3);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		cyanButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(4);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		blueButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(5);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		magentaButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(6);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		whiteButton.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				hud.toggleFrequency(7);
				hud.scan();
				hud.grabFocus();
			}
		});
		
		testFrame.pack();
		testFrame.setVisible(true);
	}
	
	public static void infoBox(String infoMessage, String location) {
        JOptionPane.showMessageDialog(null, infoMessage, "InfoBox: " + location, JOptionPane.INFORMATION_MESSAGE);
    }
}