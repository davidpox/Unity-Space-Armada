using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Operator;
using LibNoise.Generator;
using System;

public class GenerateGalaxy : MonoBehaviour {

    public GameObject star;
    public GameObject sun;
    public GameObject cam;
    public int numStars;
    public int revolutions;

    private GameObject player;
    private GameObject enemyGalaxy;
    private GameObject friendlyGalaxy;

	// Use this for initialization
	void Start () {
        friendlyGalaxy = spawnStarsSpiral("Galaxy friendly");
        friendlyGalaxy.transform.position = new Vector3(100.0f, 0.0f, 100.0f);
        enemyGalaxy = spawnStarsSpiral("Galaxy enemy");
        enemyGalaxy.transform.position = new Vector3(-100.0f, 0.0f, -100.0f);


        GenerateBossStar();
    }

    // Update is called once per frame
    void Update () {
        //cam.transform.RotateAround(player.transform.position, Vector3.up, 10.0f * Time.deltaTime);
	}



    /* TESTING */

    public int mapSizeX = 100;
    public int mapSizeY = 100;
    private float south = -90.0f;
    private float north = 90.0f;
    private float west = -180.0f;
    private float east = 180.0f;

    private Renderer sphereRenderer;
    private Texture2D texture;


    void GenerateBossStar()
    {
        GameObject sphere = GameObject.Find("testSphere");

        sphereRenderer = sphere.GetComponent<Renderer>();

        Perlin mySphere = new Perlin();

        ModuleBase myModule;
        myModule = mySphere;

        Noise2D heightMap;
        heightMap = new Noise2D(mapSizeX, mapSizeY, myModule);
        heightMap.GenerateSpherical(south, north, west, east);
        texture = heightMap.GetTexture(GradientPresets.Grayscale);
       // sphereRenderer.material.mainTexture = texture;

        //// part 2
        //float lat, longi, height;
        //Mesh mSphere = sphere.GetComponent<MeshFilter>().mesh;
        //Vector3[] verts = mSphere.vertices;
        //for (int i = 0; i < verts.Length; i++)
        //{
        //    lat = Vector3.Angle(Vector3.up, verts[i]);
        //    longi = Vector3.SignedAngle(Vector3.right, verts[i], Vector3.up);
        //    height = texture.GetPixelBilinear(lat / 180, longi / 360).r;
        //    verts[i] *= (1 + 0.5f * height);
        //}
        //mSphere.vertices = verts;
        //mSphere.RecalculateBounds();

    }

    void GenerateSpaceShipPath()
    {

    }

    GameObject spawnStarsSpiral(string GalaxyName)
    {
        GameObject StarParent = new GameObject(GalaxyName);
        GameObject Sun = Instantiate(sun);
        Sun.transform.parent = StarParent.transform;
        Sun.transform.position = Vector3.zero;
        
        float A = 80.0f;       // galaxy size                      def: 40.0f
        float B = 500.12f;       // Buldge-to-arm  (arm sweep)       def: 11.12f
        float N = 0.806f;       // "Tightness" lower = less tight   def: 0.706f

        GameObject starInstance;
        Vector3 starScale;

        for (int i = 0; i < 360 * revolutions; i++)
        {
            float angleR = i * Mathf.Deg2Rad;
            float angleOffset = UnityEngine.Random.Range(-20.0f, 20.0f) * Mathf.Deg2Rad;
            float distance = A / Mathf.Log10(B * Mathf.Tan(angleR / (2 * N)));
            float x = Mathf.Cos(angleR + angleOffset) * distance;
            float z = Mathf.Sin(angleR + angleOffset) * distance;

            if (distance < Mathf.Abs(A))
            {
                starInstance = Instantiate(star, transform.position + new Vector3(x, 0, z), star.transform.rotation);
                starInstance.transform.parent = StarParent.transform;
                float scale = UnityEngine.Random.Range(0.5f, 1.0f);
                starScale.x = scale;
                starScale.y = scale;
                starScale.z = scale;
                starInstance.transform.localScale = starScale;
            }
        }

        return StarParent;
    }
}
