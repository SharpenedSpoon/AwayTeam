using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public int rangeInNodes = 5;
	public int minDamage = 1;
	public int maxDamage = 2;
	public float criticalChance = 0.1f;
	public float criticalBonusDamageScale = 0.5f;
	public bool usesAmmo = true;
	public int maxAmmo = 10;
	public int ammo { get; private set; }

	void Start() {
		Reload();
	}
	
    /** Calculates the damage of the weapon, including extra crit damage
     */
	public int CalculateDamage(float critChance = 0.0f, float critBonusDamageScale = 0.0f) {
		int damage = Random.Range(minDamage, maxDamage);
		if (Random.Range(0.0f, 1.0f) < critChance) {
            damage = Mathf.CeilToInt(damage + (damage * critBonusDamageScale));
        }
        return damage;
	}
	
	public void ExpendAmmo(int ammoUsed = 0) {
		if (usesAmmo) {
			ammo = Mathf.Max(0, ammo - ammoUsed);
		}
	}

	public void Reload() {
		ammo = maxAmmo;
	}

	public bool HasAmmo() {
		return (!usesAmmo || ammo > 0); 
	}
	
}
