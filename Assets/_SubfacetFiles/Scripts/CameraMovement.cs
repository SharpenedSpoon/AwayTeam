using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float CameraSpeed = 2.0f;
	private float horizontalMovement = 0.0f;
	private float verticalMovement = 0.0f;
	private float depthMovement = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetMovement();
		
	}
	
	private void GetMovement() {
		horizontalMovement = 0.0f;
		verticalMovement = 0.0f;
		depthMovement = 0.0f;
		if (Input.GetKey(KeyCode.LeftArrow)) {
			horizontalMovement -= 1.0f;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			horizontalMovement += 1.0f;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			verticalMovement -= 1.0f;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			verticalMovement += 1.0f;
		}
		if (Input.GetKey(KeyCode.F)) {
			depthMovement -= 1.0f;
		}
		if (Input.GetKey(KeyCode.R)) {
			depthMovement += 1.0f;
		}
		transform.position += new Vector3(horizontalMovement * CameraSpeed, depthMovement * CameraSpeed, verticalMovement * CameraSpeed);
	}
}
