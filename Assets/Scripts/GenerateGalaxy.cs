using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGalaxy : MonoBehaviour {

    public GameObject star;
    public GameObject cam;
    public int numStars;
    public int revolutions;

    private GameObject player;


	// Use this for initialization
	void Start () {
        spawnStarsSpiral();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        //cam.transform.RotateAround(player.transform.position, Vector3.up, 10.0f * Time.deltaTime);
	}
  
    void spawnStarsSpiral()
    {
        GameObject StarParent = new GameObject("StarParent");
        float A = 40.0f;       // galaxy size                      def: 40.0f
        float B = 0.12f;       // Buldge-to-arm  (arm sweep)       def: 11.12f
        float N = 0.806f;       // "Tightness" lower = less tight   def: 0.706f

        GameObject starInstance;
        Vector3 starScale;

        for (int i = 0; i < 360 * revolutions; i++)
        {
            float angleR = i * Mathf.Deg2Rad;
            float angleOffset = Random.Range(-2.0f, 5.0f) * Mathf.Deg2Rad;
            float distance = A / Mathf.Log10(B * Mathf.Tan(angleR / (2 * N)));
            float x = Mathf.Cos(angleR + angleOffset) * distance;
            float z = Mathf.Sin(angleR + angleOffset) * distance;

            if (distance < Mathf.Abs(A))
            {
                starInstance = Instantiate(star, transform.position + new Vector3(x, 0, z), star.transform.rotation);
                starInstance.transform.parent = StarParent.transform;
                float scale = Random.Range(0.5f, 5.0f);
                starScale.x = scale;
                starScale.y = scale;
                starScale.z = scale;
                starInstance.transform.localScale = starScale;
            }
        }

    }
}
