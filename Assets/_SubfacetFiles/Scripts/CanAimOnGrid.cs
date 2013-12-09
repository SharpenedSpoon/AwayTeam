using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CanFireWeapon))]
[RequireComponent (typeof (GridManager))]
[RequireComponent (typeof (GridDrawer))]

public class CanAimOnGrid : MonoBehaviour {

	private CanFireWeapon weaponFire;
	private GridManager gridManager;
	private GridDrawer gridDrawer;

	public Vector3 currentNodePosition;
	public Vector3 targetPosition;

	public float nodeSize;

	public bool isAiming = false;
	public bool isDrawingAim = false;

	private Vector3[] aimPath;
	private float aimHeight = 1.0f;
	private Vector3 aimLevel;
	public GameObject currentTarget = null;

	RaycastHit hit;

	public bool validTarget = false;
	public bool clearLineOfSight = false;
	public bool targetInRange = false;

	void Start () {
		weaponFire = GetComponent<CanFireWeapon>();
		gridManager = GridManager.active;
		gridDrawer = GridDrawer.active;
	}

	void Update () {
		if (isAiming) {
			if (targetPosition != currentNodePosition) {
				targetPosition = currentNodePosition;
				Vector3 targetLevel = targetPosition + (aimHeight * Vector3.up);
				float shootRange = weaponFire.weapon.rangeInNodes * nodeSize;
				currentTarget = null;

				// figure out if the thing we're aiming at is a valid target
				if (!gridManager.IsNodeFree(targetPosition)) {
					currentTarget = gridManager.GetGraphObject(targetPosition);
					if (!currentTarget.CompareTag(gameObject.tag)) {
						validTarget = true;
					} else {
						validTarget = false;
					}
				}

				// figure out if we can see the target
				if (Physics.Raycast(aimLevel, targetLevel - aimLevel, out hit, Vector3.Distance(transform.position, targetLevel), ~(1 << gameObject.layer))) {
					clearLineOfSight = false;
					aimPath = new Vector3[3];
					aimPath[0] = transform.position;
					aimPath[1] = hit.point;
					aimPath[1].y = Terrain.activeTerrain.SampleHeight(aimPath[1]);
					aimPath[2] = targetPosition;
				} else {
					clearLineOfSight = true;
					aimPath = new Vector3[2];
					aimPath[0] = transform.position;
					aimPath[1] = targetPosition;
				}

				// if target in range?
				if (Vector3.Distance(aimLevel, targetLevel) <= shootRange) {
					targetInRange = true;
				} else {
					targetInRange = false;
				}
			}
		}

		if (isDrawingAim) {
			if (aimPath.Length > 1) {
				gridDrawer.DrawPath(aimPath, 1);
			}
		}
	
	}

	public void BeginAiming() {
		isAiming = true;
		isDrawingAim = true;
        targetPosition = Vector3.zero;
		aimPath = new Vector3[2];
		
		nodeSize = gridDrawer.nodeSize;
		aimHeight = gridDrawer.drawForCharacterHeight;
		aimLevel = transform.position + (aimHeight * Vector3.up);
	}

	/**Returns whether or not we can shoot at the target
	 */
	public bool EndAiming() {
		isAiming = false;
		if (targetInRange && clearLineOfSight && validTarget) {
			return true;
		} else {
			isDrawingAim = false;
			return false;
		}
	}
}
