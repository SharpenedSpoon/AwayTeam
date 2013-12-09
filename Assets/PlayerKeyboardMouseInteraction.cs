using UnityEngine;
using System.Collections;

public class PlayerKeyboardMouseInteraction : MonoBehaviour {

	public bool isActive = true;

	private CanPlanGridMovement gridMovementPlanner;
	private CanMoveOnGrid gridMovement;

	private CanAimOnGrid gridAimer;

	void Start () {
		gridMovementPlanner = GetComponent<CanPlanGridMovement>();
		gridMovement = GetComponent<CanMoveOnGrid>();

		gridAimer = GetComponent<CanAimOnGrid>();
	}

	void Update () {

		if (isActive) {
			if (!(gridMovementPlanner.isPlanningMovement || gridMovement.isMoving || gridAimer.isAiming)) {
				// wait for this character to want to do something
				if (Input.GetKeyDown(KeyCode.Alpha1)) {
					gridMovementPlanner.BeginPlanningMovement();
				} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
					gridAimer.BeginAiming();
                }
			} else {
				if (gridMovementPlanner.isPlanningMovement) {
					gridMovementPlanner.currentNodePosition = GridMouseInteraction.active.currentNodePosition;
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
                } else if (gridAimer.isAiming) {
					gridAimer.currentNodePosition = GridMouseInteraction.active.currentNodePosition;
				}
            }
		}

	}
}
