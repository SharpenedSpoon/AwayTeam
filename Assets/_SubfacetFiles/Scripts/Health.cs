using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int HP = 3;

	private ExploderObject _exploderObject = null;

	void Start () {
		_exploderObject = GetComponentInChildren<ExploderObject>();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.M)) {
			Die();
		}
	}

	public void TakeDamage(int dmg) {
		HP -= dmg;
		if (HP <= 0) {
			Die();
		}
	}

	public void Die() {
		if (_exploderObject != null) {
			_exploderObject.Explode();
		} else {
			Destroy(gameObject);
		}
	}
}
