using UnityEngine;
using System.Collections;
using Pathfinding;

public class CharacterMeta : MonoBehaviour {
	
	public float Health = 3.0f;
	public float Damage = 1.0f;
	public int Team = 0;
	public int MoveNodeRange = 3;
	public int ShootNodeRange = 10;
	public int GatherNodeRange = 1;
	public int MaxActions = 2;
	[HideInInspector]
	public int actions;
	public float moveRange { get; private set; }
	public float shootRange { get; private set; }
	public float gatherRange { get; private set; }

	private GridGraph gridGraph;
	private float nodeSize;
	
	private HUDText hudText;

	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
		nodeSize = gridGraph.nodeSize;
		hudText = GetComponent<HUDText>();

		moveRange = MoveNodeRange * nodeSize;
		shootRange = ShootNodeRange * nodeSize;
		gatherRange = GatherNodeRange * nodeSize;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.L)) {
			Die ();
		}
	}
	
	public void GetShot(GameObject shooter) {
		TakeDamage(shooter.GetComponent<CharacterMeta>().Damage);
	}
	
	public void TakeDamage(float dmg) {
		Health -= dmg;
		Debug.Log("Took " + dmg.ToString("G") + " damage!");
		if (hudText != null) {
			hudText.Add(dmg, Color.red, 10f);
		}
		if (Health <= 0.0f) {
			Debug.Log("Oh no! I died!");
			Die();
		}
	}
	
	public void Die() {
		var exploder = GetComponent<ExploderObject>();
		if (exploder != null) {
			exploder.Explode();
		} else {
			Destroy(gameObject);
		}
	}
	
	public bool IsValidTarget(GameObject target) {
		if (target != null) {
			return (target.GetComponent<CharacterMeta>().Team != Team);
		} else {
			return false;
		}
	}

	public void ResetActions() {
		actions = MaxActions;
	}

	public void ChangeActions(int amountToChange) {
		actions += amountToChange;
	}
}
