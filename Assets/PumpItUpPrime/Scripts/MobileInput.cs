using UnityEngine;
using System.Collections;

public class MobileInput : MonoBehaviour {

    public StepchartMover stepchart;

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        for (var i = 0; i < Input.touchCount; i++) {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

            if (Physics.Raycast(ray, out hit, 10)) {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                    stepchart.BeatInput(2, int.Parse(hit.transform.name));
                else
                    stepchart.BeatInput(1, int.Parse(hit.transform.name));
            }
        }

        /*Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10))
            Debug.Log(hit.transform);*/
    }

    #region Obsolete
    public void Tap(int key) {
        stepchart.BeatInput(2, key);
    }
    #endregion
}
