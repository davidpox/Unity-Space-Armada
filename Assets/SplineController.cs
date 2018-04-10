using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineController : MonoBehaviour {

    //public Transform[] path;

    //private GameObject spaceCraft;

	// Use this for initialization
	void Start () {
 //       spaceCraft = GameObject.FindGameObjectWithTag("Player");

 //       iTween.MoveTo(spaceCraft, iTween.Hash("path", path, "time", 7, "orienttopath", true, "looktime", .6, "eastype", "easeInOutSine"));//, "oncomplete", "complete"));
	}

    void complete()
    {
        print("Spaceship finished navigating");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
