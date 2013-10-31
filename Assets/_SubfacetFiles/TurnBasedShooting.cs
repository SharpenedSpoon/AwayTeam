using UnityEngine;
using System.Collections;
using Pathfinding;
using Vectrosity;
using HutongGames.PlayMaker;

public class TurnBasedShooting : MonoBehaviour {
	//private AstarPath aStarPath;
	private GridGraph gridGraph;
	private RaycastHit hit;
	private PlayMakerFSM fsm;
	private PlayMakerFSM targetFsm;
	private GameObject targetObject;
	private Vector3[] aimingLine;
	private Color aimingColor;
	public Transform target = null;
	
	private CharacterMeta characterMeta;
	// Use this for initialization
	void Start () {
		//aStarPath = GetComponent<AstarPath>();
		//gridGraph = aStarPath.astarData.gridGraph;
		gridGraph = AstarPath.active.astarData.gridGraph;
		fsm = GetComponent<PlayMakerFSM>();
		characterMeta = GetComponent<CharacterMeta>();
		if (target != null) {
			targetFsm = target.GetComponent<PlayMakerFSM>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			return;
		}
		if (fsm.FsmVariables.GetFsmBool("IsAiming").Value) {
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f))
				return;
			if (hit.rigidbody != target.rigidbody) {
				target.transform.position = hit.point;
				target.transform.position = new Vector3(target.transform.position.x, Terrain.activeTerrain.SampleHeight(target.transform.position), target.transform.position.z);
			}
			aimingLine = new Vector3[2];
			aimingLine[0] = objectMidpoint(transform);
			if (targetFsm.FsmVariables.GetFsmBool("ValidTarget").Value) {
				fsm.FsmVariables.GetFsmBool("ValidAimTarget").Value = true;
				targetObject = targetFsm.FsmVariables.GetFsmGameObject("TargetObject").Value;
				aimingLine[1] = objectMidpoint(targetObject.transform);
				aimingColor = Color.green;
			} else {
				fsm.FsmVariables.GetFsmBool("ValidAimTarget").Value = false;
				targetObject = null;
				aimingLine[1] = target.transform.position;
				aimingColor = Color.red;
			}
			VectorLine.SetLine3D(aimingColor, 0.01f, aimingLine);
			/*if (gridGraph.Linecast(transform.position, target.transform.position)) {
				fsm.FsmVariables.FindFsmBool("ValidAimTarget").Value = true;
			} else {
				fsm.FsmVariables.FindFsmBool("ValidAimTarget").Value = false;
			}
		} else if (fsm.FsmVariables.GetFsmBool("IsShooting").Value) {*/
		}
	}
	
	public void Shoot(GameObject targetObject) {
		targetObject.GetComponent<CharacterMeta>().TakeDamage(characterMeta.Damage);
	}
	
	private Vector3 objectMidpoint(Transform trans) {
		Vector3 pos = trans.position;
		Vector3 scl = trans.lossyScale;
		pos.y = pos.y + (float) 0.5*scl.y;
		return pos;
	}
}