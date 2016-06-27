package ACM;

import java.util.ArrayList;
import java.util.List;

public class Response {

	private String name;
	private String description;
	private List<Action> allActions;
	
	public Response (String inName, String inDescription) {
		name = inName;
		description = inDescription;
		allActions = new ArrayList<>();
	}
	
	public String getName () {
		return name;
	}
	
	public String getDescription () {
		return description;
	}
	
	public List<Action> getAllActions () {
		return allActions;
	}
	
	public void setAllActions (List<Action> allActionsToAdd) {
		allActions = allActionsToAdd;
	}

	public void addAction (Action actionToAdd) {
		allActions.add(actionToAdd);
	}
	
	public void performActions (Character owner) throws InterruptedException {
		for(Action anAction : allActions) {
			boolean performed = anAction.perform(owner);
		}
	}
}
