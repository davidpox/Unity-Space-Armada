using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Orbit : MonoBehaviour
{
    public Transform orbitTarget; // Assign the sphere/planet you want to orbit
    public float speed; // how fast the ship is moving
    public float distance; // how far the ship will orbit
    public GameObject bullet;
    public int health;

    private Rigidbody rb;
    private Transform _pivot;
    private Collider[] foundItems;
    private List<GameObject> turrets;
    private Hashtable bullets;
    private float shotTimer;

    private void Start()
    {
        _pivot = new GameObject("Pivot").GetComponent<Transform>();
        transform.parent = _pivot;
        transform.localPosition = Vector3.zero;
        transform.rotation = _pivot.rotation;
        shotTimer = 0.0f;
        health = 100;

        InitializePivot();

        bullet = Resources.Load<GameObject>("bullet") as GameObject;
        if (bullet == null) print("bullet not found");

        
        // Find all turrets on a spacecraft. 
        var children = transform.GetComponentsInChildren<Transform>();
        turrets = new List<GameObject>();
        foreach (Transform child in children)
        {
            if (child.gameObject.tag == "turret")
            {
                turrets.Add(child.gameObject);
            }
        }
    }

    private void InitializePivot()
    {
        _pivot.parent = orbitTarget;
        _pivot.localPosition = Vector3.zero;
        _pivot.rotation = Random.rotation;

        

        transform.Translate(new Vector3(0, 0, -distance), Space.Self);
        if (speed >= 0)
        {
            transform.Rotate(0, -90, 90, Space.Self);
        }
        else
        {
            transform.Rotate(0, 90, 90, Space.Self);
        }
    }

    private void LateUpdate()
    {
        if (health > 0)
        {
            _pivot.Rotate(Vector3.up, speed * Time.deltaTime, Space.Self);
        }
    }
    
    private void Update()
    {
        if (health > 0)
        {
            shotTimer += Time.deltaTime;

            foundItems = Physics.OverlapSphere(transform.position, 20.0f);
            foreach (Collider coll in foundItems)
            {
                if (coll.transform.IsChildOf(gameObject.transform)) break;

                if ((gameObject.tag == "friendly" && coll.gameObject.tag == "enemy") ||
                   (gameObject.tag == "enemy" && coll.gameObject.tag == "friendly"))
                {
                    if (shotTimer >= 1.0f)
                    {
                        foreach (GameObject turret in turrets)
                        {
                            turret.transform.LookAt(new Vector3(
                                coll.transform.position.x,
                                turret.transform.position.y,
                                coll.transform.position.z
                                ), gameObject.transform.up);
                            var b = Instantiate(bullet, turret.transform.position + Vector3.forward + new Vector3(0.0f, 0.1401f, 1.192f), bullet.transform.rotation);
                            b.GetComponent<bulletScript>().target = coll.gameObject;

                            shotTimer = 0.0f;
                        }
                    }
                }
            }
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Vector3.zero, Time.deltaTime * 5);
            if(gameObject.transform.position == Vector3.zero)
            {
                Destroy(_pivot.gameObject);
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
    }
}