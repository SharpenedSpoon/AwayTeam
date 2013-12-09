using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour {

	private GridManager grid;
	
	void Start () {
		grid = GridManager.active;
		UpdateGridPosition();
	}

	public void UpdateGridPosition() {
		grid.SetGraphObjectPosition(gameObject);
		// get clamped position
		transform.position = grid.GetGraphObjectPosition(gameObject);
	}
}
