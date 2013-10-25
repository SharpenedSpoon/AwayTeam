using UnityEngine;
using System.Collections;
using Vectrosity;
using HutongGames.PlayMaker;

public class AITurnBased : AIPath {
	private bool planningMovement;
	private bool isMoving;
	private PlayMakerFSM fsm;
	private RaycastHit hit;
	// Use this for initialization
	void Start () {
		base.Start();
		fsm = GetComponent<PlayMakerFSM>();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		planningMovement = fsm.FsmVariables.GetFsmBool("IsPlanningMovement").Value;
		isMoving = fsm.FsmVariables.GetFsmBool("IsMoving").Value;
		if (planningMovement) {
			canSearch = true;
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f))
				return;
			if (hit.rigidbody != target.rigidbody) {
				target.transform.position = hit.point;
			}
		} else {
			canSearch = false;
		}
		if (isMoving) {
			canMove = true;
		} else {
			canMove = false;
		}
		if (planningMovement || isMoving) {
			if (path != null) {
				VectorLine.SetLine3D(Color.green, 0.01f, path.vectorPath.ToArray());
			}
		}
		
	}
	
	/*public void StartPlanningMovement() {
		planningMovement = true;
	}
	
	public void StopPlanningMovement() {
		planningMovement = false;
	}
	
	public void StartMoving() {
		isMoving = true;
	}
	
	public void StopMoving() {
		isMoving = false;
	}*/
}
