using System.Collections;
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
        MainCamera.SetActive(true);
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
            cameras[2].transform.LookAt(AllSpaceShipCameras[1].transform);
        }
        if (cameras[0].activeInHierarchy)
        {
            cameras[0].transform.LookAt(FriendlyFleet.transform);
        }
    }

    void randomCamera()
    {

        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            int cam = Mathf.FloorToInt(Random.Range(0, AllSpaceShipCameras.Count));
            SwitchCameras(AllSpaceShipCameras[cam]);
        }
        else
        {
            int cam = Mathf.FloorToInt(Random.Range(0, PlanetCams.Count));
            SwitchCameras(PlanetCams[cam]);
        }
    }

    public void SwitchCameras(GameObject camera)
    {
        foreach(GameObject cam in cameras)
        {
            cam.SetActive(false);
        }
        foreach (GameObject cam in PlanetCams)
        {
            cam.SetActive(false);
        }
        foreach (GameObject cam in FriendlySpaceShipCameras)
        {
            cam.SetActive(false);
        }
        foreach (GameObject cam in AllSpaceShipCameras)
        {
            cam.SetActive(false);
        }

        if (!(MainCamera == camera)) { MainCamera.SetActive(false); };

        if(!camera)
        {
            print("Camera does not exist");
        }

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
