using UnityEngine;
using System.Collections;

public class TakesTurns : MonoBehaviour {

	public int maxTurns = 2;
	public int turns { get; private set; }
	
	void Start() {
		ResetTurns();
	}

	public void ResetTurns() {
		turns = maxTurns;
	}

	public void TakeTurn() {
		turns = Mathf.Max(0, turns - 1);
	}

	public bool OutOfActions() {
		return (turns == 0);
	}
}
