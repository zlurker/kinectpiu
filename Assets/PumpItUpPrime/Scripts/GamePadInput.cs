using UnityEngine;
using System.Collections;

public class GamePadInput : MonoBehaviour {

    public StepchartMover stepchart;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        for (var i = 0; i < 5; i++) {
            if (i != 2) {
                if (Input.GetButtonDown(i.ToString()))
                    stepchart.BeatInput(2, i);

                if (Input.GetButton(i.ToString()))
                    stepchart.BeatInput(1, i);
            }
        }
    }
}
