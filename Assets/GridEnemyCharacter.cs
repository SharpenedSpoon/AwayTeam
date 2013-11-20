using UnityEngine;
using System.Collections;

public class GridEnemyCharacter : GridCharacter {

	// Gameobjects and components
	private GameObject currentPlayerTarget = null;


	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		if (isActive) {
			if (isPlanningMovement) {
				handleMovementPlanning();
			} else if (isMoving) {
				drawMovement();
				MoveAlongPath();
			} else if (isPlanningShooting) {
				handleShootingPlanning();
			} else if (isShooting) {
				shootAtTarget();
				EndShooting();
			}
		}
	}

	private void handleMovementPlanning() {
		currentPlayerTarget = findClosestPlayer();
		if (currentPlayerTarget == null) {
			DeactivateCharacter();
			return;
		}
		PlanMovement(currentPlayerTarget.transform.position);
		BeginMoving ();
	}

	private void drawMovement() {
		if (path != null && pathVector.Length > 1) {
			Vectrosity.VectorLine.SetLine3D(Color.green, 0.01f, pathVector);
		}
	}

	private void handleShootingPlanning() {
		//
	}

	private void shootAtTarget() {
		//
	}

	private GameObject findClosestPlayer() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("GridObject");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				if (go.name == "PlayerCharacter") {
					closest = go;
					distance = curDistance;
				}
			}
		}
		return closest;
	}
}
