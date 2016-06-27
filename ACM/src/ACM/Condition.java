package ACM;

public class Condition {

	private final int id;
	private static int nextConditionId = 1;
	
	public Condition () {
		id = nextConditionId;
		nextConditionId++;
	}
	
	public boolean equals (Condition other) {
		boolean match = id == other.getId();
		return match;
	}
	
	public int getId () {
		return id;
	}
	
	public boolean test(Character owner) {
		return true;
	}
}
