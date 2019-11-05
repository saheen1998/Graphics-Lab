using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamConstraint : MonoBehaviour
{
    public Transform cameraOrbit;
    
	private Vector3 targetCenter = Vector3.zero;

    void Start()
    {
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
		} else
		{
			Debug.LogWarning("CameraScript5.cs: Points not found!");
		}

        cameraOrbit.position = targetCenter;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        transform.LookAt(cameraOrbit.position);
    }
}
