using UnityEngine;
using System.Collections;
using Pathfinding;
using Vectrosity;
using HutongGames.PlayMaker;

public class TurnBasedMovement : AIPath {
	private bool planningMovement;
	private GridGraph gridGraph;
	
	private Vector3[] pathArray;
	private path traversablePathSection;
	private path leftoverPathSection;
	
	private bool isMoving;
	private PlayMakerFSM fsm;
	private RaycastHit hit;
	// Use this for initialization
	void Start () {
		base.Start();
		fsm = GetComponent<PlayMakerFSM>();
		gridGraph = AstarPath.active.astarData.gridGraph;
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
				//target.transform.position = hit.point;
				target.transform.position = gridGraph.GetNearest(hit.point).clampedPosition;
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
				foreach (Node node in path.path) {
					//node.position;
					Debug.Log(node.ToString());
				}
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
