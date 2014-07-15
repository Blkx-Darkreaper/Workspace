package ACM;

import java.util.ArrayList;
import java.util.List;
import static ACM.Global.*;

public class Character {

	protected Projectile craft;
	private Situation perception;
	private List<Response> training;
	private List<Response> experience;
	private List<Assessment> allAssessments;
	
	public Character (Aircraft inCraft) {
		craft = inCraft;
		perception = new Situation() {

			//Empirical
			public boolean trailer = false;
			public boolean underFire = false;
			public Projectile target = null;
			public String pursuitType = "";
			public int gLoad = 1;
			
			//Subjective
			public int awareness;
			public int aggression;
			public int caution;
			public int confidence;
			public int desperation;
			public int altitude;
			public int airspeed;
			
			//Subjective - Enemy relative
			public int closureRate;
			public int rangeToTarget;
			public int heading;
			public int clockPosition;
			public int enemyAltitude;
			public int enemyAirspeed;
			public String enemyPursuitType;
			
			@Override
			public void update() {
				
			}
		};
		
		training = new ArrayList<>();
		experience = new ArrayList<>();
		allAssessments = new ArrayList<>();
	}
	
	public Situation getPerception() {
		return perception;
	}
	
	public void updatePerception() {
		
	}
	
	private void makeAssessment() {
		int stance = 0;
		int conditionCode = makeConditionCode();
		
		allAssessments.add(new Assessment(stance, conditionCode));
	}
	
	private int makeConditionCode () {
		int conditionCode = 0;
		
		for(Condition aCondition : allConditions) {
			boolean match = aCondition.test(this);
			
			if(match == false) {
				continue;
			}
			
			int conditionId = aCondition.getId();
			conditionId *= conditionId;
			conditionCode += conditionId;
		}
		
		return conditionCode;
	}
	
	public class Assessment {

		private int stance;
		private int conditionCode;
		
		public Assessment (int inStance, int inConditionCode) {
			stance = inStance;
			conditionCode = inConditionCode;
		}
	}
	
	public abstract class Situation {
		public abstract void update();
	}
}