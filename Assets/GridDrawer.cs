using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Vectrosity;

public class GridDrawer : MonoBehaviour {

	public Color neutralColor = Color.black;
	public Color validColor = Color.green;
	public Color invalidColor = Color.red;

	private GridGraph gridGraph = null;
	private float nodeSize = 0.0f;
	private Terrain terrain = null;

	public new static GridDrawer active;


	private float drawAboveTerrainHeight = 0.1f;
	private float drawForCharacterHeight = 2.5f;

	void Awake() {
		active = this;
	}

	void Start () {
		gridGraph = AstarPath.active.astarData.gridGraph;
		nodeSize = gridGraph.nodeSize;
		terrain = Terrain.activeTerrain;
	}

	public void DrawNode(Vector3 approximateNodePosition) {
		DrawNode(approximateNodePosition, neutralColor);
	}

	public void DrawNodeValid(Vector3 approximateNodePosition) {
		DrawNode(approximateNodePosition, true);
    }

	public void DrawNodeInvalid(Vector3 approximateNodePosition) {
		DrawNode(approximateNodePosition, false);
    }

	public void DrawNode(Vector3 approximateNodePosition, bool isValidNode) {
		if (isValidNode) {
			DrawNode(approximateNodePosition, validColor);
		} else {
			DrawNode (approximateNodePosition, invalidColor);
		}
	}

	public void DrawNode(Vector3 approximateNodePosition, Color nodeColor) {
		Vector3 nodeCenter = gridGraph.GetNearest(approximateNodePosition).clampedPosition;
		Vector3[] drawnSquare = new Vector3[5];
		drawnSquare[0] = nodeCenter + new Vector3(0.5f * nodeSize, 0.0f, 0.5f * nodeSize);
		drawnSquare[1] = nodeCenter + new Vector3(0.5f * nodeSize, 0.0f, -0.5f * nodeSize);
		drawnSquare[2] = nodeCenter + new Vector3(-0.5f * nodeSize, 0.0f, -0.5f * nodeSize);
		drawnSquare[3] = nodeCenter + new Vector3(-0.5f * nodeSize, 0.0f, 0.5f * nodeSize);
		drawnSquare[4] = drawnSquare[0];
		drawnSquare = AdjustToTerrainHeight(drawnSquare);
		drawnSquare = AddToArray(drawAboveTerrainHeight * Vector3.up, drawnSquare);
		Vectrosity.VectorLine.SetLine3D(nodeColor, 0.01f, drawnSquare);
	}

	public void DrawPath(Vector3[] vectorPath) {
		DrawPath(vectorPath, neutralColor);
	}

	public void DrawPath(Vector3[] vectorPath, int validPathLength) {
		if (vectorPath.Length <= validPathLength + 1) {
			// we can just draw the (valid) path straight up.
			DrawPath(vectorPath, true);
			return;
		} else if (validPathLength <= 1) {
			// we can just draw the (invalid) path straight up.
			DrawPath(vectorPath, false);
			return;
		}

		/* Example:
		 *   validPathLength = 3
		 * 
		 *   Path:
		 *    * -- * -- * -- * -- * -- * -- * -- *
		 *    0    1    2    3    4    5    6    7
		 *   |________________|
		 *           |
		 *        validPath
		 *                  |_____________________|
		 *                             |
		 *                         invalidPath
		 */

		List<Vector3> validPath = new List<Vector3>();
		List<Vector3> invalidPath = new List<Vector3>();

		for (int i = 0; i < vectorPath.Length; i++) {
			if (i < validPathLength) {
				validPath.Add(vectorPath[i]);
			} else if (i == validPathLength) {
				validPath.Add(vectorPath[i]);
				invalidPath.Add(vectorPath[i]);
			} else {
				invalidPath.Add(vectorPath[i]);
			}
		}

		/*for (int i = 0; i <= validPathLength + 1; i++) {
			validPath[i] = vectorPath[i];
		}
		for (int j = validPathLength + 1; j < vectorPath.Length; j++) {
			invalidPath[j - (validPathLength + 1)] = vectorPath[j];
		}*/

		DrawPath(validPath.ToArray(), true);
		DrawPath(invalidPath.ToArray(), false);
	}
	
	public void DrawPathValid(Vector3[] vectorPath) {
		DrawPath(vectorPath, true);
	}
	
	public void DrawPathInvalid(Vector3[] vectorPath) {
		DrawPath(vectorPath, false);
	}
	
	public void DrawPath(Vector3[] vectorPath, bool isValidPath) {
		if (isValidPath) {
			DrawPath(vectorPath, validColor);
		} else {
			DrawPath(vectorPath, invalidColor);
		}
	}

	public void DrawPath(Vector3[] vectorPath, Color pathColor) {
		if (vectorPath.Length <= 1) {
			// we don't have enough points to draw
			return;
		}

		Vectrosity.VectorLine.SetLine3D(pathColor, 0.01f, AddToArray(drawForCharacterHeight * Vector3.up, vectorPath));
	}

	private Vector3[] AddToArray(Vector3 vec, Vector3[] array) {
		Vector3[] output = array;
		for (int i=0; i<output.Length; i++) {
			output[i] = output[i] + vec;
		}
		return output;
	}

	private Vector3[] AdjustToTerrainHeight(Vector3[] inputPositions) {
		Vector3[] output = inputPositions;
		for (int i=0; i<output.Length; i++) {
			output[i].y = terrain.SampleHeight(output[i]);
		}
		return output;
	}
}
