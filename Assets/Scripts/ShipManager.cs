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
    public Transform[] path;

    private GameObject spawnedShip;
    private GameObject spawnedVisor;
    private GameObject spawnedWingLeft;
    private GameObject spawnedWingRight;

    void Start () {
        GenerateShip();
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.transform.parent = spawnedShip.transform;
        cam.transform.position = spawnedShip.transform.position + new Vector3(0.0f, 20.0f, -40.0f);
        cam.transform.LookAt(spawnedShip.transform);
    }

    void GenerateShip()
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

        bool backThrusters = (Random.value > 0.5f);


        GameObject SpaceParent = Instantiate(new GameObject());
        SpaceParent.name = "Spacecraft";

        spawnedShip = Instantiate(ship, transform);
        spawnedShip.transform.parent = SpaceParent.transform;
        //spawnedShip.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 visorPos = spawnedShip.transform.GetChild(0).transform.position;
        Vector3 rightWingPos = spawnedShip.transform.GetChild(1).transform.position;
        Vector3 leftWingPos = spawnedShip.transform.GetChild(2).transform.position;
        Vector3 backLeftWingPos = spawnedShip.transform.GetChild(3).transform.position;
        Vector3 backRightWingPos = spawnedShip.transform.GetChild(4).transform.position;

        spawnedVisor = Instantiate(visors[Random.Range(0, 2)], visorPos, spawnedShip.transform.rotation);           // one of two visors (windshields)
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
            int weaponCount = attachmentslots - thrusterCount;

            int thrusterSet = Random.Range(0, 2);                   // pick one of two thruster types
            int weaponSet = Random.Range(0, 3);                     // one of three weapon types


            print("We want: " + thrusterCount.ToString() + " thrusters");       // TODO remove
            print("We want: " + weaponCount.ToString() + " weapons");

            int usedSlots = 0;  // counter to increment how many slots we have used already. Self-explanatory. // TODO remove this comment

            for (int i = 0; i < thrusterCount; i++)
            {
                GameObject thruster = Instantiate(thrusters[thrusterSet], spawnedWingLeft.transform.GetChild(i + 1).transform.position, thrusters[thrusterSet].transform.rotation);          // spawn a thruster on left wing
                thruster.transform.parent = spawnedWingLeft.transform;

                GameObject thruster2 = Instantiate(thrusters[thrusterSet], spawnedWingRight.transform.GetChild(i + 1).transform.position, thrusters[thrusterSet].transform.rotation);        // and right wing
                thruster2.transform.parent = spawnedWingRight.transform;

                attachmentslots--;                                                                                                                                  // remove an attachment slot for the weapons
                usedSlots++;                                                                                                                                        // & saw we've used a slot (could probably deviate this from the attachmentSlots)
                print("Spawning thruster!");
            }
            for (int j = 0; j < attachmentslots; j++)
            {
                GameObject weapon = Instantiate(weapons[weaponSet], spawnedWingLeft.transform.GetChild(usedSlots + 1).transform.position, weapons[weaponSet].transform.rotation);       // spawn a weapon on left wing
                weapon.transform.parent = spawnedWingLeft.transform;

                GameObject weapon2 = Instantiate(weapons[weaponSet], spawnedWingRight.transform.GetChild(usedSlots + 1).transform.position, weapons[weaponSet].transform.rotation);     // and right wing
                weapon2.transform.parent = spawnedWingRight.transform;

                usedSlots++;
                print("Spawning weapon!");
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
        }


        iTween.MoveTo(SpaceParent, iTween.Hash("path", path, "speed", 10, "orienttopath", true, "looktime", .6, "eastype", "linear"));
    }

    void OnDrawGizmos()
    {
        iTween.DrawPath(path);
    }
}
