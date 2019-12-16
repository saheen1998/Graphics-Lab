using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using SFB;

public class UI_Controller : MonoBehaviour
{
    public CanvasScaler canvas;
    public GameObject menu;
    public List<GameObject> cameras;
    public GameObject video;
    public Slider videoSlider;
    public List<GameObject> options;
    public List<GameObject> penPrefabs;
    public GameObject inputFields;
    public List<InputField> info;
    public GameObject constraintList;
    public GameObject graph;
    public GameObject forceGraph;
    public List<TMP_InputField> CamPos;
    public Material matNormal;
    public Material matSelected;

    //Used inside scripts only
    [HideInInspector] public bool toBrowse;
    [HideInInspector] public List<double> wx;
    [HideInInspector] public List<double> wy;
    [HideInInspector] public List<double> wz;
    [HideInInspector] public List<double> dx;
    [HideInInspector] public List<double> dy;
    [HideInInspector] public List<double> dz;
    [HideInInspector] public List<double> rad;

    public double demoSafeRep1;
    public double demoConstraint;
    public double demoSafeRep2;
    public double demoEnd;
    [HideInInspector] public string pointDataFilePath;
    [HideInInspector] public string jointDataFilePath;
    [HideInInspector] public string forceDemoFilePath;
    [HideInInspector] public string forcePlayFilePath;
    [HideInInspector] public string constraintDataFilePath;
    [HideInInspector] public string videoFilePath;

    private VideoPlayer vidPlayer;
    private Dropdown dropdownConstraintList;
    [HideInInspector] public int selectedConsIdx = 0;
    private int constraintInd = 0;
    private bool canMoveVideo = false;
    
    public void ChangeCamera(int index)
    {
        foreach (GameObject cam in cameras)
        {
            cam.SetActive(false);
        }
        cameras[index].SetActive(true);
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
    
    public void ToggleVideo(){
        video.SetActive((video.activeSelf) ? (false) : (true));
    }

    public void PlayVideo(){
        try{
            if (vidPlayer.isPlaying){
                    vidPlayer.Pause();
                }
            else{
                    vidPlayer.Play();
                }
        }catch{
            Debug.LogWarning("UI_Controller.cs: Error in playing video or video file not uploaded!");
        }
    }

    public void BrowseVideo(){        
        try{
            videoFilePath = StandaloneFileBrowser.OpenFilePanel("Open video data file", "", "", false)[0];
            vidPlayer.url = videoFilePath;
        }catch{
            Debug.LogWarning("UI_Controller.cs: Error in opening video file or operation cancelled!");
            return;
        }
        vidPlayer.frame = 1;
        vidPlayer.Pause();
    }

    public void ScrollVideo(float t){
        vidPlayer.time = vidPlayer.length * t;
    }

    public void MoveVideo(){
        canMoveVideo = canMoveVideo? false : true;
    }

    ///////////////////Functions for constraint
    public void SetConstraint(int ind){
        constraintInd = ind;
    }

    public void AddConstriantInfo(double w1, double w2, double w3, double d1, double d2, double d3, double r, string cons){

        wx.Add(w1);
        wy.Add(w2);
        wz.Add(w3);
        dx.Add(d1);
        dy.Add(d2);
        dz.Add(d3);
        rad.Add(r);

        Dropdown.OptionData opt = new Dropdown.OptionData();
        opt.text = (dropdownConstraintList.options.Count + 1).ToString() + ". " + cons;
        dropdownConstraintList.options.Add(opt);
        selectedConsIdx = dropdownConstraintList.options.Count - 1;
        dropdownConstraintList.value = selectedConsIdx + 1;
    }

    public void SelectConstraint(int idx){

        info[0].text = wx[idx].ToString();
        info[1].text = wy[idx].ToString();
        info[2].text = wz[idx].ToString();
        info[3].text = dx[idx].ToString();
        info[4].text = dy[idx].ToString();
        info[5].text = dz[idx].ToString();
        info[6].text = rad[idx].ToString();
        selectedConsIdx = idx;

        //Change color to indicate selected constraint (Not Implemented)
        /*GameObject[] constraints = GameObject.FindGameObjectsWithTag("Constraint");
        foreach (GameObject c in constraints)
        {
            if(c.name == ("Constraint " + selectedConsIdx))
                c.GetComponent<MeshRenderer>().material = matSelected;
            else
                c.GetComponent<MeshRenderer>().material = matNormal;
        }*/
    }

    public void RemoveConstraint(){
        string cText = dropdownConstraintList.options[dropdownConstraintList.value].text;
        string[] cNum = cText.Split(new char[] {'.'} );
        selectedConsIdx = int.Parse(cNum[0]) - 1;
        Destroy(GameObject.Find("Pen " + selectedConsIdx));
        Destroy(GameObject.Find("Constraint " + selectedConsIdx));
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject pt in points){
            if(pt.name == "Point " + selectedConsIdx)
                Destroy(pt);
        }

        wx.RemoveAt(dropdownConstraintList.value);
        wy.RemoveAt(dropdownConstraintList.value);
        wz.RemoveAt(dropdownConstraintList.value);
        dx.RemoveAt(dropdownConstraintList.value);
        dy.RemoveAt(dropdownConstraintList.value);
        dz.RemoveAt(dropdownConstraintList.value);
        rad.RemoveAt(dropdownConstraintList.value);

        dropdownConstraintList.options.RemoveAt(dropdownConstraintList.value);
        if(dropdownConstraintList.options.Count <= 0)
            constraintList.transform.GetChild(0).GetComponent<Text>().text = "";
        else
            dropdownConstraintList.value = dropdownConstraintList.options.Count - 1;
    }

    public void ToggleConstraintInfo(){
        inputFields.SetActive((inputFields.activeSelf) ? (false) : (true));
    }

    public void AddConstraint(){

        toBrowse = false;

        selectedConsIdx = dropdownConstraintList.options.Count;
        GameObject pen = Instantiate(penPrefabs[constraintInd]);
        pen.name = "Pen " + (selectedConsIdx).ToString();
        
        GameObject cam = GameObject.Find("CamConstraint");
        if(cam != null) cam.GetComponent<CamConstraint>().ResetPos();
    }

    public void browseConstraintInfoFile(){
        try{
            constraintDataFilePath = StandaloneFileBrowser.OpenFilePanel("Open constraint information data file", "", "csv", false)[0];
            toBrowse = true;

            selectedConsIdx = dropdownConstraintList.options.Count;

            GameObject pen = Instantiate(penPrefabs[constraintInd]);
            pen.name = "Pen " + (selectedConsIdx).ToString();
            
            GameObject cam = GameObject.Find("CamConstraint");
            if(cam != null) cam.GetComponent<CamConstraint>().ResetPos();
        }catch{}
    }

    public void ShowConstraintList(){
        constraintList.SetActive((constraintList.activeSelf) ? (false) : (true));
    }
    ///////////////////////////////////

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

    public void SetCamPos(){
        cameras[2].transform.position = new Vector3((float)double.Parse(CamPos[0].text), (float)double.Parse(CamPos[2].text), (float)double.Parse(CamPos[1].text));
        cameras[2].transform.LookAt(GameObject.Find("Tip").GetComponent<Transform>().position);
    }

    private void Start() {
        vidPlayer = video.GetComponent<VideoPlayer>();
        dropdownConstraintList = constraintList.GetComponent<Dropdown>();
    }

    void LateUpdate(){
        if(canMoveVideo){
            
            Vector3 pos = Input.mousePosition;
            video.transform.position = pos;
        }

        /* Update slider to current video time (SLOW)
        if(vidPlayer.isPlaying){
            videoSlider.value = (float)(vidPlayer.time / vidPlayer.length);
        }*/
    }
}
