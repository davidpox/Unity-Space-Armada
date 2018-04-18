using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGalaxy : MonoBehaviour {

    public GameObject star;
    public GameObject sun;
    public int revolutions;

    private GameObject player;
    private GameObject enemyGalaxy;
    private GameObject enemyStarfield;
    private GameObject friendlyGalaxy;
    private GameObject friendlyStarfield;

    // Use this for initialization
    void Start () {
        friendlyGalaxy = spawnStarsSpiral("Galaxy friendly");
        friendlyGalaxy.transform.position = new Vector3(400.0f, 0.0f, 400.0f);
        friendlyStarfield = friendlyGalaxy.transform.GetChild(0).gameObject;
        friendlyGalaxy.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        friendlyGalaxy.transform.rotation = Quaternion.Euler(10, 45, 0);

        Light friendlyLight = friendlyGalaxy.AddComponent<Light>();
        friendlyLight.type = LightType.Directional;
        friendlyLight.intensity = 0.65f;
        friendlyLight.transform.rotation = new Quaternion(0.0f, 0.940f, 0.0f, -0.342f);

        enemyGalaxy = spawnStarsSpiral("Galaxy enemy");
        enemyGalaxy.transform.position = new Vector3(-300.0f, 0.0f, -300.0f);
        enemyStarfield = enemyGalaxy.transform.GetChild(0).gameObject;
        enemyGalaxy.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        enemyGalaxy.transform.rotation = Quaternion.Euler(10, 45, 0);

        Light enemyLight = enemyGalaxy.AddComponent<Light>();
        enemyLight.type = LightType.Directional;
        enemyLight.intensity = 0.65f;
        enemyLight.transform.rotation = new Quaternion(0.0f, 0.383f, 0.0f, 0.924f);  
    }

    GameObject spawnStarsSpiral(string GalaxyName)
    {
        GameObject GalaxyParent = new GameObject(GalaxyName);
        GameObject StarParent = new GameObject("StarParent");
        StarParent.transform.parent = GalaxyParent.transform;

        GameObject Sun = Instantiate(sun);
        Sun.transform.parent = GalaxyParent.transform;
        
        
        float A = 80.0f;       // galaxy size                      def: 40.0f                     80.0f; 
        float B = 500.12f;    // Buldge-to-arm  (arm sweep)       def: 11.12f                  500.12f
        float N = 0.806f;       // "Tightness" lower = less tight   def: 0.706f                   0.806f;

        GameObject starInstance;
        Vector3 starScale;

        for (int i = 0; i < 360 * revolutions; i++)
        {
            float angleR = i * Mathf.Deg2Rad;
            float angleOffset = Random.Range(-20.0f, 20.0f) * Mathf.Deg2Rad;
            float distance = A / Mathf.Log10(B * Mathf.Tan(angleR / (2 * N)));
            float x = Mathf.Cos(angleR + angleOffset) * distance;
            float z = Mathf.Sin(angleR + angleOffset) * distance;

            if (distance < Mathf.Abs(A))
            {
                starInstance = Instantiate(star, transform.position + new Vector3(x, 0, z), star.transform.rotation);
                starInstance.transform.parent = StarParent.transform;
                float scale = Random.Range(0.5f, 1.0f);
                starScale.x = scale;
                starScale.y = scale;
                starScale.z = scale;
                starInstance.transform.localScale = starScale;
            }
        }

        return GalaxyParent;
    }

    void Update()
    {
        enemyStarfield.transform.Rotate(transform.up, 1.0f * Time.deltaTime);
        friendlyStarfield.transform.Rotate(transform.up, 1.0f * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            Time.timeScale -= 0.1f;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Time.timeScale += 0.1f;
        }
    }

}
