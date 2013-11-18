using UnityEngine;
using System.Collections;
using Vectrosity;
using Pathfinding;
using System.Linq;

public class GridPlayerCharacter : GridCharacter {

	// Gameobjects and Components

	// Variables and numbers
	private Vector3 lastMouseNodePosition = Vector3.zero;
	private bool validMovementPath = false;
	private bool validAimingPath = false;

	// Arrays
	private Path path;
	private Vector3[] pathVector;

	// Colors
	private Color pathColor = Color.red;

	public override void Start () {
		base.Start();
	}

	public override void Update () {
		if (gridInteraction.activeGridObject == gameObject) {
			if (isPlanningMovement) {
				handleMovementPlanning();
			} else if (isMoving) {
				var angToTarget = Vector3.Angle(transform.position, pathVector[1]);
				if (angToTarget != Vector3.Angle(transform.position, transform.position + transform.forward)) {
					//RotateTowards(angToTarget);
				}
				//controller.SimpleMove(moveSpeed * transform.forward);
			} else if (isPlanningShooting) {
				handleShootingPlanning();
			} else if (isShooting) {
				//
			}
		}
	}

	private void handleShootingPlanning() {
		aimAtMouse();
		checkValidAiming();
		drawAiming();
	}

	private void aimAtMouse() {
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f)) {
			return;
		}
		pathVector = new Vector3[2];
		pathVector[0] = transform.position + (1.0f * Vector3.up);
		pathVector[1] = hit.point + (1.0f * Vector3.up);
	}

	private void checkValidAiming() {
		validAimingPath = CheckValidShootingTarget(pathVector[1]);
		if (validAimingPath) {
			pathColor = Color.green;
		} else {
			pathColor = Color.red;
		}
	}

	private void drawAiming() {
		Vectrosity.VectorLine.SetLine3D(pathColor, 0.01f, pathVector);
	}

	private void handleMovementPlanning() {
		if (lastMouseNodePosition != gridInteraction.currentNode) {
			planMovement();
			checkValidMovement();
		}
		if (validMovementPath) {
			drawMovement();
		}
	}

	private void planMovement() {
		lastMouseNodePosition = gridInteraction.currentNode;
		path = planMovement(lastMouseNodePosition);
		pathVector = path.vectorPath.ToArray();
		pathVector = pathVector.Select(x => x + new Vector3(0.0f, 1.0f, 0.0f)).ToArray();
	}

	private void checkValidMovement() {
		if (pathVector.Length <= 1) {
			validMovementPath = false;
		} else {
			validMovementPath = CheckValidMovementPath(pathVector);
		}

		if (validMovementPath) {
			pathColor = Color.green;
		} else {
			pathColor = Color.red;
		}
	}

	private void drawMovement() {
		Vectrosity.VectorLine.SetLine3D(Color.green, 0.01f, pathVector);
	}
}
