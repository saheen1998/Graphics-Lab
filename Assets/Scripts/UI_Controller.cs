using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using SFB;

public class UI_Controller : MonoBehaviour
{
    public GameObject menu;
    public string pointDataFilePath;
    public string jointDataFilePath;
    public string forceDemoFilePath;
    public string forcePlayFilePath;
    public string constraintDataFilePath;
    public List<GameObject> cameras;
    public GameObject video;
    public List<GameObject> options;
    public List<GameObject> penPrefabs;
    public GameObject inputFields;
    public List<Text> info;
    public bool toBrowse;
    public GameObject graph;
    public GameObject forceGraph;

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
        forceGraph.SetActive(false);
    }

    public void ToggleForceGraph(){
        graph.SetActive(false);
        forceGraph.SetActive((forceGraph.activeSelf) ? (false) : (true));
    }
    
    public void ToggleMenu(){
        menu.SetActive((menu.activeSelf) ? (false) : (true));
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

    public void BrowseVideo(){
        vidTex.time = 0.5f * vidTex.length;
    }


    ///////////////////Functions for constraint
    public void SetConstraint(int ind){
        constraintInd = ind;
    }

    public void SetConstraintInfo(){
        if (inputFields.activeSelf){

            toBrowse = false;

            //Destroy previous constraint game objects
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

    public void browseConstraintInfoFile(){
        try{
            constraintDataFilePath = StandaloneFileBrowser.OpenFilePanel("Open constraint information data file", "", "csv", false)[0];
            toBrowse = true;

            //Destroy previous constraint game objects
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
            
            GameObject cam = GameObject.Find("CamConstraint");
            if(cam != null) cam.GetComponent<CamConstraint>().ResetPos();
        }catch{}
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
    
    public void SetForceDFile(){
        try{
            forceDemoFilePath = StandaloneFileBrowser.OpenFilePanel("Open demonstration tong force data file", "", "csv", false)[0];
        }catch{}
    }
    
    public void SetForcePFile(){
        try{
            forcePlayFilePath = StandaloneFileBrowser.OpenFilePanel("Open playback gripper force data file", "", "csv", false)[0];
        }catch{}
    }

    private void Start() {
        vidTex = video.GetComponent<VideoPlayer>();
    }
}
