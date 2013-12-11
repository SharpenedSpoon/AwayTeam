using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public new static CameraController active;

	void Start() {
		active = this;
	}

	void Update() {
		int horizontal = 0;
		int vertical = 0;
		if (Input.GetKey (KeyCode.W)) { vertical += 1; }
		if (Input.GetKey (KeyCode.S)) { vertical -= 1; }
		if (Input.GetKey (KeyCode.D)) { horizontal += 1; }
		if (Input.GetKey (KeyCode.A)) { horizontal -= 1; }

		transform.transform.position += vertical * Vector3.forward + horizontal * Vector3.right;
	}

	public void GoToPosition(Vector3 pos) {
		transform.position = pos + new Vector3(0.0f, 21.0f, -12.0f);
	}
}
