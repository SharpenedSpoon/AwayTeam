using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CanPlanGridMovement))]
[RequireComponent (typeof (CanMoveOnGrid))]
[RequireComponent (typeof (CanAimOnGrid))]
[RequireComponent (typeof (CanShootOnGrid))]
[RequireComponent (typeof (TakesTurns))]

public class PlayerKeyboardMouseInteraction : MonoBehaviour {

	public bool isActive = true;

	private CanPlanGridMovement gridMovementPlanner;
	private CanMoveOnGrid gridMovement;

	private CanAimOnGrid gridAimer;
	private CanShootOnGrid gridShooter;

	private TakesTurns turns;

	void Start () {
		gridMovementPlanner = GetComponent<CanPlanGridMovement>();
		gridMovement = GetComponent<CanMoveOnGrid>();

		gridAimer = GetComponent<CanAimOnGrid>();
		gridShooter = GetComponent<CanShootOnGrid>();

		turns = GetComponent<TakesTurns>();
	}

	void Update () {

		if (isActive) {
			if (turns.OutOfActions()) {
				isActive = false;
			}
			if (!(gridMovementPlanner.isPlanningMovement || gridMovement.isMoving || gridAimer.isAiming)) {
				// if out of actions, we need to deactive the character
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
							turns.TakeTurn();
                        } else {
                            // cancel movement
                        }
                    }
                } else if (gridAimer.isAiming) {
					gridAimer.currentNodePosition = GridMouseInteraction.active.currentNodePosition;
					// wait for the player to confirm where they want to shoot
					if (Input.GetMouseButtonDown(0)) {
						if (gridAimer.EndAiming()) {
							// we want to shoot
							gridShooter.BeginShooting(gridAimer.currentTarget);
							turns.TakeTurn();
						} else {
							// cancel aiming
						}
					}
				}
            }
		}
	}
}
