using UnityEngine;
using System.Collections;

public class GamepadCalibrator : MonoBehaviour {

    // Use this for initialization
    public Renderer[] calibrationTest;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Debug.Log(Input.GetAxis("Horizontal"));
        for (var i = 0; i < 5; i++) {
            if (i != 2) {
                if (Input.GetButton(i.ToString()))
                    calibrationTest[i].material.color = Color.white;
                else
                    calibrationTest[i].material.color = Color.black;
            }
        }
    }
}
