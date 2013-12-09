using UnityEngine;
using System.Collections;

public class HasHealth : MonoBehaviour {

	public int maxHP = 4;
	public int health { get; private set; }

	public bool explodeOnDeath = true;
	private ExploderObject exploderObject = null;
	
	void Start() {
		ResetHealth();
		exploderObject = GetComponent<ExploderObject>();
	}

	/** Take damage. Returns whether or not this damage was fatal
	 */
	public bool TakeDamage(int dmg) {
		health -= dmg;
		if (health <= 0) {
			Die();
			return true;
		} else {
			return false;
		}
	}

	/** Heal some health, without going over the maximum
	 */
	public void RegainHealth(int heal) {
		health += heal;
		if (health > maxHP) {
			health = maxHP;
		}
	}

	public void ResetHealth() {
		health = maxHP;
	}

	private void Die() {
		if (explodeOnDeath && exploderObject != null) {
			exploderObject.Explode();
		} else {
			Destroy(gameObject);
		}
	}
}
