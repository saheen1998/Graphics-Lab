using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Windows.Forms;
using System.Linq;

public class RobotControllerScript : MonoBehaviour
{
    public float animationSpeed = 2;
    public enum enMotion{regular, dab};
    public enMotion motion = enMotion.regular;
    public string dataFileName;
    public Transform tipTranform;

    public Transform L0;
    public Transform L1;
    public Transform L2;
    public Transform L3;
    public Transform L4;
    public Transform L5;
    public Transform L6;

    // public float angle0 = 0;
    // public float angle1 = 0;
    // public float angle2 = 0;
    // public float angle3 = 0;
    // public float angle4 = 0;
    // public float angle5 = 0;
    // public float angle6 = 0;
    
    public Animation anim;
    
    private AnimationClip clip;
    private AnimationClip dabClip;
    private int n_data;
    private TrailRenderer trail;

    private GraphScript gsc;
    private List<double> d0 = new List<double>();
    private List<double> d1 = new List<double>();
    private List<double> d2 = new List<double>();
    private List<double> d3 = new List<double>();
    private List<double> d4 = new List<double>();
    private List<double> d5 = new List<double>();
    private List<double> d6 = new List<double>();

    void RotateJoint(Transform arm, float jointAngle)
    {
        arm.localRotation = Quaternion.Euler(arm.localEulerAngles.x, jointAngle, arm.localEulerAngles.z);
    }

    void AddAnim(string rPath, Transform arm, int armNo, double[,] d, ref AnimationClip clip)
    {
        AnimationCurve xCurve;
        AnimationCurve yCurve;
        AnimationCurve zCurve;

        Keyframe[] xKeys = new Keyframe[n_data];
        Keyframe[] yKeys = new Keyframe[n_data];
        Keyframe[] zKeys = new Keyframe[n_data];

        float keyMultiplier = 10f/n_data;
        
        for (int i=0; i<n_data; i++)
        {
            xKeys[i] = new Keyframe(i*keyMultiplier, arm.localEulerAngles.x);
            if (armNo == 2 || armNo == 4)
                yKeys[i] = new Keyframe(i*keyMultiplier, (float)d[i,armNo]*180/Mathf.PI);
            else
                yKeys[i] = new Keyframe(i*keyMultiplier, -(float)d[i,armNo]*180/Mathf.PI);
            zKeys[i] = new Keyframe(i*keyMultiplier, arm.localEulerAngles.z);
        }

        xCurve = new AnimationCurve(xKeys);
        yCurve = new AnimationCurve(yKeys);
        zCurve = new AnimationCurve(zKeys);
        clip.SetCurve(rPath, typeof(Transform), "localEulerAngles.x", xCurve);
        clip.SetCurve(rPath, typeof(Transform), "localEulerAngles.y", yCurve);
        clip.SetCurve(rPath, typeof(Transform), "localEulerAngles.z", zCurve);
    }
    void AddAnimBase(string rPath, Transform arm, int armNo, double[,] d, ref AnimationClip clip)
    {
        AnimationCurve xCurve;
        AnimationCurve yCurve;
        AnimationCurve zCurve;

        Keyframe[] xKeys = new Keyframe[n_data];
        Keyframe[] yKeys = new Keyframe[n_data];
        Keyframe[] zKeys = new Keyframe[n_data];
        
        float keyMultiplier = 10f/n_data;
        
        for (int i=0; i<n_data; i++)
        {
            xKeys[i] = new Keyframe((float)i*keyMultiplier, arm.localEulerAngles.x);
            yKeys[i] = new Keyframe((float)i*keyMultiplier, arm.localEulerAngles.y);
            zKeys[i] = new Keyframe((float)i*keyMultiplier, -(float)d[i,armNo]*180/Mathf.PI);
        }

        xCurve = new AnimationCurve(xKeys);
        yCurve = new AnimationCurve(yKeys);
        zCurve = new AnimationCurve(zKeys);
        clip.SetCurve(rPath, typeof(Transform), "localEulerAngles.x", xCurve);
        clip.SetCurve(rPath, typeof(Transform), "localEulerAngles.y", yCurve);
        clip.SetCurve(rPath, typeof(Transform), "localEulerAngles.z", zCurve);
    }

    // public void CreateRobotAnimation(){
    //     StartCoroutine(IEnumCreateRobotAnimation());
    // }

    public void CreateRobotAnimation()
    {
        trail = tipTranform.GetComponent<TrailRenderer>();
        gsc = GameObject.Find("Graph").GetComponent<GraphScript>();

        anim = GetComponent<Animation>();
        clip = new AnimationClip();
        clip.legacy = true;
        dabClip = new AnimationClip();
        dabClip.legacy = true;

        
		////////////Get each row from csv data file
		GameObject UIController = GameObject.Find("UI Controller");
		StreamReader joint_data;
        StreamReader temp;
		try{
			joint_data = new StreamReader(UIController.GetComponent<UI_Controller>().jointDataFilePath);
			temp = new StreamReader(UIController.GetComponent<UI_Controller>().jointDataFilePath);
		}catch{
			Debug.LogWarning("RobotControllerScript.cs: Joint data file does not exist or cannot be read!");
			MessageBox.Show("Point data file does not exist or cannot be read!", "Warning!");
			return;
		}

        //Get number of lines in file
        int i = 0;
        string tempdata;
        do{
            tempdata = temp.ReadLine();
            i++;
        }while(tempdata != null);
        n_data = i;

        //Read data from joint data file
		string data;
		data = joint_data.ReadLine();
		double[,] d = new double[n_data, 7];
        i = 0;
        do{
			string[] jointData = data.Split(new char[] {','} );
			d[i,0] = double.Parse(jointData[0]);
			d[i,1] = double.Parse(jointData[1]);
			d[i,2] = double.Parse(jointData[2]);
			d[i,3] = double.Parse(jointData[3]);
			d[i,4] = double.Parse(jointData[4]);
			d[i,5] = double.Parse(jointData[5]);
			d[i,6] = double.Parse(jointData[6]);

            d0.Add(double.Parse(jointData[0]));
            d1.Add(double.Parse(jointData[1]));
            d2.Add(double.Parse(jointData[2]));
            d3.Add(double.Parse(jointData[3]));
            d4.Add(double.Parse(jointData[4]));
            d5.Add(double.Parse(jointData[5]));
            d6.Add(double.Parse(jointData[6]));

            i++;
			data = joint_data.ReadLine();
		}while(data != null);

        AddAnimBase("base/L0", L0, 0, d, ref clip);
        AddAnim("base/L0/L1", L1, 1, d, ref clip);
        AddAnim("base/L0/L1/Body/L2", L2, 2, d, ref clip);
        AddAnim("base/L0/L1/Body/L2/Body/L3", L3, 3, d, ref clip);
        AddAnim("base/L0/L1/Body/L2/Body/L3/Body/L4", L4, 4, d, ref clip);
        AddAnim("base/L0/L1/Body/L2/Body/L3/Body/L4/Body/L5", L5, 5, d, ref clip);
        AddAnim("base/L0/L1/Body/L2/Body/L3/Body/L4/Body/L5/Body/L6", L6, 6, d, ref clip);

        anim.AddClip(clip, "regular");
        anim.AddClip(dabClip, "dab");
    }

    public void ChangeGraph(int idx){
        GameObject[] pts = GameObject.FindGameObjectsWithTag("Graph Point");
        foreach (GameObject pt in pts)
        {
            Destroy(pt);
        }
        switch (idx)
        {
            case 0: gsc.ShowGraph(d0);
                    break;
            case 1: gsc.ShowGraph(d1);
                    break;
            case 2: gsc.ShowGraph(d2);
                    break;
            case 3: gsc.ShowGraph(d3);
                    break;
            case 4: gsc.ShowGraph(d4);
                    break;
            case 5: gsc.ShowGraph(d5);
                    break;
            case 6: gsc.ShowGraph(d6);
                    break;
            default:break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // RotateJoint(L1, -angle1);
        // RotateJoint(L2, angle2);
        // RotateJoint(L3, -angle3);
        // RotateJoint(L4, angle4);
        // RotateJoint(L5, -angle5);

        try{
            if(anim["regular"].normalizedTime > 0 && anim["regular"].normalizedTime < 0.99f){
                trail.emitting = true;
            }
            else{
                trail.emitting = false;
            }
        }catch{}
    }

    public void play()
    {
        try{
            anim.Play("regular");
            anim["regular"].speed = animationSpeed;
            anim["regular"].normalizedTime = 0f;
            trail.Clear();
        }catch{
            Debug.LogWarning("RobotControllerScript: Joint animation clip not found");
        }
    }

    public void animationScroll(float time)
    {
        try{
            anim.Play("regular");
            anim["regular"].speed = 0;
            anim["regular"].normalizedTime = time;
        }catch{
            Debug.LogWarning("RobotControllerScript: Joint animation clip not found");
        }
    }
}
