using UnityEngine;
using System.Collections;

public class PlayerKeyboardMouseInteraction : MonoBehaviour {

	public bool isActive = true;

	private CanPlanGridMovement gridMovementPlanner;
	private CanMoveOnGrid gridMovement;

	void Start () {
		gridMovementPlanner = GetComponent<CanPlanGridMovement>();
		gridMovement = GetComponent<CanMoveOnGrid>();
	}

	void Update () {
		if (isActive) {
			if (!gridMovementPlanner.isPlanningMovement && !gridMovement.isMoving) {
				// wait for this character to want to do something
				if (Input.GetKeyDown(KeyCode.Alpha1)) {
					gridMovementPlanner.BeginPlanningMovement();
				} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
					//gridMovementPlanner.BeginPlanningMovement();
                }
			} else {
				if (gridMovementPlanner.isPlanningMovement) {
					// wait for the player to confirm where they want to move
					if (Input.GetMouseButtonDown(0)) {
						if (gridMovementPlanner.EndPlanningMovement()) {
							// we want to start moving
							gridMovement.BeginMovement();
                        } else {
                            // cancel movement, stop drawing the path.
                            gridMovementPlanner.isDrawingMovement = false;
                        }
                    }
                }
            }
		}

	}
}
