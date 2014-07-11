package ACM;

import javax.swing.*;

public class Frame {
	
	public static void main(String[] args) {
		new Frame();
	}
	
	public Frame () {
		JFrame frame = new JFrame();
		frame.setTitle("ACM");
		frame.add(new Board());
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setSize(512, 300);
		frame.setVisible(true);
		frame.setLocationRelativeTo(null);
	}
}
