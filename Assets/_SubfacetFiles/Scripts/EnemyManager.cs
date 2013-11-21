using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	// Gameobjects and components
	private PlayMakerFSM fsm;
	private GameObject currentEnemy = null;
	private List<GameObject> enemyCharacters;

	// Variables and arrays and bools
	private bool isEnemyTurn = false;
	
	void Start () {
		fsm = GetComponent<PlayMakerFSM>();
		enemyCharacters = new List<GameObject>();
		var gos = GameObject.FindGameObjectsWithTag("GridObject");
		foreach (GameObject go in gos) {
			if (go.name == "EnemyCharacter") {
				enemyCharacters.Add(go);
			}
		}
	}

	void Update () {
		if (isEnemyTurn) {
			if (enemyCharacters.Count == 0) {
				// if all enemy characters are dead, then the enemies get no turns
				EndEnemyTurn();
			} else {
				if (currentEnemy == null) { // we need to set a currentEnemy
					foreach (GameObject enemy in enemyCharacters) {
						// find the first enemy in the list that has actions, and set it to be the currentEnemy
						if (enemy.GetComponent<CharacterMeta>().actions > 0) {
							currentEnemy = enemy;
							break;
						}
					}

					if (currentEnemy == null) {
						// if there is still no currentEnemy, then all enemies are out of actions.
						EndEnemyTurn();
					} else {
						// if we have a currentEnemy, make them active
						currentEnemy.SendMessage("MakeActive");
					}
				}
			}
		}
	}

	public void BeginEnemyTurn() {
		isEnemyTurn = true;
		currentEnemy = null;
		enemyCharacters.RemoveAll(item => item == null);
		foreach (GameObject enemy in enemyCharacters) {
			enemy.SendMessage("ResetActions");
		}
	}

	public void EndEnemyTurn() {
		isEnemyTurn = false;
		currentEnemy = null;
		fsm.SendEvent("NextPhase");
	}
}
