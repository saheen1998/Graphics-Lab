using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using SFB;

public class UI_Controller : MonoBehaviour
{
    public string pointDataFilePath;
    public string jointDataFilePath;
    public List<GameObject> cameras;
    public GameObject video;
    public List<GameObject> options;
    public List<GameObject> penPrefabs;
    public GameObject inputFields;
    public GameObject graph;

    private VideoPlayer vidTex;
    private int constraintInd = 0;
    
    public void ChangeCamera(int index)
    {
        foreach (GameObject cam in cameras)
        {
            cam.SetActive(false);
        }
        cameras[index].SetActive(true);

        if (index == 2) video.SetActive(true);
        else video.SetActive(false);
    }

    public void ToggleOptions(){
        foreach (GameObject opt in options)
        {
            opt.SetActive((opt.activeSelf) ? (false) : (true));
        }
    }

    public void ToggleGraph(){
        graph.SetActive((graph.activeSelf) ? (false) : (true));
    }

    public void PlayVideo(){
        if (vidTex.isPlaying)
            {
                vidTex.Pause();
            }
            else
            {
                vidTex.Play();
            }
    }


    ///////////////////Functions for constraint
    public void SetConstraint(int ind){
        constraintInd = ind;
    }

    public void SetConstraintInfo(){
        if (inputFields.activeSelf){

            GameObject[] pens = GameObject.FindGameObjectsWithTag("Pen");
            foreach (GameObject pen in pens){
                Destroy(pen);
            }
            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            foreach (GameObject pt in points){
                Destroy(pt);
            }
            GameObject[] constraints = GameObject.FindGameObjectsWithTag("Constraint");
            foreach (GameObject c in constraints){
                Destroy(c);
            }
            Instantiate(penPrefabs[constraintInd]);
            inputFields.SetActive(false);
            
            GameObject cam = GameObject.Find("CamConstraint");
            if(cam != null) cam.GetComponent<CamConstraint>().ResetPos();
        }else{
            inputFields.SetActive(true);
        }
    }

    public void SetPointDataFile(){
        try{
            pointDataFilePath = StandaloneFileBrowser.OpenFilePanel("Open point data file", "", "csv", false)[0];
        }catch{}
    }

    public void SetJointDataFile(){
        try{
            jointDataFilePath = StandaloneFileBrowser.OpenFilePanel("Open joint data file", "", "csv", false)[0];
        }catch{}
    }

    private void Start() {
        vidTex = video.GetComponent<VideoPlayer>();
    }
}
