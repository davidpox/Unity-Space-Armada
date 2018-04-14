using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinSphere : MonoBehaviour {
    private Mesh mesh;
    private MeshRenderer renderer;
    private float radius;
    public Color GrassColor;

    // Use this for initialization
    void Start () {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("DAB/Vertex Detail Specular"));

        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].x += (Perlin.Noise(verts[i]) * .2f);
            verts[i].y += (Perlin.Noise(verts[i]) * .2f);
            verts[i].z += (Perlin.Noise(verts[i]) * .2f);
        }
        mesh.vertices = verts;
        mesh.RecalculateBounds();

        radius = renderer.bounds.extents.magnitude;
        ApplyColours();
	}

    void ApplyColours()
    {
        Color[] colours = new Color[mesh.vertices.Length];
        float[] distances = new float[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            distances[i] = Vector3.Distance(renderer.bounds.center - transform.position, mesh.vertices[i]);
            colours[i] = GrassColor;
        }
        mesh.colors = colours;
    }
}
