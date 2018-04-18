using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bulletScript : MonoBehaviour {

    public GameObject target;
    
	// Use this for initialization
	void Start () {
        Invoke("kill", 5.0f);
	}
	
    void Update()
    {
        if (!target) return; 
        transform.LookAt(target.transform);
        transform.Translate(Vector3.forward * 120.0f * Time.deltaTime);
    }

	void kill () {
        Destroy(gameObject);
	}

    void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.GetComponentInParent<Orbit>())
        {
            coll.transform.GetComponentInParent<Orbit>().health -= 10;
            print(coll.transform.GetComponentInParent<Orbit>().health);
        }
        kill();
    }
}
