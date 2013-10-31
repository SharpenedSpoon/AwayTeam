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
	
	private Vector3[] aimingLine;
	
	private bool isAiming;
	// Use this for initialization
	void Start () {
		base.Start();
		fsm = GetComponent<PlayMakerFSM>();
		gridGraph = AstarPath.active.astarData.gridGraph;
		characterMeta = GetComponent<CharacterMeta>();
		isAiming = false;
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
		if (path != null && (canSearch || canMove)) {
			VectorLine.SetLine3D(Color.yellow, 0.01f, path.vectorPath.ToArray());
		}
		
		if (isAiming) {
			VectorLine.SetLine3D(Color.yellow, 0.01f, aimingLine);
		}
		
	}
	
	public void PlanMovement(GameObject targetObject) {
		canSearch = true;
		var destRatio = 1.0f;
		var dist = Vector3.Distance(transform.position, targetObject.transform.position);
		if (dist > characterMeta.MoveRange) {
			destRatio = characterMeta.MoveRange / dist;
		}
		var dest = Vector3.Lerp(transform.position, targetObject.transform.position, destRatio);
		dest.y = Terrain.activeTerrain.SampleHeight(dest);
		dest = gridGraph.GetNearest(dest).clampedPosition;
		target.transform.position = dest;
	}
	
	public void BeginMovement() {
		
			//canSearch = false;
			canMove = true;
		
	}
	
	public void EndMovement() {
		canSearch = false;
		canMove = false;
	}
	
	public void PlanShooting(GameObject targetObject) {
		isAiming = true;
		aimingLine = new Vector3[2];
		aimingLine[0] = objectMidpoint(transform);
		aimingLine[1] = objectMidpoint(targetObject.transform);
	}
	
	public void Shoot(GameObject targetObject) {
		isAiming = false;
		targetObject.GetComponent<CharacterMeta>().TakeDamage(characterMeta.Damage);
	}
	
	private Vector3 objectMidpoint(Transform trans) {
		Vector3 pos = trans.position;
		Vector3 scl = trans.lossyScale;
		pos.y = pos.y + (float) 0.5*scl.y;
		return pos;
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
