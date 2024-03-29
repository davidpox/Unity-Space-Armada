﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject MainCamera;
    public GameObject FriendlyFleet;
    public List<GameObject> cameras;
    public List<GameObject> PlanetCams;
    public List<GameObject> FriendlySpaceShipCameras;
    public List<GameObject> AllSpaceShipCameras;
    public float cameraTimer;
    public bool freecam;


    // Use this for initialization
    void Start () {
        cameraTimer = 0.0f;
        freecam = false;
	}
	
    public void activateSpaceShipCameras()
    {
        int cam = Mathf.FloorToInt(Random.Range(0, FriendlySpaceShipCameras.Count));
        SwitchCameras(FriendlySpaceShipCameras[cam]);
    }

    void Update()
    {
        if (freecam)
        {
            cameraTimer += Time.deltaTime;
            if (cameraTimer >= 5.0f)
            {
                randomCamera();
                cameraTimer = 0.0f;
            }
        }

        if(cameras[1].activeInHierarchy)
        {
            cameras[1].transform.LookAt(AllSpaceShipCameras[1].transform);
        }
        if (cameras[0].activeInHierarchy) 
        {
            cameras[0].transform.LookAt(FriendlyFleet.transform);
        }
    }

    public void randomCamera()
    {

        if (Random.Range(0.0f, 1.0f) > 0.85f)        // lower chance (25%) for spaceship cam because they're quite sickening, really. 
        {
            int cam = Mathf.FloorToInt(Random.Range(0, AllSpaceShipCameras.Count));
            if(AllSpaceShipCameras[cam])
                SwitchCameras(AllSpaceShipCameras[cam]);
            
        }
        else
        {
            int cam = Mathf.FloorToInt(Random.Range(0, PlanetCams.Count));
            if (PlanetCams[cam])
                SwitchCameras(PlanetCams[cam]);
        }
    }

    public void SwitchCameras(GameObject camera)
    {
        foreach(GameObject cam in cameras)
        {
            if (!cam) { break; }
            cam.SetActive(false);
        }
        foreach (GameObject cam in PlanetCams)
        {
            if (!cam) { break; }
            cam.SetActive(false);
        }
        foreach (GameObject cam in FriendlySpaceShipCameras)
        {
            if(!cam) { break; }
            cam.SetActive(false);
        }
        foreach (GameObject cam in AllSpaceShipCameras)
        {
            if (!cam) { break; }
            cam.SetActive(false);
        }

        if (!(MainCamera == camera)) { MainCamera.SetActive(false); };

        camera.SetActive(true);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (!cameras[0].activeInHierarchy)
        {
            SwitchCameras(cameras[0]);
        }
    }
}
