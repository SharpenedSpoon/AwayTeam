using UnityEngine;
using System.Collections;
using Pathfinding;

public class GridObject : MonoBehaviour {

	private GridGraph gridGraph;

	// Use this for initialization
	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
		transform.position = gridGraph.GetNearest(transform.position).clampedPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
