using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretScript : MonoBehaviour {

    public GameObject bullet;
    private Rigidbody rb;

    void Shoot()
    {
        var b = Instantiate(bullet, gameObject.transform.position + Vector3.forward + new Vector3(0.0f, 0.1401f, 1.192f), transform.rotation);
        rb = b.GetComponent<Rigidbody>();
        rb.velocity = Vector3.forward * 10.0f;
    }
}
