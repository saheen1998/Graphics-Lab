using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ForceGraphScript : MonoBehaviour
{
    public Text textForce;
    public Sprite ptSprite;
    public Sprite lineSprite;
    private RectTransform graphContainer;

    private float height;
    private float width;
    private float ymax = 6.28f;
    private RectTransform lineRect;

    private List<double> forces;
    private List<double> forcesD = new List<double>();
    private List<double> forcesP = new List<double>();
    private int count;

    private void Awake() {
        graphContainer = transform.Find("Force Graph Container").GetComponent<RectTransform>();
        height = graphContainer.sizeDelta.y;
        width = graphContainer.sizeDelta.x;

        //Plot the current state time line
        GameObject line = new GameObject("Force State line", typeof(Image));
        line.transform.SetParent(graphContainer, false);
        line.GetComponent<Image>().sprite = lineSprite;
        lineRect = line.GetComponent<RectTransform>();
        lineRect.anchoredPosition = new Vector2(0, 0);
        lineRect.sizeDelta = new Vector2(1, height);
        lineRect.anchorMin = new Vector2(0, 0.5f);
        lineRect.anchorMax = new Vector2(0, 0.5f);
    }

    void PlotPoint(Vector2 pos){
        GameObject pt = new GameObject("ForceGraphPt", typeof(Image));
        pt.transform.SetParent(graphContainer, false);
        pt.gameObject.tag = "Force Graph Point";
        pt.GetComponent<Image>().sprite = ptSprite;
        RectTransform ptRect= pt.GetComponent<RectTransform>();
        ptRect.anchoredPosition = pos;
        ptRect.sizeDelta = new Vector2(1, 1);
        ptRect.anchorMin = new Vector2(0, 0.5f);
        ptRect.anchorMax = new Vector2(0, 0.5f);
    }

    public void ShowGraph(List<double> val){
        forces = new List<double>(val);
        count = val.Count;
        ymax = 2.1f * Math.Max( Math.Abs((float)val.Max()), Math.Abs((float)val.Min()) );
        for (int i = 0; i < count; i++)
        {
            float xPos =  ((float)(i) / (count-1)) * width;
            float yPos = (float)val[i] / ymax * height;
            PlotPoint(new Vector2(xPos, yPos));
        }
    }

    public void UpdateCurrentState(float val){
        lineRect.anchoredPosition = new Vector2(val * width, 0);
        int idx = (int)Math.Floor(val * (count - 1));
        try{
            textForce.text = ((float)forces[idx]).ToString("F6") + " Units";
        }
        catch{}
    }

    public void ReadDemoFile(){
        
		////////////Get each row from csv data file
		GameObject UIController = GameObject.Find("UI Controller");
		StreamReader forceD_data;
		try{
			forceD_data = new StreamReader(UIController.GetComponent<UI_Controller>().forceDemoFilePath);
		}catch{
			Debug.LogWarning(gameObject.name + " - ForceGraphScript.cs: Demo force data file does not exist or cannot be read!");    //ERROR
			MessageBox.Show("Demo force data file does not exist or cannot be read!", "Error!");
			return;
		}

        //Read demo data from joint data file
		string data;
		data = forceD_data.ReadLine();
        int i = 0;
        do{
			string[] FData = data.Split(new char[] {' '} );

            forcesD.Add(double.Parse(FData[0]));

            i++;
			data = forceD_data.ReadLine();
		}while(data != null);
    }

    public void ReadPlayFile(){
        
		////////////Get each row from csv data file
		GameObject UIController = GameObject.Find("UI Controller");
		////////////Get each row from csv data file
		StreamReader forceP_data;
		try{
			forceP_data = new StreamReader(UIController.GetComponent<UI_Controller>().forcePlayFilePath);
		}catch{
			Debug.LogWarning(gameObject.name + " - ForceGraphScript.cs: Playback force data file does not exist or cannot be read!");    //ERROR
			MessageBox.Show("Playback force data file does not exist or cannot be read!", "Error!");
			return;
		}

        //Read playback data from joint data file
		string data;
		data = forceP_data.ReadLine();
        int i = 0;
        do{
			string[] PData = data.Split(new char[] {' '} );

            forcesP.Add(double.Parse(PData[0]));

            i++;
			data = forceP_data.ReadLine();
		}while(data != null);
    }

    public void ChooseGraph(int idx){
        GameObject[] pts = GameObject.FindGameObjectsWithTag("Force Graph Point");
        foreach (GameObject pt in pts)
        {
            Destroy(pt);
        }
        try{
            switch (idx)
            {
                case 0: ShowGraph(forcesD);
                        break;
                case 1: ShowGraph(forcesP);
                        break;
                default: break;
            }
        }catch{
            Debug.LogWarning(gameObject.name + " - ForceGraphScript.cs: Cannot display force graph or both force data files not loaded!");
        }
    }
}