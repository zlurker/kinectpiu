using UnityEngine;
using System.Collections;

public class CollisionTest : MonoBehaviour {

    public int beatTypeToCheck;
    public StepchartMover stepchartMover;
	// Use this for initialization
	
    void OnCollisionStay2D(Collision2D coll) {
        stepchartMover.BeatInput(2,beatTypeToCheck);
    }
}
