using UnityEngine;
using System.Collections;
using Pathfinding;

public class GridMouseInteraction : MonoBehaviour {

	public Vector3 currentNodePosition { get; private set; }
	private RaycastHit hit;
	private Terrain terrain;
	private GridGraph gridGraph;
	private GridDrawer gridDrawer;
	private GridManager gridManager;

	private bool validState = false;

	public new static GridMouseInteraction active;
	public bool followMouse = true;
	public bool drawMouseNode = true;

	void Awake() {
		active = this;
	}

	void Start () {
		currentNodePosition = Vector3.zero;
		terrain = Terrain.activeTerrain;
		gridGraph = AstarPath.active.astarData.gridGraph;
		gridDrawer = GridDrawer.active;
		gridManager = GridManager.active;
	}

	void Update () {
		if (followMouse) {
			if (getMouseNode() != currentNodePosition) {
				currentNodePosition = getMouseNode();
				validState = !gridManager.IsNodeFree(currentNodePosition);
			}
		}

		if (drawMouseNode) {
			gridDrawer.DrawNode(currentNodePosition, validState);
		}

	}

	private Vector3 getMouseNode() {
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f)) {
			return currentNodePosition;
		}

		Vector3 terrainPosition = hit.point;
		terrainPosition.y = terrain.SampleHeight(terrainPosition);
		return gridGraph.GetNearest(terrainPosition).clampedPosition;
	}
}
