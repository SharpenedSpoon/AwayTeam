using UnityEngine;
using System.Collections;

public class TakesTurns : MonoBehaviour {

	public int maxTurns = 2;
	public int turns { get; private set; }
	public bool isActive = false;
	
	void Start() {
		ResetTurns();
	}

	public void ResetTurns() {
		turns = maxTurns;
	}

	public void TakeTurn() {
		turns = Mathf.Max(0, turns - 1);
	}

	public bool HasActions() {
		return (turns > 0);
    }

	public bool OutOfActions() {
		return (turns == 0);
	}

	public void MakeActive() {
		isActive = true;
	}
	
	public void MakeInactive() {
		isActive = false;
    }

	public void DoneWithAction() {
		DoneWithAction(true);
	}

	public void DoneWithAction(bool successfulAction) {
		if (successfulAction) {
			TakeTurn();
		}
	}
}
