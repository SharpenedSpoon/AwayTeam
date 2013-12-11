using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionGeneration : MonoBehaviour {

	public List<GameObject> SpawnCharacters(GameObject character, int num) {
		List<GameObject> output = new List<GameObject>();
		if (character != null) {
			for (int i=0; i<num; i++) {
				Vector3 thisPosition = new Vector3(Random.Range(50.0f, 100.0f), 0.0f, Random.Range(50.0f, 100.0f));
				thisPosition.y = Terrain.activeTerrain.SampleHeight(thisPosition);
				output.Add(Instantiate(character, thisPosition, Quaternion.identity) as GameObject);
			}
		}
		return output;
	}
}
