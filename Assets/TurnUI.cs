using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class TurnUI : MonoBehaviour {

	private CoherentUIView view;
	private PlayMakerFSM fsm;
	private bool lastWasPlayerTurnCheck = true;

	void Start () {
		view = Camera.main.GetComponent<CoherentUIView>();
		fsm = GetComponent<PlayMakerFSM>();
	}

	void Update () {
		if (FsmVariables.GlobalVariables.GetFsmBool("IsEnemyTurn").Value != lastWasPlayerTurnCheck) {
			lastWasPlayerTurnCheck = fsm.FsmVariables.GetFsmBool("IsEnemyTurn").Value;
			//lastWasPlayerTurnCheck = FsmVariables.GlobalVariables.GetFsmBool("IsEnemyTurn").Value;
			view.View.TriggerEvent("changeTurn");
		}
	}
}
