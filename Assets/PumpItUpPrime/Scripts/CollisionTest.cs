using UnityEngine;
using System.Collections;

public class CollisionTest : MonoBehaviour {

    public int beatTypeToCheck;
    public StepchartMover stepchartMover;
	// Use this for initialization
	
    void OnCollisionStay2D(Collision2D coll) {
        stepchartMover.CheckNormalBeats(beatTypeToCheck);
    }

    void OnCollisionExit2D(Collision2D coll) {
        stepchartMover.holdingDown[beatTypeToCheck] = 0;
    }
}
