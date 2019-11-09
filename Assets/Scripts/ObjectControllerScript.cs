using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectControllerScript : MonoBehaviour
{
    public Transform table;
    public Slider slider;

    public void SetPositions(){
        //table.position = tablePos;
    }

    public void ToggleTable(bool stat){
        table.gameObject.SetActive(stat);
    }

    public void SetTableHeight(float h) {
        table.position = new Vector3(table.position.x, h - .04f, table.position.z);
        slider.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = (table.position.y + 0.04).ToString("F4");
    }
}
