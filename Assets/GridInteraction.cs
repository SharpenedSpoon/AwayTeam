using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Vectrosity;
using HutongGames.PlayMaker;

public class GridInteraction : MonoBehaviour {

	// Objects and Components
	public GameObject currentGridObject { get; private set; }
	public GameObject activeGridObject { get; private set; }
	public GameObject activePlayer { get; private set; }
	private GridGraph gridGraph;
	private RaycastHit hit;
	private Terrain activeTerrain;
	private PlayMakerFSM turnManagerFsm;

	// Variables and numbers and stuff
	private float nodeSize;
	private Vector3 currentNode = Vector3.zero;
	private Vector3[] currentNodeSquare;
	private Vector3[] activeNodeSquare;

	// Colors
	public Color activeNodeColor = Color.green;
	public Color defaultEmptyNodeColor = Color.black;
	public Color defaultGridObjectNodeColor = Color.white;
	public Color playerNodeColor = Color.blue;
	public Color enemyNodeColor = Color.red;
	public Color resourceNodeColor = Color.yellow;
	private Color currentNodeColor = Color.black;

	
	void Start () {
		activeTerrain = Terrain.activeTerrain;
		gridGraph = AstarPath.active.astarData.gridGraph;
		nodeSize = gridGraph.nodeSize;
		turnManagerFsm = GameObject.Find("TurnManager").GetComponent<PlayMakerFSM>();

		currentGridObject = null;
		activeGridObject = null;
	}

	void Update () {
		selectCurrentGridNode();
		drawActiveGridObjectNode();

		if (Input.GetMouseButtonDown(0)) {
			if (turnManagerFsm.FsmVariables.GetFsmBool("IsPlayerTurn").Value) {
				updateActiveGridObject();
			}
		}
	}

	private void selectCurrentGridNode() {
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f)) {
			//currentNode = Vector3.zero;
			return;
		}
		var usingOldGridObject = true;
		var pos = hit.point;
		pos.y = activeTerrain.SampleHeight(pos);
		pos = gridGraph.GetNearest(pos).clampedPosition;

		if (currentNode == Vector3.zero || currentNode != pos) {
			currentNode = pos;
			currentGridObject = findClosestGridObject(currentNode);
			usingOldGridObject = false;
		}

		if (usingOldGridObject) {
			// we need to check and see if the old grid object has moved
			if (currentGridObject != null) {
				if (gridGraph.GetNearest(currentGridObject.transform.position).clampedPosition != currentNode) {
					currentGridObject = null;
				}
			}
		}

		drawCurrentGridNode();
	}

	private void drawCurrentGridNode() {
		currentNodeSquare = makeGridSquare(currentNode);
		if (currentGridObject == null) {
			currentNodeColor = defaultEmptyNodeColor;
		} else {
			currentNodeColor = defaultGridObjectNodeColor;
			if (currentGridObject.name == "PlayerCharacter") {
				currentNodeColor = playerNodeColor;
			} else if (currentGridObject.name == "EnemyCharacter") {
				currentNodeColor = enemyNodeColor;
			} else if (currentGridObject.name == "CollectibleResource") {
				currentNodeColor = resourceNodeColor;
			}
		}
		Vectrosity.VectorLine.SetLine3D(currentNodeColor, 0.01f, currentNodeSquare);
	}

	private void updateActiveGridObject() {
		if (activeGridObject != null) {
			activeGridObject.SendMessage("MakeInactive");
			activeGridObject = null;
		}
		if (currentGridObject != null) {
			currentGridObject.SendMessage("MakeActive");
			activeGridObject = currentGridObject;
		}
	}
	
	private void drawActiveGridObjectNode() {
		if (activeGridObject == null) {
			return;
		}

		activeNodeSquare = makeGridSquare(activeGridObject.transform.position, 0.3f);
		Vectrosity.VectorLine.SetLine3D(activeNodeColor, 0.01f, activeNodeSquare);
	}

	private Vector3[] makeGridSquare(Vector3 centerNode, float distanceAboveTerrain = 0.2f) {
		// first ensure that the given node is at terrain level
		centerNode.y = activeTerrain.SampleHeight(centerNode);

		// now create the square
		var output = new Vector3[5];
		output[0] = centerNode + (new Vector3(-0.5f*nodeSize, 0.0f, -0.5f*nodeSize));
		output[1] = centerNode + (new Vector3(-0.5f*nodeSize, 0.0f, 0.5f*nodeSize));
		output[2] = centerNode + (new Vector3(0.5f*nodeSize, 0.0f, 0.5f*nodeSize));
		output[3] = centerNode + (new Vector3(0.5f*nodeSize, 0.0f, -0.5f*nodeSize));
		output[4] = output[0];
		output = addVectorToArray(output, new Vector3(0.0f, distanceAboveTerrain, 0.0f));
		return output;
	}

	private Vector3[] addVectorToArray(Vector3[] vectorArray, Vector3 vectorToAdd) {
		var vectorList = new List<Vector3>();
		foreach (Vector3 vec in vectorArray) {
			vectorList.Add(vec + vectorToAdd);
		}
		return vectorList.ToArray();
	}

	private GameObject findClosestGridObject(Vector3 sourcePosition) {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("GridObject");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = sourcePosition;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		if (distance > .707*nodeSize) { // hacky 1/sqrt(2) check to see if found object is inside node square
			closest = null;
		}
		return closest;
	}
}
