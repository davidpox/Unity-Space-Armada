using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinSphere : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Mesh mSphere = gameObject.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mSphere.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] += Mathf.PerlinNoise(
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
