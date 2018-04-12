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
            verts[i].x += (Perlin.Noise(verts[i]) * .2f);
            verts[i].y += (Perlin.Noise(verts[i]) * .2f);
            verts[i].z += (Perlin.Noise(verts[i]) * .2f);
        }
        mSphere.vertices = verts;
        mSphere.RecalculateBounds();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
