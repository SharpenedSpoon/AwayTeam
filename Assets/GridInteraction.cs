using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Vectrosity;
using HutongGames.PlayMaker;

public class GridInteraction : MonoBehaviour {

	// Components and such
	private GridGraph gridGraph;
	private RaycastHit hit;

	// Variables and numbers
	private float nodeSize;
	
	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
		nodeSize = gridGraph.nodeSize;
	}

	void Update () {
		selectCurrentGridNode();

	}

	private void selectCurrentGridNode() {
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f)) {
			return;
		}
		var pos = hit.point;
		pos.y = Terrain.activeTerrain.SampleHeight(pos);
		pos = gridGraph.GetNearest(pos).clampedPosition;
		var nodeSquare = new Vector3[4];
		nodeSquare[0] = pos + (new Vector3(-0.5f*nodeSize, 0.0f, -0.5f*nodeSize));
		nodeSquare[1] = pos + (new Vector3(-0.5f*nodeSize, 0.0f, 0.5f*nodeSize));
		nodeSquare[2] = pos + (new Vector3(0.5f*nodeSize, 0.0f, 0.5f*nodeSize));
		nodeSquare[3] = pos + (new Vector3(0.5f*nodeSize, 0.0f, -0.5f*nodeSize));
		nodeSquare = addVectorToArray(nodeSquare, new Vector3(0.0f, 1.0f, 0.0f));
		Vectrosity.VectorLine.SetLine3D(Color.red, 0.1f, nodeSquare);
	}

	private Vector3[] addVectorToArray(Vector3[] vectorArray, Vector3 vectorToAdd) {
		var vectorList = new List<Vector3>();
		foreach (Vector3 vec in vectorArray) {
			vectorList.Add(vec + vectorToAdd);
		}
		return vectorList.ToArray();
	}
}
