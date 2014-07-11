package ACM;

import java.util.ArrayList;
import java.util.List;

public class Global {

	static final int PLAYER_START_POSITION = 250;
	
	static List<Condition> allConditions = new ArrayList<>();
	private static Condition noCondition;
	
	public static void createAllConditions() {
		createConditionNoCondition();
	}
	
	private static void createConditionNoCondition() {
		noCondition = new Condition() {
			public boolean test(Character owner) {
				return true;
			}
		};
		allConditions.add(noCondition);
	}
}
