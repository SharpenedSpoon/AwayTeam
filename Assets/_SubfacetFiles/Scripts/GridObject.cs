using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class GridObject : MonoBehaviour {

	// Gameobjects and Components
	public GridInteraction gridInteraction { get; private set; }
	protected Transform tr;
	protected GridGraph gridGraph;

	// Variables and numbers
	public bool isActive { get; private set; }
	[HideInInspector]
	public bool hasControl = false;
	protected RaycastHit hit;
	private float nodeSize;


	public virtual void Start () {
		isActive = false;

		gridGraph = AstarPath.active.astarData.gridGraph;
		gridInteraction = GameObject.Find("GridInteractionController").GetComponent<GridInteraction>();

		nodeSize = gridGraph.nodeSize;
		transform.position = gridGraph.GetNearest(transform.position).clampedPosition;

		tr = transform;
	}

	public virtual void Update () {
	
	}

	protected Vector3[] addVectorToArray(Vector3[] vectorArray, Vector3 vectorToAdd) {
		var vectorList = new List<Vector3>();
		foreach (Vector3 vec in vectorArray) {
			vectorList.Add(vec + vectorToAdd);
		}
		return vectorList.ToArray();
	}

	public void MakeActive() {
		isActive = true;
	}

	public void MakeInactive() {
		isActive = false;
	}

	public void LoseControl() {
		hasControl = false;
	}

	public void GainControl() {
		hasControl = true;
	}
}
