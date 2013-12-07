using UnityEngine;
using System.Collections;
using Pathfinding;

public class GridMovement : MonoBehaviour {

	/*private GridGraph gridGraph = null;

	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
	}
	
	void Update () {
		
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
	}*/
}
