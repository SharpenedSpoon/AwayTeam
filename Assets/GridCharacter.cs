using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class GridCharacter : GridObject {

	// Gameobjects and components
	private CharacterMeta characterMeta;
	protected Seeker seeker;

	// Variables and bools and arrays
	protected bool isPlanningMovement = false;
	protected bool isMoving = false;
	protected bool isPlanningShooting = false;
	protected bool isShooting = false;

	public override void Start () {
		base.Start();
		characterMeta = GetComponent<CharacterMeta>();
		seeker = GetComponent<Seeker>();
	}

	public override void Update () {
		base.Update();
	
	}

	public bool CheckValidShootingTarget(GameObject targetObject) {
		var output = false;
		if (Vector3.Distance(transform.position, targetObject.transform.position) <= characterMeta.shootRange) {
			var ray = new Ray(transform.position, targetObject.transform.position - transform.position);

			if (Physics.Raycast(ray, out hit)) {
				if (hit.rigidbody.gameObject == targetObject) {
					output = true;
				}
			}
		}
		return output;
	}

	public bool CheckValidShootingTarget(Vector3 targetPosition) {
		var output = false;
		if (Vector3.Distance(transform.position, targetPosition) <= characterMeta.shootRange) {
			output = true;
		}
		return output;
	}

	public bool CheckValidMovementPath(Vector3[] movementPath) {
		var output = false;
		if (movementPath.Length <= characterMeta.moveRange) {
			output = true;
		}
		return output;
	}

	public Path planMovement(Vector3 movementTarget) {
		return seeker.StartPath(transform.position, movementTarget);
	}

	public void BeginPlanningMovement() {
		isPlanningMovement = true;
	}
	
	public void EndPlanningMovement() {
		isPlanningMovement = false;
	}
	
	public void BeginMoving() {
		EndPlanningMovement();
		isMoving = true;
	}
	
	public void EndMovement() {
		isMoving = false;
	}
	
	public void BeginPlanningShooting() {
		isPlanningShooting = true;
	}
	
	public void EndPlanningShooting() {
		isPlanningShooting = false;
	}
	
	public void BeginShooting() {
		EndPlanningShooting();
		isShooting = true;
	}
	
	public void EndShooting() {
		isShooting = false;
	}
}
