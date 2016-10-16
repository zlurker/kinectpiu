using UnityEngine;
using System.Collections;

public class SpriteExperiment : MonoBehaviour {

    public Sprite test;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(test.bounds.extents.y);
	}
}
