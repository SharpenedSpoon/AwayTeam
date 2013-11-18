using UnityEngine;
using System.Collections;
using Pathfinding;

public class GridObject : MonoBehaviour {

	// Gameobjects and Components
	public GridInteraction gridInteraction { get; private set; }
	protected Transform tr;
	private GridGraph gridGraph;

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
