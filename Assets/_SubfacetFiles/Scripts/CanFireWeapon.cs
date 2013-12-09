using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Weapon))]

public class CanFireWeapon : MonoBehaviour {
	
	private int aimStat = 10;
	private float aimChance = 1.0f;

	private int criticalStat = 0;
	private float criticalChance = 0.0f;
	private float criticalScale = 0.0f;

	public Weapon weapon { get; private set; }
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
	public bool Shoot(GameObject targetObject) {
		HasHealth targetHealth = targetObject.GetComponent<HasHealth>();
		if (targetHealth == null) {
			Debug.Log("Tried to shoot, but target had no health");
			return false;
		}

		if (!weapon.HasAmmo() || Random.Range(0.0f, 1.0f) >= aimChance) {
			Debug.Log("Shot, and missed!");
			return false;
		} else {
			// TODO: Before shooting, add a small wait time and simple effects
			Debug.Log("Shot, and hit!");
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
