using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {

    public GameObject ship;
    public GameObject visor1;
    public GameObject visor2;
    public GameObject wing;
    public GameObject wing2;
    public GameObject wingBack;
    public GameObject thruster1;
    public GameObject thruster2;
    public GameObject turret1;
    public GameObject turret2;
    public GameObject missiles;
    public Transform[] pathF;
    public Transform[] pathE;

    public Material enemyColor;
    public Material enemyThruster;
    public Material enemyOrange;

    private GameObject friendlyFleet;
    private GameObject enemyFleet;
    private GameObject test;
    private GameObject testParent;

    void Start () {

        friendlyFleet = new GameObject("friendlyFleet");
        friendlyFleet.transform.position = new Vector3(300, 20, 300);
        enemyFleet = new GameObject("enemyFleet");
        enemyFleet.transform.position = new Vector3(-300, 20, -300);

        int z = 0;
        int x = 0;
        for (int i = 0; i < 9; i++)     // Spawns the ships at an offset. 
        {            
            if (i % 3 == 0) { z += 10; x = 0; }

            GameObject ship = GenerateShip(new Vector3(300, 20, 300) + new Vector3(x + Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), z + Random.Range(-3.0f, 3.0f)));
            ship.transform.parent = friendlyFleet.transform;
            BoxCollider bc1 = ship.AddComponent<BoxCollider>();
            bc1.size = new Vector3(100.0f, 100.0f, 100.0f);
            bc1.isTrigger = true;
            ship.tag = "friendly";


            GameObject ship2 = GenerateShip(new Vector3(-300, 20, -300) + new Vector3(x + Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), z + Random.Range(-3.0f, 3.0f)), true);
            ship2.transform.parent = enemyFleet.transform;
            BoxCollider bc2 = ship2.AddComponent<BoxCollider>();
            bc2.size = new Vector3(100.0f, 100.0f, 100.0f);
            bc2.isTrigger = true;
            ship2.tag = "enemy";



            x += 10;
        }

        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.transform.parent = friendlyFleet.transform;
        cam.transform.position = friendlyFleet.transform.GetChild(4).position + new Vector3(0.0f, 5.0f, -10.0f);
        cam.transform.LookAt(friendlyFleet.transform.GetChild(4));


        iTween.MoveTo(friendlyFleet, iTween.Hash("name", "friendlyPath", "path", pathF, "speed", 30.0f, "orienttopath", true, "looktime", 0.6f, "easetype", iTween.EaseType.linear, "oncomplete", "activateOrbitFriendly", "onCompleteTarget", GameObject.Find("Galaxy Generator")));
    }


    GameObject GenerateShip(Vector3 pos, bool enemy = false)
    {
        List<GameObject> visors = new List<GameObject>();
        visors.Add(visor1);
        visors.Add(visor2);

        List<GameObject> wings = new List<GameObject>();
        wings.Add(wing);
        wings.Add(wing2);
        wings.Add(wingBack);

        List<GameObject> thrusters = new List<GameObject>();
        thrusters.Add(thruster1);
        thrusters.Add(thruster2);

        List<GameObject> weapons = new List<GameObject>();
        weapons.Add(turret1);
        weapons.Add(turret2);
        weapons.Add(missiles);

        bool backThrusters = (Random.value > 0.7f);


        GameObject SpaceParentlocal = new GameObject("Spacecraft");
        if (enemy) SpaceParentlocal.name = "SpacecraftENEMY";

        GameObject spawnedShip = Instantiate(ship, SpaceParentlocal.transform);
        spawnedShip.transform.parent = SpaceParentlocal.transform;
        if (enemy)
        {
            spawnedShip.transform.GetChild(5).GetComponent<TrailRenderer>().material = enemyThruster;
            Material[] mats = spawnedShip.GetComponentInChildren<MeshRenderer>().materials;
            mats[1] = enemyOrange;
            spawnedShip.GetComponentInChildren<MeshRenderer>().materials = mats;
        }
        
        Vector3 visorPos = spawnedShip.transform.GetChild(0).transform.position;
        Vector3 rightWingPos = spawnedShip.transform.GetChild(1).transform.position;
        Vector3 leftWingPos = spawnedShip.transform.GetChild(2).transform.position;
        Vector3 backLeftWingPos = spawnedShip.transform.GetChild(3).transform.position;
        Vector3 backRightWingPos = spawnedShip.transform.GetChild(4).transform.position;


        GameObject spawnedWingLeft;
        GameObject spawnedWingRight;

        GameObject spawnedVisor = Instantiate(visors[Random.Range(0, 2)], visorPos, spawnedShip.transform.rotation);           // one of two visors (windshields)
        spawnedVisor.transform.parent = spawnedShip.transform;

        if (!backThrusters)                                                                  // if we're using front thrusters, use one of two wing styles 
        {
            int wingSet = Random.Range(0, 2);
            //Setting up the wings
            spawnedWingLeft = Instantiate(wings[wingSet], leftWingPos, wings[wingSet].transform.rotation);
            spawnedWingLeft.transform.parent = spawnedShip.transform;

            spawnedWingRight = Instantiate(wings[wingSet], rightWingPos, wings[wingSet].transform.rotation);
            spawnedWingRight.transform.parent = spawnedShip.transform;
            spawnedWingRight.transform.transform.localScale = new Vector3(spawnedWingRight.transform.localScale.x, spawnedWingRight.transform.localScale.y, -spawnedWingRight.transform.localScale.z);        // mirrors the wing

            // Thruster & Weapons spawning. This part is a huge mess, sorry. 

            int attachmentslots;
            if (wingSet == 0) { attachmentslots = 3; }          // determining how many slots we have for thrusters & weapons depending on wing type. Probably shouldn't hardcode this?
            else { attachmentslots = 1; }

            int thrusterCount = Random.Range(0, attachmentslots + 1);       // Generate a random number of thrusters, use the rest for weapons

            int thrusterSet = Random.Range(0, 2);                   // pick one of two thruster types
            int weaponSet = Random.Range(0, 3);                     // one of three weapon types

            int usedSlots = 0;

            for (int i = 0; i < thrusterCount; i++)
            {
                GameObject thruster = Instantiate(thrusters[thrusterSet], spawnedWingLeft.transform.GetChild(i + 1).transform.position, thrusters[thrusterSet].transform.rotation);          // spawn a thruster on left wing
                thruster.transform.parent = spawnedWingLeft.transform;

                GameObject thruster2 = Instantiate(thrusters[thrusterSet], spawnedWingRight.transform.GetChild(i + 1).transform.position, thrusters[thrusterSet].transform.rotation);        // and right wing
                thruster2.transform.parent = spawnedWingRight.transform;

                if (enemy)
                {
                    thruster.GetComponentInChildren<TrailRenderer>().material = enemyThruster;
                    Material[] mats = thruster.GetComponentInChildren<MeshRenderer>().materials;
                    mats[2] = enemyOrange;
                    thruster.GetComponentInChildren<MeshRenderer>().materials = mats;

                    thruster2.GetComponentInChildren<TrailRenderer>().material = enemyThruster;
                    thruster2.GetComponentInChildren<MeshRenderer>().materials[2] = enemyOrange;
                    thruster2.GetComponentInChildren<MeshRenderer>().materials = mats;
                }

                attachmentslots--;                                                                                                                                  // remove an attachment slot for the weapons
                usedSlots++;                                                    // & say we've used a slot (could probably derive this from the attachmentSlots)
            }
            for (int j = 0; j < attachmentslots; j++)
            {
                GameObject weapon = Instantiate(weapons[weaponSet], spawnedWingLeft.transform.GetChild(usedSlots + 1).transform.position, weapons[weaponSet].transform.rotation);       // spawn a weapon on left wing
                weapon.transform.parent = spawnedWingLeft.transform;
                weapon.tag = "turret";

                GameObject weapon2 = Instantiate(weapons[weaponSet], spawnedWingRight.transform.GetChild(usedSlots + 1).transform.position, weapons[weaponSet].transform.rotation);     // and right wing
                weapon2.transform.parent = spawnedWingRight.transform;
                weapon.tag = "turret";

                usedSlots++;
            }
        }
        else                                                                                  // else, instantiate the back wings & thrusters. No weapons on this guy. 
        {
            int thrusterSet = Random.Range(0, 2);

            spawnedWingLeft = Instantiate(wings[2], backLeftWingPos, wings[2].transform.rotation);
            spawnedWingLeft.transform.parent = spawnedShip.transform;

            spawnedWingRight = Instantiate(wings[2], backRightWingPos, wings[2].transform.rotation);
            spawnedWingRight.transform.parent = spawnedShip.transform;
            spawnedWingRight.transform.transform.localScale = new Vector3(spawnedWingRight.transform.localScale.x, spawnedWingRight.transform.localScale.y, -spawnedWingRight.transform.localScale.z);        // mirrors the wing

            GameObject thruster1 = Instantiate(thrusters[thrusterSet], spawnedWingLeft.transform.GetChild(1).transform.position, thrusters[thrusterSet].transform.rotation);// Quaternion.Euler(0, 0, 0));
            thruster1.transform.parent = spawnedWingLeft.transform;
            thruster1.transform.Rotate(Vector3.left, 90.0f);
            GameObject thruster2 = Instantiate(thrusters[thrusterSet], spawnedWingRight.transform.GetChild(1).transform.position, thrusters[thrusterSet].transform.rotation);
            thruster2.transform.parent = spawnedWingRight.transform;
            thruster2.transform.Rotate(Vector3.left, -90.0f);

            if (enemy)
            {
                thruster1.GetComponentInChildren<TrailRenderer>().material = enemyThruster;
                Material[] mats = thruster1.GetComponentInChildren<MeshRenderer>().materials;
                mats[2] = enemyOrange;
                thruster1.GetComponentInChildren<MeshRenderer>().materials = mats;

                thruster2.GetComponentInChildren<TrailRenderer>().material = enemyThruster;
                thruster2.GetComponentInChildren<MeshRenderer>().materials[2] = enemyOrange;
                thruster2.GetComponentInChildren<MeshRenderer>().materials = mats;
            }
        }



        SpaceParentlocal.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //SpaceParentlocal.transform.position = path[0].position;

        SpaceParentlocal.transform.position = pos;
        ship.transform.position = Vector3.zero;
        return SpaceParentlocal;
    }

    void activateOrbitFriendly()
    {
        print("Completed Friendly Path");

        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.transform.parent = enemyFleet.transform;
        cam.transform.position = enemyFleet.transform.GetChild(4).position + new Vector3(0.0f, 5.0f, -10.0f);
        cam.transform.LookAt(enemyFleet.transform.GetChild(4));
        
        iTween.StopByName("friendlyPath");
        foreach (Transform _ship in friendlyFleet.transform)
        {
            if (_ship.gameObject.tag == "MainCamera") break;
            _ship.gameObject.AddComponent<Orbit>();
            _ship.gameObject.GetComponent<Orbit>().orbitTarget = GameObject.Find("PlanetLand").transform;
            _ship.gameObject.GetComponent<Orbit>().distance = Vector3.Distance(Vector3.zero, _ship.transform.position);
            _ship.gameObject.GetComponent<Orbit>().speed = Random.Range(40, 90);
        }
        iTween.MoveTo(enemyFleet, iTween.Hash("name", "enemyPath", "path", pathE, "speed", 30.0f, "orienttopath", true, "looktime", 0.6f, "easetype", iTween.EaseType.linear, "oncomplete", "activateOrbitEnemy", "onCompleteTarget", GameObject.Find("Galaxy Generator")));
    }

    void activateOrbitEnemy()
    {
        print("Completed Enemy Path");
        iTween.StopByName("enemyPath");
        foreach (Transform _ship in enemyFleet.transform)
        {
            if (_ship.gameObject.tag == "MainCamera") break;
            _ship.gameObject.AddComponent<Orbit>();
            _ship.gameObject.GetComponent<Orbit>().orbitTarget = GameObject.Find("PlanetLand").transform;
            _ship.gameObject.GetComponent<Orbit>().distance = Vector3.Distance(Vector3.zero, _ship.transform.position);
            _ship.gameObject.GetComponent<Orbit>().speed = Random.Range(40, 90);
        }
    }

    void OnDrawGizmosSelected()
    {
        iTween.DrawPath(pathF);
        iTween.DrawPath(pathE);
    }
}
