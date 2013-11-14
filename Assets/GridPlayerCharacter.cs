using UnityEngine;
using System.Collections;
using Vectrosity;

public class GridPlayerCharacter : GridCharacter {

	// Gameobjects and Components

	// Variables and numbers

	// Arrays
	private Vector3[] aimLine;

	// Colors
	private Color aimColor = Color.red;

	public override void Start () {
		base.Start();
	
	}

	public override void Update () {
		if (gridInteraction.activeGridObject == gameObject) {
			aimAtMouse();
		}
	}

	private void aimAtMouse() {
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f)) {
			return;
		}
		aimLine = new Vector3[2];
		aimLine[0] = transform.position + (1.0f * Vector3.up);
		aimLine[1] = hit.point + (1.0f * Vector3.up);
		if (!CheckValidShootingTarget(aimLine[1])) {
			aimColor = Color.red;
		} else {
			aimColor = Color.green;
		}
		Vectrosity.VectorLine.SetLine3D(aimColor, 0.01f, aimLine);
	}
}
