using UnityEngine;
using System.Collections;

public class Constraint_axial : MonoBehaviour {

	public Transform pointPrefab;
	public GameObject torusPrefab;
	public string dataFileName;

	public double wx, wy, wz, dx, dy, dz, radius;

	private Transform torusTransform;
	
	// Use this for initialization
	void Start () {
		
		double[] p = new double[3];

		LineRenderer line = gameObject.GetComponent<LineRenderer>();

		//Get each row from csv data file
		TextAsset xyz_data = Resources.Load<TextAsset>(dataFileName);
		if(xyz_data == null){
			Debug.LogError("Constraint_axial.cs: Point data file does not exist or cannot be read!");
			return;
		}
		string[] data = xyz_data.text.Split(new char[] {'\n'} );

		//Set number of vertices for line
		line.positionCount = data.Length-1;

		//Plot each point
		for(int i = 0; i<data.Length-1 ; i++){
			
			string[] pointData = data[i].Split(new char[] {','} );
			p[0] = double.Parse(pointData[0]);
			p[1] = double.Parse(pointData[2]);
			p[2] = double.Parse(pointData[1]);
			transform.position = new Vector3((float)p[0]*100, (float)p[1]*100, (float)p[2]*100);
			var point = Instantiate(pointPrefab, transform.position, Quaternion.identity);
			point.name = "Point " + (i + 1).ToString();

			//Set a vertex for the line at the point
			line.SetPosition(i,transform.position);

			//Check if point is on constraint
			//int resConstraint = Func.check_constraint_axial(wx, wy, wz, dx, dy, dz, radius, p);
		}

		//Move constraint center to centroid
		
		double[,] w = {{-wx},{wz},{-wy}};
		double[,] ew = new double[3, 3];
		float qx = 0, qy = 0, qz = 0, qw = 0;

		ew = Func.exp_map(w);
		Func.matToQ(ew, ref qx, ref qy, ref qz, ref qw);

		Quaternion rot = new Quaternion(qx, qy, qz, qw);
		GameObject obj_axialTorus;
		Vector3 center = new Vector3((float)dx*100, (float)dz*100, (float)dy*100);
		obj_axialTorus = (GameObject)Instantiate(torusPrefab, center, rot);
		//torusTransform = obj_axialTorus.GetComponent<Transform>();
		//torusTransform.Rotate(-90,0,0,Space.Self);
		ParticleSystem.ShapeModule ring = obj_axialTorus.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().shape;
		ring.radius = Mathf.Abs((float)radius*100);
	}
}
