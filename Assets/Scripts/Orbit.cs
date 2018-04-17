using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform orbitTarget; // Assign the sphere/planet you want to orbit
    public float speed; // how fast the ship is moving
    public float distance; // how far the ship will orbit

    private Transform _pivot;

    private void Start()
    {
        _pivot = new GameObject("Pivot").GetComponent<Transform>();
        //_pivot.rotation = Random.rotation;
        transform.parent = _pivot;
        transform.localPosition = Vector3.zero;
        transform.rotation = _pivot.rotation;

        InitializePivot();
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
        _pivot.Rotate(Vector3.up, speed * Time.deltaTime, Space.Self);
    }
}