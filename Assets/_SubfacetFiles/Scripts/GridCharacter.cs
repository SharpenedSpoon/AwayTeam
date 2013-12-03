using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using HutongGames;

public class GridCharacter : GridObject {

	public bool LogEvents = false;

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
	protected bool isPlanningGathering = false;
	protected bool isGathering = false;
	protected Path path;
	//protected Path fullPath;
	protected Vector3[] pathVector;
	protected Vector3[] inRangePathVector;
	protected Vector3[] outOfRangePathVector;

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

		if (Input.GetKey(KeyCode.L)) {
			ExploderObject explode = GetComponent<ExploderObject>();
			if (explode != null) {
				explode.Explode();
			}
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

	/*public bool CheckValidShootingTarget(Vector3 targetPosition) {
		var output = false;
		var currentGridObject = gridInteraction.currentGridObject;
		if (Vector3.Distance(transform.position, targetPosition) <= characterMeta.shootRange) {
			if (currentGridObject != null && currentGridObject.name != gameObject.name) {
				output = true;
			}
		}
		return output;
	}*/

	public bool CheckValidTarget(Vector3 initialTargetPosition, float range) {
		var output = false;
		
		// aim at 1.0 unit above ground
		Vector3 targetPosition = initialTargetPosition;
		targetPosition.y = Terrain.activeTerrain.SampleHeight(targetPosition);
		targetPosition = targetPosition + new Vector3(0.0f, 1.0f, 0.0f);
		
		// if in range at all
		if (Vector3.Distance(transform.position, targetPosition) <= range) {
			
			// move the current object away to avoid accidental raycast hits
			Vector3 originalPosition = transform.position;
			//transform.position += new Vector3(0.0f, -1000.0f, 0.0f);
			if (Physics.Raycast(Vector3.Lerp(originalPosition, targetPosition, 0.2f), targetPosition - originalPosition, out hit, Vector3.Distance(originalPosition, initialTargetPosition))) {
				// we hit something
				if (gridGraph.GetNearest(hit.point).clampedPosition == gridGraph.GetNearest(initialTargetPosition).clampedPosition) {
					//var currentGridObject = gridInteraction.currentGridObject;
					//if (currentGridObject != null && currentGridObject.name != gameObject.name) {
					output = true;
					//}
				}
			} else {
				// we didn't hit anything
				output = true;
			}
			//transform.position = originalPosition;
			
		}
		return output;
	}

	public bool CheckValidShootingTarget(Vector3 initialTargetPosition) {
		return CheckValidTarget(initialTargetPosition, characterMeta.shootRange);
	}

	public bool CheckValidGatheringTarget(Vector3 initialTargetPosition) {
		if (CheckValidTarget(initialTargetPosition, characterMeta.gatherRange)) {
			if (gridInteraction.currentGridObject != null && gridInteraction.currentGridObject.name == "CollectibleResource") {
				return true;
			}
		}
		return false;
		//return CheckValidTarget(initialTargetPosition, characterMeta.gatherRange);
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

			// Create the in- and out-of- range drawing paths
			int moveNodeRange = characterMeta.MoveNodeRange;
			if (pathVector.Length <= moveNodeRange) {
				inRangePathVector = pathVector;
				outOfRangePathVector = new Vector3[0];
			} else {
				var inRange = new List<Vector3>();
				var outOfRange = new List<Vector3>();
				if (pathVector.Length - moveNodeRange > 1) { // we have to account for the fact that the pathVector is 1, because of the starting node
					outOfRange.Add(pathVector[moveNodeRange]);
				}
				//inRangePathVector = new Vector3[moveNodeRange];
				//outOfRangePathVector = new Vector3[pathVector.Length - moveNodeRange];
				for (int i = 0; i < pathVector.Length; i++) {
					if (i < moveNodeRange + 1) {
						inRange.Add(pathVector[i]);
					} else {
						outOfRange.Add(pathVector[i]);
					}
				}
				inRangePathVector = inRange.ToArray();
				outOfRangePathVector = outOfRange.ToArray();
			}
		}
	}

	public void MoveAlongPath() {
		if (path == null) {
			//We have no path to move after yet
			//return;
		} else  if (currentWaypoint >= path.vectorPath.Count || currentWaypoint > characterMeta.MoveNodeRange) {
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
		if (LogEvents) { Debug.Log("GridCharacter: BeginPlanningMovement"); }
	}
	
	public void EndPlanningMovement() {
		isPlanningMovement = false;
		if (LogEvents) { Debug.Log("GridCharacter: EndPlanningMovement"); }
	}
	
	public void BeginMoving() {
		EndPlanningMovement();
		isMoving = true;
		if (LogEvents) { Debug.Log("GridCharacter: BeginMoving"); }
	}
	
	public void EndMovement() {
		isMoving = false;
		isIdle = true;
		if (fsm != null) {
			fsm.SendEvent("NextMovePhase");
		}
		characterMeta.ChangeActions(-1);
		path = null;
		if (LogEvents) { Debug.Log("GridCharacter: EndMovement"); }
	}
	
	public void BeginPlanningShooting() {
		isIdle = false;
		isPlanningShooting = true;
		if (LogEvents) { Debug.Log("GridCharacter: BeginPlanningShooting"); }
	}
	
	public void EndPlanningShooting() {
		isPlanningShooting = false;
		if (LogEvents) { Debug.Log("GridCharacter: EndPlanningShooting"); }
	}
	
	public void BeginShooting() {
		EndPlanningShooting();
		isShooting = true;
		if (LogEvents) { Debug.Log("GridCharacter: BeginShooting"); }
	}
	
	public void EndShooting() {
		isShooting = false;
		isIdle = true;
		if (fsm != null) {
			fsm.SendEvent("NextShootPhase");
		}
		characterMeta.ChangeActions(-1);
		if (LogEvents) { Debug.Log("GridCharacter: EndShooting"); }
	}

	public void BeginPlanningGathering() {
		isPlanningGathering = true;
		if (LogEvents) { Debug.Log("GridCharacter: BeginPlanningGathering"); }
	}

	public void EndPlanningGathering() {
		isPlanningGathering = false;
		if (LogEvents) { Debug.Log("GridCharacter: EndPlanningGathering"); }
	}

	public void BeginGathering() {
		isGathering = true;
		if (LogEvents) { Debug.Log("GridCharacter: BeginGathering"); }
	}

	public void EndGathering() {
		isGathering = false;
		isIdle = true;
		if (fsm != null) {
			fsm.SendEvent("NextShootPhase");
		}
		characterMeta.ChangeActions(-1);
		if (LogEvents) { Debug.Log("GridCharacter: EndGathering"); }
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
