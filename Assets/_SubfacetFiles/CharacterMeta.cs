using UnityEngine;
using System.Collections;

public class CharacterMeta : MonoBehaviour {
	
	public float Health = 3.0f;
	public float Damage = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GetShot(GameObject shooter) {
		TakeDamage(shooter.GetComponent<CharacterMeta>().Damage);
	}
	
	public void TakeDamage(float dmg) {
		Damage -= dmg;
		Debug.Log("Took " + dmg.ToString("G") + " damage!");
		if (Damage <= 0.0f) {
			Die();
		}
	}
	
	public void Die() {
		Destroy(gameObject);
	}
}
