using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;


public class GridManager : MonoBehaviour {

	private GridGraph gridGraph = null;
	private List<GameObject>[,] graphObjects;
	private Dictionary<GameObject, int[,]> gameObjectCoordinates;

	
	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
		if (gridGraph.size == Vector2.zero) {
			Debug.LogError("Grid Graph has no nodes!");
			Destroy(gameObject);
		}
		graphObjects = new List<GameObject>[Mathf.FloorToInt(gridGraph.size.x), Mathf.FloorToInt(gridGraph.size.y)];
		gameObjectCoordinates = new Dictionary<GameObject, int[,]>();
	}

	void Update () {
	
	}

	public void SetPosition(GameObject go) {
		if (gameObjectCoordinates.ContainsKey(go)) {
			//
		} else {
			//gameObjectCoordinates.Add()
		}
	}

	//private 
}
