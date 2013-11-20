using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using HutongGames;

public class GridCharacter : GridObject {

	// Gameobjects and components
	protected CharacterMeta characterMeta;
	protected Seeker seeker;
	protected CharacterController controller;
	protected PlayMakerFSM fsm;
	//public float moveSpeed = 1.0f;
	//public float turningSpeed = 1.0f;

	// Variables and bools and arrays
	protected bool isIdle = true;
	protected bool isPlanningMovement = false;
	protected bool isMoving = false;
	protected bool isPlanningShooting = false;
	protected bool isShooting = false;
	protected Path path;
	protected Vector3[] pathVector;

	//The AI's speed per second
	public float speed = 100;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 0.5f;
	
	//The waypoint we are currently moving towards
	protected int currentWaypoint = 0;

	public override void Start () {
		base.Start();
		characterMeta = GetComponent<CharacterMeta>();
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		fsm = GetComponent<PlayMakerFSM>();
	}

	public override void Update () {
		base.Update();

		if (isActive && characterMeta.actions <= 0) {
			Debug.Log("Out of actions!");
			DeactivateCharacter();
		}

		/*if (hasControl) {
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
		}*/
	}

	public void DeactivateCharacter() {
		MakeInactive();
		LoseControl();
		if (fsm != null) {
			fsm.SendEvent("OutOfActions");
		}
		gridInteraction.GainControl();
		gridInteraction.activeGridObject = null;
	}

	public bool CheckValidShootingTarget(GameObject targetObject) {
		var output = false;
		if (Vector3.Distance(transform.position, targetObject.transform.position) <= characterMeta.shootRange) {
			//var ray = new Ray(transform.position, targetObject.transform.position - transform.position);

			//if (Physics.Raycast(ray, out hit)) {
				//if (hit.rigidbody.gameObject == targetObject) {
					output = true;
				//}
			//}
		}
		return output;
	}

	public bool CheckValidShootingTarget(Vector3 targetPosition) {
		var output = false;
		var currentGridObject = gridInteraction.currentGridObject;
		if (Vector3.Distance(transform.position, targetPosition) <= characterMeta.shootRange) {
			if (currentGridObject != null && currentGridObject.name != gameObject.name) {
				output = true;
			}
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

	public bool CheckValidMovementPath(Path p) {
		var output = false;
		if (p.path.Count <= characterMeta.MoveNodeRange) {
			output = true;
		}
		return output;
	}

	public void PlanMovement(Vector3 movementTarget) {
		//return seeker.StartPath(transform.position, movementTarget);
		seeker.StartPath(transform.position, movementTarget, OnPathComplete);
	}

	public void OnPathComplete (Path p) {
		//We got our path back
		if (p.error) {
			//
		} else {
			//Yey, now we can get a Vector3 representation of the path
			//from p.vectorPath
			path = p;
			pathVector = p.vectorPath.ToArray();
			pathVector = addVectorToArray(pathVector, new Vector3(0.0f, 1.0f, 0.0f));
			currentWaypoint = 0;
		}
	}

	public void MoveAlongPath() {
		if (path == null) {
			//We have no path to move after yet
			//return;
		} else  if (currentWaypoint >= path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			transform.position = path.vectorPath[currentWaypoint-1];
			EndMovement();
			//return;
		} else {
		
			//Direction to the next waypoint
			Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			dir *= speed * Time.fixedDeltaTime;
			controller.SimpleMove(dir);
			
			//Check if we are close enough to the next waypoint
			//If we are, proceed to follow the next waypoint
			if (Vector3.Distance(transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
				currentWaypoint++;
				//return;
			}
		}
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
		if (fsm != null) {
			fsm.SendEvent("NextMovePhase");
		}
	}
	
	public void BeginPlanningShooting() {
		isIdle = false;
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
		if (fsm != null) {
			fsm.SendEvent("NextShootPhase");
		}
	}

	/*protected virtual void RotateTowards (Vector3 dir) {
		Quaternion rot = tr.rotation;
		Quaternion toTarget = Quaternion.LookRotation(dir);
		
		rot = Quaternion.Slerp (rot, toTarget, turningSpeed*Time.fixedDeltaTime);
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		tr.rotation = rot;
	}*/
}
