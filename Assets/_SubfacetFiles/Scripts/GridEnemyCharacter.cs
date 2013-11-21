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
			if (characterMeta.actions <= 0) {
				MakeInactive();
			} else if (isIdle) {
				figureOutWhatToDo();
			} else if (isPlanningMovement) {
				handleMovementPlanning();
			} else if (isMoving) {
				drawMovement();
				MoveAlongPath();
			} else if (isPlanningShooting) {
				handleShootingPlanning();
			} else if (isShooting) {
				shootAtTarget();
			}
		}
	}

	private void figureOutWhatToDo() {
		currentPlayerTarget = findClosestPlayer();
		if (currentPlayerTarget == null) {
			DeactivateCharacter();
			return;
		} else {
			if (CheckValidShootingTarget(currentPlayerTarget)) {
				BeginPlanningShooting();
			} else {
				BeginPlanningMovement();
			}
		}
	}

	private void handleMovementPlanning() {
		var destPos = currentPlayerTarget.transform.position;

		// we can only aim as far as our shoot range. let's calculate that.
		var dist = Vector3.Distance(transform.position, destPos);
		var asdfadf = Mathf.Max(0.0f, Mathf.Min(1.0f, Mathf.Min(characterMeta.moveRange / dist, (characterMeta.moveRange / dist)))); // last part is "don't move closer than shoot range"
		destPos = Vector3.Lerp (transform.position, destPos, asdfadf);
		destPos = AstarPath.active.astarData.gridGraph.GetNearest(destPos).clampedPosition;

		//PlanMovement(currentPlayerTarget.transform.position);
		PlanMovement(destPos);
		BeginMoving();
	}

	private void drawMovement() {
		if (path != null && pathVector.Length > 1) {
			Vectrosity.VectorLine.SetLine3D(Color.green, 0.01f, pathVector);
		}
	}

	private void handleShootingPlanning() {

		// --------
		// do stuff
		// --------

		aimAtTarget();

		BeginShooting();
	}

	private void shootAtTarget() {

		// --------
		// do stuff
		// --------

		currentPlayerTarget.SendMessage("TakeDamage", characterMeta.Damage);

		EndShooting();
	}

	private void aimAtTarget() {
		pathVector = new Vector3[2];
		pathVector[0] = transform.position + (1.0f * Vector3.up);
		pathVector[1] = currentPlayerTarget.transform.position + (1.0f * Vector3.up);
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
