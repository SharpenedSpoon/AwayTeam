using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCharacter : GridObject {

	// Gameobjects and components
	private CharacterMeta characterMeta;

	public override void Start () {
		base.Start();
		characterMeta = GetComponent<CharacterMeta>();
	}

	public override void Update () {
		base.Update();
	
	}

	public bool CheckValidShootingTarget(GameObject targetObject) {
		var output = false;
		if (Vector3.Distance(transform.position, targetObject.transform.position) <= characterMeta.shootRange) {
			var ray = new Ray(transform.position, targetObject.transform.position - transform.position);

			if (Physics.Raycast(ray, out hit)) {
				if (hit.rigidbody.gameObject == targetObject) {
					output = true;
				}
			}
		}
		return output;
	}

	public bool CheckValidShootingTarget(Vector3 targetPosition) {
		var output = false;
		if (Vector3.Distance(transform.position, targetPosition) <= characterMeta.shootRange) {
			output = true;
		}
		return output;
	}
}
