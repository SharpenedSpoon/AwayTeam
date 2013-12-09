using UnityEngine;
using System.Collections;

public class CanMoveOnGrid : MonoBehaviour {

	public bool isMoving = false;
	
	public int moveRange = 0;
	public float moveSpeed = 30.0f;

	private float nextWaypointDistance = 0.5f;
	private int currentWaypoint = 0;
	private Vector3[] vectorPath;
	
	private Seeker seeker;
	private CharacterSheet characterSheet = null;
	private CanPlanGridMovement gridMovementPlanner = null;
	private GridManager gridManager = null;

	void Start () {
		seeker = GetComponent<Seeker>();
		
		characterSheet = GetComponent<CharacterSheet>();
		if (characterSheet != null) {
			moveRange = characterSheet.moveRange;
			moveSpeed = characterSheet.moveSpeed;
        }

		gridMovementPlanner = GetComponent<CanPlanGridMovement>();

		gridManager = GridManager.active;
	}

	void Update () {
		if (isMoving) {
			moveTowardsNextWaypoint();

		}
	}

	public void BeginMovement() {
		isMoving = true;
		vectorPath = gridMovementPlanner.path.vectorPath.ToArray();
		currentWaypoint = 0;
	}

	public void EndMovement() {
		isMoving = false;
	}

	public void moveTowardsNextWaypoint() {
		if (Vector3.Distance(transform.position, vectorPath[currentWaypoint]) < nextWaypointDistance) {
			// we want to go to next waypoint. also, update our position in the grid.
			currentWaypoint++;
			gridManager.SetGraphObjectPosition(gameObject);
			if (currentWaypoint > moveRange) {
				// stop moving, stop drawing the movement path, and reset to our clamped position
				transform.position = gridManager.GetGraphObjectPosition(gameObject);
				isMoving = false;
				gridMovementPlanner.isDrawingMovement = false;
				return;
			}
        }

		Vector3 dir = (vectorPath[currentWaypoint] - transform.position).normalized;
		dir *= moveSpeed * Time.deltaTime;
		transform.Translate(dir);
	}
}
