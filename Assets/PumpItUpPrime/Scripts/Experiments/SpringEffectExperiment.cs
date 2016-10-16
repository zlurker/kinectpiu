using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
public class SpringEffectExperiment : MonoBehaviour {

    public float iniLength;
    public SpriteRenderer sprite;
    bool test;

	// Use this for initialization
	void Start () {
        StartCoroutine(Test());
	}
	
	// Update is called once per frame
	void Update () {
        //while ()
        if (test) 
        transform.localScale = new Vector2(1, Mathf.Lerp(transform.localScale.y, 2, 0.01f *1.5f));

        float scaleValue = 0;        
        scaleValue = iniLength/sprite.bounds.extents.y;

         foreach (Transform child in transform) {
            if (child != sprite.transform && child.tag != "LongBeat")
            child.localScale = new Vector2(2, 2 * scaleValue);
        }
	}

    IEnumerator Test() {
        yield return new WaitForSeconds(2f);
        transform.localScale = new Vector2(1, 0.001f);
        test = true;
    }
}
