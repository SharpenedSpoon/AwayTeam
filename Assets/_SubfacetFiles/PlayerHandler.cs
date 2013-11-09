using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Vectrosity;
using HutongGames.PlayMaker;

public class PlayerHandler : AIPath {
	
	public int MaxActions = 2;
	
	private GameObject pathfindingObject = null;
	private GameObject enemyTarget = null;
	private CharacterMeta characterMeta = null;
	private PlayMakerFSM fsm = null;
	private bool isPlanningMovement = false;
	private bool isMoving = false;
	private bool isPlanningShooting = false;
	private bool isShooting = false;
	private Vector3[] pathForDisplay;
	private Color pathColor = Color.black;
	public bool validMovementPath { get; private set; }
	public bool validShootingPath { get; private set; }
	
	private RaycastHit hit;
	//private Path shortPath;
	private GridGraph gridGraph;
	
	private int actions = 0;
	
	
	// Use this for initialization
	void Start () {
		validMovementPath = false;
		validShootingPath = false;
		
		pathfindingObject = GameObject.Find("PathfindingTarget");
		gridGraph = AstarPath.active.astarData.gridGraph;
		characterMeta = GetComponent<CharacterMeta>();
		fsm = GetComponent<PlayMakerFSM>();
		
		// Set AIPath variables
		target = pathfindingObject.transform;
		canMove = false;
		canSearch = false;
		repathRate = 0.01f;
		
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		// put the pathfinding object where the mouse is
		if (isPlanningMovement || isPlanningShooting) {
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f)) {
				return;
			}
			if (hit.rigidbody != pathfindingObject.rigidbody) {
				pathfindingObject.transform.position = gridGraph.GetNearest(hit.point).clampedPosition;
			}
		}
		
		// determine if the given path is valid
		if (isPlanningMovement && path != null) {
			if (path.path.Count > characterMeta.MoveRange) {
				validMovementPath = false;
				pathColor = Color.red;
			} else {
				validMovementPath = true;
				pathColor = Color.green;
			}
			
			// draw the path
			pathForDisplay = addVectorToList(path.vectorPath.ToArray(), new Vector3(0.0f, 1.0f, 0.0f));
			Vectrosity.VectorLine.SetLine3D(pathColor, repathRate, pathForDisplay);
		} else {
			validMovementPath = false;
		}
		
	}
	
	public void BeginPlayerTurn() {
		actions = 0;
	}
	
	public void ChangeActions(int deltaActions) {
		actions = Mathf.Clamp(actions + deltaActions, 0, MaxActions);
	}
	
	public void StartPlanningMovement() {
		canSearch = true;
		isPlanningMovement = true;
	}
	
	public void StartMoving() {
		canSearch = false;
		canMove = true;
		isPlanningMovement = false;
		isMoving = true;
	}
	
	public override void OnTargetReached ()
	{
		base.OnTargetReached();
		canMove = false;
	}
	
	private Vector3[] addVectorToList(Vector3[] vectorArray, Vector3 vectorToAdd) {
		var vectorList = new List<Vector3>();
		foreach (Vector3 vec in vectorArray) {
			vectorList.Add(vec + vectorToAdd);
		}
		return vectorList.ToArray();
	}
}