using UnityEngine;
using System.Collections;

public class CameraScript2 : MonoBehaviour {

	public int speed;
	public int zoom;

	private Vector3 targetCenter = Vector3.zero;

	private void Start() {
		
		float maxLen = 1;
		
		//transform.position = new Vector3(penObject.position.x - distFromPen, penObject.position.y + distFromPen/2, penObject.position.z - distFromPen);
		var objects = GameObject.FindGameObjectsWithTag("Point");
		if(objects.Length != 0){
			
			Transform[] targets = new Transform[objects.Length];
			for(int i = 0; i<objects.Length; i++){
				targets[i] = objects[i].GetComponent<Transform>();
			}

			var bounds = new Bounds(targets[0].position, Vector3.zero);
			for(int i = 0; i<objects.Length; i++){
				bounds.Encapsulate(targets[i].position);
			}

			targetCenter = bounds.center;

			for(int i = 0; i<objects.Length; i++){
				float len = Vector3.Distance(bounds.center, targets[i].position);
				if (len > maxLen)
					maxLen = len;
			}
		} else
		{
			Debug.LogWarning("CameraScript2.cs: Points not found!");
		}

		transform.position = targetCenter;
		transform.LookAt(targetCenter);
		transform.position = new Vector3(transform.position.x, targetCenter.y, targetCenter.z - maxLen - zoom);
	}
	void Update () {

		transform.RotateAround(targetCenter, Vector3.up, speed * Time.deltaTime * 10);
	}
}
