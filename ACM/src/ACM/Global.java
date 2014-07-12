package ACM;

import java.util.ArrayList;
import java.util.List;

import ACM.Character.Situation;

public class Global {

	public static final int PLAYER_START_POSITION = 250;
	
	public static List<Condition> allConditions = new ArrayList<>();
	private static Condition noCondition;
	private static Condition trailer;
	
	public static void createAllConditions() {
		createConditionNoCondition();
	}
	
	private static void createConditionNoCondition() {
		noCondition = new Condition() {
			public boolean test(Character master) {
				return true;
			}
		};
		allConditions.add(noCondition);
	}
	
	private static void createConditionTrailer() {
		trailer = new Condition() {
			public boolean test(Character master) {
				Situation perception = master.getPerception();
				
				//return perception.trailer;
				
				return true;
			}
		};
	}
}
