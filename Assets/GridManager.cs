using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;


public class GridManager : MonoBehaviour {

	private GridGraph gridGraph = null;
	private Dictionary<GameObject, Vector3> graphObjects = new Dictionary<GameObject, Vector3>();

	public new static GridManager active;

	void Awake() {
		active = this;
	}

	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
		if (gridGraph.size == Vector2.zero) {
			Debug.LogError("Grid Graph has no nodes!");
			Destroy(gameObject);
		} else if (gridGraph == null) {
			Debug.LogError("Grid Graph does not exist!");
            Destroy(gameObject);
        }
    }
    
    void Update () {
	
	}

	/**Sets or updates the (internally tracked) position of a gameobject.
	 */
	public void SetGraphObjectPosition(GameObject go) {
        if (gridGraph == null) {
			gridGraph = AstarPath.active.astarData.gridGraph;
		}
		Debug.Log ("go.transform.position");
		Debug.Log (go.transform.position);
		Debug.Log ("go.transform.position");
		Debug.Log (gridGraph.GetNearest(go.transform.position));
		Debug.Log ("go.transform.position");
		Debug.Log (gridGraph.GetNearest(go.transform.position).clampedPosition);
		Vector3 closestNodePosition = gridGraph.GetNearest(go.transform.position).clampedPosition;
		if (graphObjects.ContainsKey(go)) {
			graphObjects[go] = closestNodePosition;
		} else {
			graphObjects.Add(go, closestNodePosition);
		}
	}

	/**Returns the clamped node position of a graph object, or false if it is not registered
	 */
	public Vector3 GetGraphObjectPosition(GameObject go) {
		//if (graphObjects.ContainsKey(go)) {
			return graphObjects[go];
		/*} else {
			return null;
		}*/
	}

	/**Given a position, returns whether or not the nearest node has any objects on it
	 */
	public bool IsNodeFree(Vector3 position) {
		Vector3 closestNodePosition = gridGraph.GetNearest(position).clampedPosition;
		return (!graphObjects.ContainsValue(closestNodePosition));
	}
	 
}
