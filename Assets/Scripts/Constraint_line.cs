using UnityEngine;
using System.Collections;

public class Constraint_line : MonoBehaviour {

	public Transform pointPrefab;
	public Transform linePrefab;
	public string dataFileName;

	public double wx, wy, wz, dx, dy, dz;
	

	// Use this for initialization
	void Start () {
		
		double[] p = new double[3];
		double[] cent = new double[3];
		int n = 0;

		LineRenderer line = gameObject.GetComponent<LineRenderer>();

		//Get each row from csv data file
		TextAsset xyz_data = Resources.Load<TextAsset>(dataFileName);
		if(xyz_data == null){
			Debug.LogError("Constraint_line.cs: Point data file does not exist or cannot be read!");
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
			transform.position = new Vector3((float)p[0], (float)p[1], (float)p[2]);
			var point = Instantiate(pointPrefab, transform.position, Quaternion.identity);
			point.name = "Point " + (i + 1).ToString();

			//Set a vertex for the line at the point
			line.SetPosition(i,transform.position);

			
			double[,] norm1 = {{1}, {0}, {0}};
			double[,] norm2 = {{0}, {0}, {1}};
			double resConstraint1 = Func.check_constraint_plane(wx, wy, wz, dx, dy, dz, norm1, p);
			double resConstraint2 = Func.check_constraint_plane(wx, wy, wz, dx, dy, dz, norm2, p);
			//Debug.Log(resConstraint1 + " , " + resConstraint2);
			//Find centroid
			if(resConstraint1 < 0.001 && resConstraint1 > -0.001 && resConstraint2 < 0.001 && resConstraint2 > -0.001){
				cent[0]+=p[0];
				cent[1]+=p[1];
				cent[2]+=p[2];
				n++;
			}
		}

		//Move plane center to centroid
		cent[0]/=n;
		cent[1]/=n;
		cent[2]/=n;
		
		double[,] w = {{-wx},{wz},{-wy}};
		double[,] ew = new double[3, 3];
		float qx = 0, qy = 0, qz = 0, qw = 0;

		ew = Func.exp_map(w);
		Func.matToQ(ew, ref qx, ref qy, ref qz, ref qw);

		Quaternion rot = new Quaternion(qx, qy, qz, qw);
		Transform obj_line;
		obj_line = (Transform)Instantiate(linePrefab, new Vector3((float)cent[0], (float)cent[1], (float)cent[2]), rot);
	}
}