using UnityEngine;
using System.Collections;
using Pathfinding;

public class CanPlanGridMovement : MonoBehaviour {

	public Vector3 targetPosition;

	public Path path;

	public bool isPlanningMovement = false;
	public bool isDrawingMovement = false;

	public int moveRange = 0;

	private Seeker seeker;
	private CharacterSheet characterSheet = null;

	public Vector3 currentNodePosition;
	
	void Start () {
		seeker = GetComponent<Seeker>();

		characterSheet = GetComponent<CharacterSheet>();
		if (characterSheet != null) {
			moveRange = characterSheet.moveRange;
		}
	}

	void Update () {
		if (isPlanningMovement) {
			if (targetPosition != currentNodePosition) {
				targetPosition = currentNodePosition;
				seeker.StartPath(transform.position, targetPosition, OnPathComplete);
			}
		}

		if (path == null) {
			return;
		}

		if (isDrawingMovement) {
			GridDrawer.active.DrawPath(path.vectorPath.ToArray(), moveRange);
		}
	}

	public void OnPathComplete(Path p) {
		p.Claim(this);
		if (!p.error) {
			if (path != null) { path.Release(this); }
			path = p;
		} else {
			p.Release(this);
            Debug.Log("Target was not reachable.");
        }
	}
	
	public void BeginPlanningMovement() {
		isPlanningMovement = true;
		isDrawingMovement = true;
		targetPosition = Vector3.zero;
		path = null;
    }

	/**Returns whether or not the path should be traversed.
	 */
	public bool EndPlanningMovement() {
		isPlanningMovement = false;
		return hasValidPath();
    }

	public bool hasValidPath() {
		return (path != null && path.vectorPath.Count > 1);
	}
}
