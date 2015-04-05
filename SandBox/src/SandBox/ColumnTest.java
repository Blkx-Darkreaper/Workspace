package SandBox;

import static org.junit.Assert.*;

import org.junit.Before;
import org.junit.Test;

public class ColumnTest {
	
	Column toTest;
	Material a, b, c, d, e, f, g, h, i, j;

	@Before
	public void setUp() throws Exception {
		toTest = new Column(1, 1);
		a = new Material("a", 0, 0);
		b = new Material("b", 1, 0);
		c = new Material("c", 2, 0);
		d = new Material("d", 3, 0);
		e = new Material("e", 5, 0);
		f = new Material("f", 6, 0);
		g = new Material("g", 7, 0);
		h = new Material("h", 8, 0);
		i = new Material("i", 9, 0);
		j = new Material("j", 10, 0);
		
		toTest.add(a);
		toTest.add(b);
		toTest.add(c);
		toTest.add(d);
		toTest.add(e);
		toTest.add(f);
		toTest.add(g);
		toTest.add(h);
		toTest.add(i);
		toTest.add(j);
	}

	@Test
	public void findMiddle() {
		Material found = toTest.getMaterialAtElevation(5);
		assertTrue(found == e);
	}

	@Test
	public void findBottom() {
		Material found = toTest.getMaterialAtElevation(0);
		assertTrue(found == a);
	}
	
	@Test
	public void findTop() {
		Material found = toTest.getMaterialAtElevation(10);
		assertTrue(found == j);
	}
	
	@Test
	public void findNextBelow() {
		Material found = toTest.getMaterialAtElevation(4);
		assertTrue(found == d);
	}
	
	@Test
	public void findTopOutOfRange() {
		Material found = toTest.getMaterialAtElevation(12);
		assertTrue(found == j);
	}
}
