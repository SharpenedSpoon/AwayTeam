﻿using UnityEngine;
using System.Collections;

public class CanShootOnGrid : MonoBehaviour {

	private CanAimOnGrid gridAimer;
	private CanFireWeapon weaponShooter;

	void Start () {
		gridAimer = GetComponent<CanAimOnGrid>();
		weaponShooter = GetComponent<CanFireWeapon>();
	}

	public void BeginShooting(GameObject targetObject) {
		// eventually this will be a bit more complex.
		weaponShooter.Shoot(targetObject);
		gridAimer.isDrawingAim = false;
		Debug.Log("Bang!");
	}
}
