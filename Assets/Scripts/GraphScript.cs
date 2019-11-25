using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GraphScript : MonoBehaviour
{
    public Text textAngle;
    public Sprite ptSprite;
    public Sprite lineSprite;
    private RectTransform graphContainer;

    private float height;
    private float width;
    private float ymax = 6.28f;
    private RectTransform lineRect;

    private List<double> angles;
    private int count;
    private bool radians = true;

    private void Awake() {
        graphContainer = transform.Find("Graph Container").GetComponent<RectTransform>();
        height = graphContainer.sizeDelta.y;
        width = graphContainer.sizeDelta.x;

        //Plot the current state time line
        GameObject line = new GameObject("State line", typeof(Image));
        line.transform.SetParent(graphContainer, false);
        line.GetComponent<Image>().sprite = lineSprite;
        lineRect = line.GetComponent<RectTransform>();
        lineRect.anchoredPosition = new Vector2(0, 0);
        lineRect.sizeDelta = new Vector2(1, height);
        lineRect.anchorMin = new Vector2(0, 0.5f);
        lineRect.anchorMax = new Vector2(0, 0.5f);
    }

    void PlotPoint(Vector2 pos){
        GameObject pt = new GameObject("GraphPt", typeof(Image));
        pt.transform.SetParent(graphContainer, false);
        pt.gameObject.tag = "Graph Point";
        pt.GetComponent<Image>().sprite = ptSprite;
        RectTransform ptRect= pt.GetComponent<RectTransform>();
        ptRect.anchoredPosition = pos;
        ptRect.sizeDelta = new Vector2(1, 1);
        ptRect.anchorMin = new Vector2(0, 0.5f);
        ptRect.anchorMax = new Vector2(0, 0.5f);
    }

    public void ShowGraph(List<double> val){
        angles = new List<double>(val);
        count = val.Count;
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
            if (radians)
                textAngle.text = ((float)angles[idx]).ToString("F6") + " radians";
            else
                textAngle.text = ((float)angles[idx] * 180 / Math.PI).ToString("F4") + " degrees";
        }
        catch{}
    }

    public void ChangeAngleUnit(){
        radians = radians ? false : true;
        UpdateCurrentState(0);
    }
}