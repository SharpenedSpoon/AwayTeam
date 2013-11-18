﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;
using Pathfinding;
using System.Linq;

public class GridPlayerCharacter : GridCharacter {

	// Gameobjects and Components

	// Variables and numbers
	private Vector3 lastMouseNodePosition = Vector3.zero;
	private bool validMovementPath = false;
	private bool validAimingPath = false;

	// Arrays

	// Colors
	private Color pathColor = Color.red;

	public override void Start () {
		base.Start();
		lastMouseNodePosition = Vector3.zero;
	}

	public override void Update () {
		if (gridInteraction.activeGridObject == gameObject) {
			if (isPlanningMovement) {
				handleMovementPlanning();
			} else if (isMoving) {
				drawMovement();
				MoveAlongPath();
			} else if (isPlanningShooting) {
				handleShootingPlanning();
			} else if (isShooting) {
				//
			}
		}
	}

	private void handleMovementPlanning() {
		if (lastMouseNodePosition != gridInteraction.currentNode) {
			lastMouseNodePosition = gridInteraction.currentNode;
			getMovementPath();
			checkValidMovement();
		}
		if (validMovementPath) {
			drawMovement();
		}
	}

	private void getMovementPath() {
		PlanMovement(lastMouseNodePosition);
		//path = seeker.StartPath(transform.position, gridInteraction.currentNode, OnPathComplete);
		//pathVector = path.vectorPath.ToArray();
		//pathVector = pathVector.Select(x => x + new Vector3(0.0f, 1.0f, 0.0f)).ToArray();
		//pathVector = addVectorToArray(pathVector, new Vector3(0.0f, 1.0f, 0.0f));
	}


	private void checkValidMovement() {
		if (path == null) {
			return;
		}
		if (pathVector.Length <= 1) {
			validMovementPath = false;
		} else {
			validMovementPath = CheckValidMovementPath(pathVector);
		}

		if (validMovementPath) {
			pathColor = Color.green;
		} else {
			pathColor = Color.red;
		}
	}

	private void drawMovement() {
		Vectrosity.VectorLine.SetLine3D(Color.green, 0.01f, pathVector);
	}

	private void handleShootingPlanning() {
		aimAtMouse();
		checkValidAiming();
		drawAiming();
	}
	
	private void aimAtMouse() {
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100.0f)) {
			return;
		}
		pathVector = new Vector3[2];
		pathVector[0] = transform.position + (1.0f * Vector3.up);
		pathVector[1] = hit.point + (1.0f * Vector3.up);
	}
	
	private void checkValidAiming() {
		validAimingPath = CheckValidShootingTarget(pathVector[1]);
		if (validAimingPath) {
			pathColor = Color.green;
		} else {
			pathColor = Color.red;
		}
	}
	
	private void drawAiming() {
		Vectrosity.VectorLine.SetLine3D(pathColor, 0.01f, pathVector);
	}
}
