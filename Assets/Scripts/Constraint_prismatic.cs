using UnityEngine;
using System.Collections;
using System.IO;
using System.Windows.Forms;

public class Constraint_prismatic : MonoBehaviour {

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
		GameObject UIController = GameObject.Find("UI Controller");
		StreamReader xyz_data;
		try{
			xyz_data = new StreamReader(UIController.GetComponent<UI_Controller>().pointDataFilePath);
		}catch{
			Debug.LogWarning("Constraint_prismatic.cs: Point data file does not exist or cannot be read!");
			MessageBox.Show("Point data file does not exist or cannot be read!", "Error!");
			return;
		}

		string data;
		data = xyz_data.ReadLine();
		line.positionCount = 0;
		//Plot each point
		do{
			
			string[] pointData = data.Split(new char[] {','} );
			p[0] = double.Parse(pointData[0]);
			p[1] = double.Parse(pointData[2]);
			p[2] = double.Parse(pointData[1]);
			transform.position = new Vector3((float)p[0], (float)p[1], (float)p[2]);
			/*var point = Instantiate(pointPrefab, transform.position, Quaternion.identity);
			point.name = "Point " + (line.positionCount + 1).ToString();*/

			//Set a vertex for the line at the point
			line.SetPosition(line.positionCount++, transform.position);

			
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
			data = xyz_data.ReadLine();
		}while(data != null);

		//Move plane center to centroid
		if(n == 0){
			Debug.LogWarning("Constraint_prismatic.cs: Prismatic constraint cannot be placed!");
			MessageBox.Show("Prismatic constraint cannot be placed!", "Warning!");
			return;
		}
		cent[0]/=n;
		cent[1]/=n;
		cent[2]/=n;
		
		double[,] w = {{-wx},{wz},{-wy}};
		double[,] ew = new double[3, 3];
		float qx = 0, qy = 0, qz = 0, qw = 0;

		ew = Func.exp_map(w);
		Func.matToQ(ew, ref qx, ref qy, ref qz, ref qw);

		Quaternion rot = new Quaternion(qx, qy, qz, qw);
		Instantiate(linePrefab, new Vector3((float)cent[0], (float)cent[1], (float)cent[2]), rot);
	}
}