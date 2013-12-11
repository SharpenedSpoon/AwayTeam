using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnController : MonoBehaviour {
	
	public GameObject playerCharacter = null;
	public GameObject enemyCharacter = null;
	public int playerCount = 3;
	public int enemyCount = 10;

	private MissionGeneration missionGeneration;
	private List<GameObject> playerTeam;
	private List<GameObject> enemyTeam;

	private GridDrawer gridDrawer;

	private GameObject activeCharacter = null;
	private TakesTurns activeCharacterTurn = null;
	private bool playerTurn = true;

	void Start () {
		missionGeneration = GetComponent<MissionGeneration>();
		playerTeam = missionGeneration.SpawnCharacters(playerCharacter, playerCount);
		enemyTeam = missionGeneration.SpawnCharacters(enemyCharacter, enemyCount);

		gridDrawer = GridDrawer.active;

		StartPlayerTurn();
	}

	void Update () {
		if (activeCharacter == null) {
			// Whose turn should it be?
			if (playerTurn) {
				activeCharacter = GetNextCharacterWithTurns(playerTeam);
				if (activeCharacter == null) {
					playerTurn = false;
					StartEnemyTurn();
				}
			} else {
				activeCharacter = GetNextCharacterWithTurns(enemyTeam);
				if (activeCharacter == null) {
					StartPlayerTurn();
                }
			}

			// If we now have an active character
			activeCharacterTurn = activeCharacter.GetComponent<TakesTurns>();
		} else {
			// We have an active character, let's figure out if they've still got turns
			if (activeCharacterTurn.OutOfActions()) {
				activeCharacter.SendMessage("MakeInactive");
				activeCharacter = null;
				activeCharacterTurn = null;
			} else {
				DrawActiveCharacter();
			}
		}
	}

	private void DrawActiveCharacter() {
		gridDrawer.DrawNode(activeCharacter.transform.position, Color.blue);
	}

	private void StartPlayerTurn() {
		playerTurn = true;
		for (int i=0; i<playerTeam.Count; i++) {
			playerTeam[i].SendMessage("ResetTurns");
		}
	}

	private void StartEnemyTurn() {
		playerTurn = false;
		for (int i=0; i<enemyTeam.Count; i++) {
			enemyTeam[i].SendMessage("ResetTurns");
        }
    }

	private GameObject GetNextCharacterWithTurns(List<GameObject> characterList) {
		GameObject output = null;
		for (int i=0; i<characterList.Count; i++) {
			if (!characterList[i].GetComponent<TakesTurns>().OutOfActions()) {
				output = characterList[i];
				CameraController.active.GoToPosition(output.transform.position);
				output.SendMessage("MakeActive");
				break;
			}
		}
		return output;
	}
}
