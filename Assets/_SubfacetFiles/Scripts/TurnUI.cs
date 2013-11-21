using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class TurnUI : MonoBehaviour {

	// Gameobjects and components
	private CoherentUIView viewComponent = null;
	private PlayMakerFSM fsm;
	private GridInteraction gridInteraction;

	// Variables and bools and arrays
	private bool isPlayerTurn = true;

	void Start () {
		viewComponent = Camera.main.GetComponent<CoherentUIView>();
		gridInteraction = GameObject.Find("GridInteractionController").GetComponent<GridInteraction>();
		fsm = GetComponent<PlayMakerFSM>();


		if (viewComponent) {
			viewComponent.OnReadyForBindings += this.RegisterBindings;
		}
		viewComponent.ReceivesInput = true;
	}

	void Update () {
		if (fsm.FsmVariables.GetFsmBool("IsPlayerTurn").Value != isPlayerTurn) {
			isPlayerTurn = fsm.FsmVariables.GetFsmBool("IsPlayerTurn").Value;
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			endTurnButtonClicked();
		}
	}

	private void RegisterBindings(int frame, string url, bool isMain)
	{
		if (isMain) {
			var view = viewComponent.View;
			if (view != null) {
				view.BindCall("changeTurn", (System.Action)this.endTurnButtonClicked);
				//view.BindCall("startMovement", (System.Action)this.movementButtonClicked);
				//view.BindCall("startShooting", (System.Action)this.shootingtButtonClicked);
			}
		}
	}

	private void endTurnButtonClicked() {
		Debug.Log("changed turn");
		if (isPlayerTurn) {
			fsm.SendEvent("BeginEnemyTurn");
		} else {
			fsm.SendEvent("BeginPlayerTurn");
		}
	}

	private void movementButtonClicked() {
		var activeGridObject = gridInteraction.activeGridObject;
		if (activeGridObject != null) {
			if (activeGridObject.name == "PlayerCharacter") {
				activeGridObject.GetComponent<GridPlayerCharacter>().BeginPlanningMovement();
			}
		}
	}

	private void shootingtButtonClicked() {
		//
	}
}
