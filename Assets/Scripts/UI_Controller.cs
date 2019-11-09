using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UI_Controller : MonoBehaviour
{
    public List<GameObject> cameras;
    public GameObject video;
    public List<GameObject> options;

    private VideoPlayer vidTex;
    
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

    public void SetConstraintInfo(){
        
    }

    private void Start() {
        vidTex = video.GetComponent<VideoPlayer>();
    }

    /*private void Update() {
        if (video.activeSelf){
            Debug.Log(vidTex.frame);
        }
    }*/
}
