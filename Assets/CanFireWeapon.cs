﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Weapon))]

public class CanFireWeapon : MonoBehaviour {
	
	private int aimStat = 10;
	private float aimChance = 1.0f;

	private int criticalStat = 0;
	private float criticalChance = 0.0f;
	private float criticalScale = 0.0f;

	private Weapon weapon;
	private CharacterSheet characterSheet = null;
	
	void Start() {
		weapon = GetComponent<Weapon>();
		characterSheet = GetComponent<CharacterSheet>();
		if (characterSheet != null) {
			aimStat = characterSheet.aim;
			criticalStat = characterSheet.critical;
		}
		aimChance = aimStat / 10;
		criticalChance = criticalStat / 10;
		criticalScale = criticalStat * 0.5f;
	}

	/** Possibly shoot at a target game object. Returns
	 * a bool for whether or not the shot hit.
	 */
	public bool Fire(GameObject targetObject) {
		HasHealth targetHealth = targetObject.GetComponent<HasHealth>();
		if (targetHealth == null) {
			return false;
		}

		if (weapon.HasAmmo() || Random.Range(0.0f, 1.0f) >= aimChance) {
			return false;
		} else {
			ShootAtTarget(targetHealth);
			weapon.ExpendAmmo(1);
			return true;
		}
	}

	/** Shoot at a target, passing the weapon damage and so forth.
	 */
	private void ShootAtTarget(HasHealth targetHealth) {
		int dmg = weapon.CalculateDamage(criticalChance, criticalScale);
		targetHealth.TakeDamage(dmg);
	}
	
}