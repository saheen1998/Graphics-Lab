using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public Transform target;

	private Vector3 targetCenter;

	private void Start() {
		
	}
	void Update () {

		gameObject.transform.LookAt(target.position);
	}
}
