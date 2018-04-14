using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStarGenerator : MonoBehaviour {
    private Mesh mesh;
    private MeshRenderer renderer;
    private GameObject AstroidParent;
    private float radius;
    public Color GrassColor;
    public Material AstroidMaterial;
    float yaw, pitch, roll;

    // Use this for initialization
    void Start () {

        yaw = Random.Range(-20.0f, 20.0f);
        pitch = Random.Range(-20.0f, 20.0f);
        roll = Random.Range(-20.0f, 20.0f);

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

        spawnAstroidBelt(500, 100, 140, Vector3.zero);
    }

    void Update()
    {
        gameObject.transform.Rotate(AstroidParent.transform.up, 4.0f * Time.deltaTime);
    }


    /* ================================= */
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

    void spawnAstroidBelt(int numStars, float distanceMin, float distanceMax, Vector3 pos)
    {
        AstroidParent = new GameObject("AstroidParent");
       // AstroidParent.transform.parent = gameObject.transform;
        GameObject astroid = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        astroid.name = "Astroid";
        astroid.transform.parent = AstroidParent.transform;
        Mesh mesh = astroid.GetComponent<MeshFilter>().mesh;

        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].x += (Perlin.Noise(verts[i].normalized) * 0.3f);
            verts[i].y += (Perlin.Noise(verts[i].normalized) * 0.3f);
            verts[i].z += (Perlin.Noise(verts[i].normalized) * 0.3f);
        }
        mesh.vertices = verts;
        mesh.RecalculateBounds();

        for (int i = 0; i < numStars; i++)
        {
            float angle = Random.Range(0.0f, 360.0f);
            float distance = Random.Range(distanceMin, distanceMax);
            float x = (Mathf.Cos(angle * Mathf.Deg2Rad) * distance) * Random.Range(1.0f, 1.6f);
            float z = (Mathf.Sin(angle * Mathf.Deg2Rad) * distance) * Random.Range(0.9f, 1.2f);
            float y = Random.Range(-10.0f, 10.0f);
            float scale = Random.Range(0.8f, 3.0f);

            GameObject iAstroid = Instantiate(astroid, pos + new Vector3(x, y, z), new Quaternion());
            iAstroid.transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            iAstroid.transform.localScale = new Vector3(scale, scale, scale);
            iAstroid.transform.parent = AstroidParent.transform;
            MeshRenderer renderer = iAstroid.GetComponent<MeshRenderer>();
            renderer.material = AstroidMaterial;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        AstroidParent.transform.Rotate(new Vector3(yaw, pitch, roll));

    }
}
