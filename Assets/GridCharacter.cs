using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class GridCharacter : GridObject {

	// Gameobjects and components
	private CharacterMeta characterMeta;
	protected Seeker seeker;
	protected CharacterController controller;
	public float moveSpeed = 1.0f;
	public float turningSpeed = 1.0f;

	// Variables and bools and arrays
	protected bool isIdle = true;
	protected bool isPlanningMovement = false;
	protected bool isMoving = false;
	protected bool isPlanningShooting = false;
	protected bool isShooting = false;

	public override void Start () {
		base.Start();
		characterMeta = GetComponent<CharacterMeta>();
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
	}

	public override void Update () {
		base.Update();

		if (hasControl) {
			if (isIdle) {
				if (Input.GetKeyDown(KeyCode.M)) {
					BeginPlanningMovement();
				} else if (Input.GetKeyDown(KeyCode.S)) {
					BeginPlanningShooting();
				}
			} else if (isPlanningMovement) {
				if (Input.GetKeyDown(KeyCode.M)) {
					BeginMoving();
				}
			} else if (isPlanningShooting) {
				if (Input.GetKeyDown(KeyCode.S)) {
					BeginShooting();
				}
			}
		}
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
		isIdle = false;
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
		isIdle = true;
	}
	
	public void BeginPlanningShooting() {
		isIdle = true;
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
		isIdle = true;
	}

	protected virtual void RotateTowards (Vector3 dir) {
		Quaternion rot = tr.rotation;
		Quaternion toTarget = Quaternion.LookRotation(dir);
		
		rot = Quaternion.Slerp (rot, toTarget, turningSpeed*Time.fixedDeltaTime);
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		tr.rotation = rot;
	}
}
