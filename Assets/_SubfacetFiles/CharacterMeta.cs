using UnityEngine;
using System.Collections;

public class CharacterMeta : MonoBehaviour {
	
	public float Health = 3.0f;
	public float Damage = 1.0f;
	public int Team = 0;
	public float MoveRange = 15.0f;
	public float ShootRange = 30.0f;
	
	private HUDText hudText;

	// Use this for initialization
	void Start () {
		hudText = GetComponent<HUDText>();
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
		hudText.Add(-123f, Color.red, 0f);
		if (Damage <= 0.0f) {
			Die();
		}
	}
	
	public void Die() {
		Destroy(gameObject);
	}
}
