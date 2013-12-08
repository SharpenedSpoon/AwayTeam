using UnityEngine;
using System.Collections;
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


	public float drawAboveTerrainHeight = 0.1f;
	public float drawForCharacterHeight = 1.0f;

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
		DrawNode(approximateNodePosition, validColor);
    }

	public void DrawNodeInvalid(Vector3 approximateNodePosition) {
		DrawNode(approximateNodePosition, invalidColor);
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
