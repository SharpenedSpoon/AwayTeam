using UnityEngine;
using System.Collections;
using Pathfinding;
using Vectrosity;
using HutongGames.PlayMaker;

public class TurnBasedShooting : MonoBehaviour {
	private AstarPath aStarPath;
	private GridGraph gridGraph;
	private RaycastHit hit;
	private PlayMakerFSM fsm;
	public Transform target = null;
	// Use this for initialization
	void Start () {
		//aStarPath = GetComponent<AstarPath>();
		//gridGraph = aStarPath.astarData.gridGraph;
		gridGraph = AstarPath.active.astarData.gridGraph;
		fsm = GetComponent<PlayMakerFSM>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			return;
		}
		Debug.Log(fsm.FsmVariables.GetFsmBool("IsAiming").Value);
		if (fsm.FsmVariables.GetFsmBool("IsAiming").Value) {
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f))
				return;
			if (hit.rigidbody != target.rigidbody) {
				target.transform.position = hit.point;
			}
			if (gridGraph.Linecast(transform.position, target.transform.position)) {
				//fsm.FsmVariables.FindFsmBool("IsAiming").Value = true;
			}
		} else if (fsm.FsmVariables.GetFsmBool("IsShooting").Value) {
			var shootRay = new Vector3[2];
			shootRay[0] = transform.position;
			shootRay[1] = target.transform.position;
			//var shootRay = new Array(transform.position, target.transform.position);
			VectorLine.SetLine3D(Color.green, 0.01f, shootRay);
		}
	}
}
