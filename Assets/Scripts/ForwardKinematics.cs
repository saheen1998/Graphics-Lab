using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematics : MonoBehaviour
{
    public List<double> l = new List<double>();
    public List<double> a = new List<double>();

    private List<double> x = new List<double>(){0, 0, 0, 0, 0, 0, 0};
    private List<double> y = new List<double>(){0, 0, 0, 0, 0, 0, 0};
    private List<double> z = new List<double>(){0, 0, 0, 0, 0, 0, 0};
    private double xf;
    private double yf;
    private double zf;

    public Vector3 GetPoint(List<float> ang){
        Vector3 pos;

        double diag;
        float diagAng = Mathf.Atan2((float)l[1], (float)l[0]);
        ang[0] = ang[0];// + diagAng;
        diag = l[1] / Mathf.Sin(diagAng);

        x[0] = diag * Mathf.Cos(ang[0] + diagAng);
        y[0] = 0.325f;
        z[0] = diag * Mathf.Sin(ang[0] + diagAng);

        x[1] = x[0] + l[2] * Mathf.Cos(ang[1]);
        y[1] = y[0] + l[2] * Mathf.Sin(ang[1]);
        z[1] = z[0] + l[2] * Mathf.Sin(ang[0]);

        pos = new Vector3((float)x[1], (float)y[1], (float)z[1]);
        return pos;
    }
}
