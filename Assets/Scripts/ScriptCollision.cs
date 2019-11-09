using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCollision : MonoBehaviour
{

    public GameObject text;
    private void OnTriggerStay(Collider other) {
        text.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {
        text.SetActive(false);
    }
}
