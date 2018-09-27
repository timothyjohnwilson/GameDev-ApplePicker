using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingBackground : MonoBehaviour {

    public Material material;
    private float startval;
    void Start()
    {
        startval = 0f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
            
            Color color = new Color(startval, .5f, .5f);
            gameObject.GetComponent<Renderer>().material.color = color;

        startval += 0.001f % 1f;
        

    }
}
