using UnityEngine;
using System.Collections;
using Pathfinding;
using Vectrosity;
using HutongGames.PlayMaker;

public class AITurnBased : AIPath {
private bool planningMovement;
	private GridGraph gridGraph;
	
	private Vector3[] pathArray;
	private Path traversablePathSection;
	private Path leftoverPathSection;
	
	private bool isMoving;
	private PlayMakerFSM fsm;
	private RaycastHit hit;
	
	private Color moveColor;
	
	private CharacterMeta characterMeta;
	// Use this for initialization
	void Start () {
		base.Start();
		fsm = GetComponent<PlayMakerFSM>();
		gridGraph = AstarPath.active.astarData.gridGraph;
		characterMeta = GetComponent<CharacterMeta>();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		/*planningMovement = fsm.FsmVariables.GetFsmBool("IsPlanningMovement").Value;
		isMoving = fsm.FsmVariables.GetFsmBool("IsMoving").Value;
		if (planningMovement) {
			
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
				if (path.path.Count > 3) {
					fsm.FsmVariables.GetFsmBool("ValidMoveTarget").Value = false;
					moveColor = Color.red;
				} else {
					fsm.FsmVariables.GetFsmBool("ValidMoveTarget").Value = true;
					moveColor = Color.green;
				}
				foreach (Node node in path.path) {
					//node.position;
					Debug.Log(node.ToString());
				}
				VectorLine.SetLine3D(moveColor, 0.01f, path.vectorPath.ToArray());
			}
		}*/
		if (path != null && canSearch) {
			VectorLine.SetLine3D(Color.yellow, 0.01f, path.vectorPath.ToArray());
		}
		
	}
	
	public void PlanMovement(GameObject targetPlayer) {
		canSearch = true;
		var destRatio = 1.0f;
		var dist = Vector3.Distance(transform.position, targetPlayer.transform.position);
		if (dist > characterMeta.MoveRange) {
			destRatio = characterMeta.MoveRange / dist;
		}
		var dest = Vector3.Lerp(transform.position, targetPlayer.transform.position, destRatio);
		dest.y = Terrain.activeTerrain.SampleHeight(dest);
		//dest = gridGraph.GetNearest(dest).clampedPosition;
		target.transform.position = dest;
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
