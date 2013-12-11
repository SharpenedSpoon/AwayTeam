using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CanPlanGridMovement))]
[RequireComponent (typeof (CanMoveOnGrid))]
[RequireComponent (typeof (CanAimOnGrid))]
[RequireComponent (typeof (CanShootOnGrid))]
[RequireComponent (typeof (TakesTurns))]
[RequireComponent (typeof (CharacterSheet))]

public class EnemyArtificialIntelligence : MonoBehaviour {

	private CanPlanGridMovement gridMovementPlanner;
	private CanMoveOnGrid gridMovement;
	
	private CanAimOnGrid gridAimer;
	private CanShootOnGrid gridShooter;
	
	private TakesTurns turns;

	private CharacterSheet characterSheet;

	private GameObject closestPlayer = null;

	void Start () {
		gridMovementPlanner = GetComponent<CanPlanGridMovement>();
		gridMovement = GetComponent<CanMoveOnGrid>();
		
		gridAimer = GetComponent<CanAimOnGrid>();
		gridShooter = GetComponent<CanShootOnGrid>();
		
        turns = GetComponent<TakesTurns>();

		characterSheet = GetComponent<CharacterSheet>();
	}

	void Update () {
		if (turns.isActive) {
			if (!(gridMovementPlanner.isPlanningMovement || gridMovement.isMoving || gridAimer.isAiming)) {
				// wait for this character to want to do something
				closestPlayer = GetClosestPlayer();
				if (closestPlayer == null) {
					// couldn't find any player characters. exit function early.
					turns.TakeTurn();
					return;
				}

				// begin aiming so we can see if the target is in range
				gridAimer.BeginAiming();
				gridAimer.currentNodePosition = closestPlayer.transform.position;
				gridAimer.FindAimTarget();
				if (gridAimer.CanShootTarget(closestPlayer)) {
					// we can (likely) shoot the target! do so.
					if (gridAimer.EndAiming()) {
						gridShooter.BeginShooting(closestPlayer);
					}
				} else {
					// target is too far away, let's try and move closer
					gridMovementPlanner.BeginPlanningMovement();
				}


				if (Input.GetKeyDown(KeyCode.Alpha1)) {
					gridMovementPlanner.BeginPlanningMovement();
				} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
					gridAimer.BeginAiming();
				}
			} else {
				if (gridMovementPlanner.isPlanningMovement) {
					gridMovementPlanner.currentNodePosition = closestPlayer.transform.position;
					// wait for the player to confirm where they want to move
					if (gridMovementPlanner.hasValidPath()) {
						if (gridMovementPlanner.EndPlanningMovement()) {
							// we want to start moving
							gridMovement.BeginMovement();
							//turns.TakeTurn();
						} else {
							// cancel movement
						}
					}
				} /*else if (gridAimer.isAiming) {
					gridAimer.currentNodePosition = GridMouseInteraction.active.currentNodePosition;
					// wait for the player to confirm where they want to shoot
                    if (Input.GetMouseButtonDown(0)) {
                        if (gridAimer.EndAiming()) {
                            // we want to shoot
                            gridShooter.BeginShooting(gridAimer.currentTarget);
                            //turns.TakeTurn();
                        } else {
                            // cancel aiming
                        }
                    }
                }*/
            }
        }
	}

    private GameObject GetClosestPlayer() {
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		closestPlayer = null;
		if (allPlayers.Length == 0) {
			return closestPlayer;
		}

		for (int i=0; i<allPlayers.Length; i++) {
			if (closestPlayer == null || Vector3.Distance(transform.position, allPlayers[i].transform.position) < Vector3.Distance(transform.position, closestPlayer.transform.position)) {
				closestPlayer = allPlayers[i];
			}
		}
		return closestPlayer;
	}
}
