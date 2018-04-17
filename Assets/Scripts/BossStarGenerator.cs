using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LibNoise.Unity;
using LibNoise.Unity.Operator;
using LibNoise.Unity.Generator;

public class BossStarGenerator : MonoBehaviour {
    public Color GrassColor;
    public Material AstroidMaterial;
    public ClimateType[] climates;
    public int treesCount;
    public int rockCount;
    public int flowerCount;


    private ClimateType selectedClimate;
    private Mesh mesh;
    private new MeshRenderer renderer;
    private GameObject astroid;
    private GameObject AstroidParent;
    private bool shouldSpawnAstroids;
    private float yaw, pitch, roll;
    private Perlin perlin;
    private int astroidCount;

    [System.Serializable]
    public struct ClimateType
    {
        public string name;
        public Material WaterColor;
        public Color TerrainColor;
        public GameObject[] Trees;
        public GameObject[] Rocks;
        public GameObject[] Flowers;
        public bool GenerateWater;
        public float maxLacunarity;
        [Range(0.1f, 1.0f)]
        public float maxPersistance;
        public int maxOctaves;
        public float maxFrequency;
    }

    // Use this for initialization
    void Start () {

        int c = Mathf.FloorToInt(Random.Range(0, 4));                                                                               // Select a random climate.
        selectedClimate = climates[c];

        yaw = Random.Range(-20.0f, 20.0f);                                                                                          // Random angle for astroid belt
        pitch = Random.Range(-20.0f, 20.0f);
        roll = Random.Range(-20.0f, 20.0f);

        astroidCount = Random.Range(100, 500);

        if (Random.Range(0.0f, 1.0f) > 0.5f) { spawnAstroidBelt(astroidCount, 100, 140, gameObject.transform.position); shouldSpawnAstroids = true; }                    // Should we spawn an astroid belt? 50% chance. 

        GenerateMeshL();                                                                                                             // Apply perlin noise to the sphere. 

        ApplyColours();                                                                                                             // Give the sphere colours depending on climate. 
                
        GenerateFoliage();                                                                                                          // Generate extras (trees, rocks, flowers, etc.) 

        ApplyUI();
    }
    // Libnoise Perlin
    void GenerateMeshL()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("DAB/Vertex Detail Specular"));

        Vector3[] verts = mesh.vertices;

        Vector3 _displacement = Random.Range(-10, 10) * Vector3.one;

        perlin = new Perlin();

        perlin.Seed = (int)System.DateTime.Now.Ticks;

        perlin.Lacunarity = selectedClimate.maxLacunarity;
        perlin.Persistence = selectedClimate.maxPersistance;
        perlin.OctaveCount = selectedClimate.maxOctaves;
        perlin.Frequency = selectedClimate.maxFrequency;
        
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].x += (float)perlin.GetValue(verts[i] + _displacement) * 0.2f;        //(Perlin.Fbm(verts[i], 2) * 0.6f);
            verts[i].y += (float)perlin.GetValue(verts[i] + _displacement) * 0.2f;        //(Perlin.Fbm(verts[i], 2) * 0.6f);
            verts[i].z += (float)perlin.GetValue(verts[i] + _displacement) * 0.2f;        //(Perlin.Fbm(verts[i], 2) * 0.6f);
        }
        mesh.vertices = verts;
        mesh.RecalculateBounds();

        gameObject.AddComponent<MeshCollider>();
        
    }
    //Keijiro Perlin
    void GenerateMeshK()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("DAB/Vertex Detail Specular"));

        Vector3[] verts = mesh.vertices;

        int octave = Mathf.FloorToInt(Random.Range(1, 20));
        print(octave);


        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].x += (KeijiroPerlin.Fbm(verts[i], octave) * 0.6f);        //(Perlin.Fbm(verts[i], 2) * 0.6f);
            verts[i].y += (KeijiroPerlin.Fbm(verts[i], octave) * 0.6f);        //(Perlin.Fbm(verts[i], 2) * 0.6f);
            verts[i].z += (KeijiroPerlin.Fbm(verts[i], octave) * 0.6f);        //(Perlin.Fbm(verts[i], 2) * 0.6f);
        }
        mesh.vertices = verts;
        mesh.RecalculateBounds();

        gameObject.AddComponent<MeshCollider>();
    }

    void Update()
    {
        if (shouldSpawnAstroids)
        {
            gameObject.transform.Rotate(AstroidParent.transform.up, 4.0f * Time.deltaTime);
        } else
        {
            gameObject.transform.Rotate(Vector3.up, 4.0f * Time.deltaTime);
        }
    }


    /* ================================= */
    void GenerateFoliage()
    {
        if(selectedClimate.GenerateWater)                                                                                           // Change water color depending on climate
        {
            GameObject waterSphere = gameObject.transform.GetChild(0).gameObject;
            MeshRenderer mr = waterSphere.GetComponent<MeshRenderer>();
            mr.material = selectedClimate.WaterColor;
        } else if (!selectedClimate.GenerateWater)                                                                                  // Or remove it if requested by climate
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }

        if (selectedClimate.name == "Hell")
        {
            gameObject.transform.GetChild(0).localScale = new Vector3(1.03f, 1.03f, 1.03f);
        }

        // Generate Trees
        if (selectedClimate.Trees.Length > 0)
        {
            for (int i = 0; i < treesCount; i++)
            {
                int treeIdx = Mathf.FloorToInt(Random.Range(0, selectedClimate.Trees.Length - 1));                                  // A random tree from the climate 
                Vector3 randomSpherePoint = Random.onUnitSphere * 70;                                                               // Random Point on the sphere

                RaycastHit hit;
                if (Physics.Raycast(randomSpherePoint, -randomSpherePoint, out hit))
                {
                    if (hit.transform.name == "PlanetLand")                                                                         // Make sure we dont spawn in water!
                    {
                        GameObject fol = Instantiate(selectedClimate.Trees[treeIdx], hit.point, Quaternion.identity);
                        fol.transform.parent = gameObject.transform;

                        Vector3 forward = fol.transform.forward - (Vector3.Dot(fol.transform.forward, hit.normal)) * hit.normal;    // calculate how to position the foliage item to align with the hit normal
                        fol.transform.rotation = Quaternion.LookRotation(forward, hit.normal);
                    }
                }
            }
        }

        // Generate Rocks
        if (selectedClimate.Rocks.Length > 0)
        {
            for (int i = 0; i < rockCount; i++)
            {
                int RockIdx = Mathf.FloorToInt(Random.Range(0, selectedClimate.Rocks.Length - 1));                                  // A random tree from the climate 
                Vector3 randomSpherePoint = Random.onUnitSphere * 70;                                                               // Random Point on the sphere

                RaycastHit hit;
                if (Physics.Raycast(randomSpherePoint, -randomSpherePoint, out hit))
                {
                    if (hit.transform.name == "PlanetLand")                                                                         // Make sure we dont spawn in water!
                    {
                        GameObject fol = Instantiate(selectedClimate.Rocks[RockIdx], hit.point, Quaternion.identity);
                        fol.transform.parent = gameObject.transform;

                        Vector3 forward = fol.transform.forward - (Vector3.Dot(fol.transform.forward, hit.normal)) * hit.normal;    // calculate how to position the foliage item to align with the hit normal
                        fol.transform.rotation = Quaternion.LookRotation(forward, hit.normal);
                    }
                }
            }
        }

        // Generate Flowers
        if (selectedClimate.Flowers.Length > 0)
        {
            for (int i = 0; i < flowerCount; i++)
            {
                int FlowerIdx = Mathf.FloorToInt(Random.Range(0, selectedClimate.Flowers.Length - 1));                              // A random tree from the climate 
                Vector3 randomSpherePoint = Random.onUnitSphere * 70;                                                               // Random Point on the sphere

                RaycastHit hit;
                if (Physics.Raycast(randomSpherePoint, -randomSpherePoint, out hit))
                {
                    if (hit.transform.name == "PlanetLand")                                                                         // Make sure we dont spawn in water!
                    {
                        GameObject fol = Instantiate(selectedClimate.Flowers[FlowerIdx], hit.point, Quaternion.identity);
                        fol.transform.parent = gameObject.transform;

                        Vector3 forward = fol.transform.forward - (Vector3.Dot(fol.transform.forward, hit.normal)) * hit.normal;    // calculate how to position the foliage item to align with the hit normal
                        fol.transform.rotation = Quaternion.LookRotation(forward, hit.normal);
                    }
                }
            }
        }
    }

    void ApplyColours()
    {
        Color[] colours = new Color[mesh.vertices.Length];
        float[] distances = new float[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            distances[i] = Vector3.Distance(renderer.bounds.center - transform.position, mesh.vertices[i]);
            colours[i] = selectedClimate.TerrainColor;
        }
        mesh.colors = colours;
    }

    void spawnAstroidBelt(int numStars, float distanceMin, float distanceMax, Vector3 pos)
    {
        AstroidParent = new GameObject("AstroidParent");
        ReMakeAstroid();

        for (int i = 0; i < numStars; i++)
        {
            ReMakeAstroid();
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

    void ReMakeAstroid()
    {
        astroid = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        astroid.name = "Astroid";
        astroid.transform.parent = AstroidParent.transform;
        Mesh mesh = astroid.GetComponent<MeshFilter>().mesh;

        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].x += (KeijiroPerlin.Noise(verts[i].normalized) * 0.3f);
            verts[i].y += (KeijiroPerlin.Noise(verts[i].normalized) * 0.3f);
            verts[i].z += (KeijiroPerlin.Noise(verts[i].normalized) * 0.3f);
        }
        mesh.vertices = verts;
        mesh.RecalculateBounds();
    }

    public GameObject canvas;
    public Text _txtClimate;
    public Text _txtLacunarity;
    public Text _txtPersistence;
    public Text _txtOctaves;
    public Text _txtFrequency;
    public Text _txtAstroids;

    void ApplyUI()
    {
        _txtClimate.text = "• Climate: " + selectedClimate.name;
        _txtLacunarity.text  = "• Lacunarity: " + perlin.Lacunarity.ToString();
        _txtPersistence.text  = "• Persistence: " + perlin.Persistence.ToString();
        _txtOctaves.text  = "• Octaves: " + perlin.OctaveCount.ToString();
        _txtFrequency.text  = "• Frequency: " + perlin.Frequency.ToString();

        if(shouldSpawnAstroids) { _txtAstroids.text = "• Astroids: Yes, " + astroidCount.ToString(); }
    }

    public Toggle _ToggleUI;

    public void ToggleUI()
    {
        canvas.SetActive(!canvas.activeInHierarchy);
    }
}
