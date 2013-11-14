using UnityEngine;
using System.Collections;
using Pathfinding;

public class CharacterMeta : MonoBehaviour {
	
	public float Health = 3.0f;
	public float Damage = 1.0f;
	public int Team = 0;
	public int MoveNodeRange = 3;
	public int ShootNodeRange = 10;
	public float moveRange { get; private set; }
	public float shootRange { get; private set; }

	private GridGraph gridGraph;
	private float nodeSize;
	
	private HUDText hudText;

	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
		nodeSize = gridGraph.nodeSize;
		hudText = GetComponent<HUDText>();

		moveRange = MoveNodeRange * nodeSize;
		shootRange = ShootNodeRange * nodeSize;
	}

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
	
	public bool IsValidTarget(GameObject target) {
		if (target != null) {
			return (target.GetComponent<CharacterMeta>().Team != Team);
		} else {
			return false;
		}
	}
}
