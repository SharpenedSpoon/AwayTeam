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
				currentTarget = null;

				// figure out if the thing we're aiming at is a valid target

				FindAimTarget();

				if (currentTarget != null) {
					validTarget = IsValidTarget(currentTarget);
					clearLineOfSight = HasLineOfSight(currentTarget);
					targetInRange = InRange(currentTarget);
				}
			}
		}

		if (isDrawingAim) {
			if (aimPath.Length > 1) {
				gridDrawer.DrawPath(aimPath, 1);
			}
		}
	
	}

	public bool IsValidTarget(GameObject testObject) {
		return (!testObject.CompareTag(gameObject.tag));
	}

	public bool HasLineOfSight(GameObject testObject) {
		bool output = false;
		Vector3 targetLevel = testObject.transform.position + (aimHeight * Vector3.up);
		float shootRange = weaponFire.weapon.rangeInNodes * nodeSize;
		if (Physics.Raycast(aimLevel, targetLevel - aimLevel, out hit, Vector3.Distance(transform.position, targetLevel), ~(1 << gameObject.layer))) {
			output = false;
			aimPath = new Vector3[3];
			aimPath[0] = transform.position;
			aimPath[1] = hit.point;
			aimPath[1].y = Terrain.activeTerrain.SampleHeight(aimPath[1]);
			aimPath[2] = targetPosition;
		} else {
			output = true;
			aimPath = new Vector3[2];
			aimPath[0] = transform.position;
			aimPath[1] = targetPosition;
		}
		return output;
	}

	public bool InRange(GameObject testObject) {
		Vector3 targetLevel = testObject.transform.position + (aimHeight * Vector3.up);
		float shootRange = weaponFire.weapon.rangeInNodes * nodeSize;
		return Vector3.Distance(aimLevel, targetLevel) <= shootRange;
	}

	public bool CanShootTarget(GameObject testObject) {
		return (IsValidTarget(testObject) && HasLineOfSight(testObject) && InRange(testObject));
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

	public void FindAimTarget() {
		if (!gridManager.IsNodeFree(targetPosition)) {
			currentTarget = gridManager.GetGraphObject(targetPosition);
		}
	}

	/**Returns whether or not we can shoot at the target
	 */
	public bool EndAiming() {
		isAiming = false;
		if (currentTarget != null && CanShootTarget(currentTarget)) {
			return true;
		} else {
			isDrawingAim = false;
			return false;
		}
	}
}
