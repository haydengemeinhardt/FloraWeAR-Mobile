using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
public class AR_Cursor : MonoBehaviour
{
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    public Camera playercam;

    Vector3 newDirection;
    Vector3 newRotation;

    public Rigidbody projectile;

    void Start()
    {
        InvokeRepeating("LaunchProjectile", 2.0f, 1.0f);
    }

    void LaunchProjectile()
    {
        // This stuff is basic AR setup 
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        // If the user is touching the screen
        if (hits.Count > 0)
        {
            // Sets position to plane nearest to where they touches
            objectToPlace.transform.position = hits[0].pose.position;

            // Basic algorithm to set rotation to always face the camera
            newRotation = playercam.transform.position - hits[0].pose.position;
            newDirection = Vector3.RotateTowards(objectToPlace.transform.forward, newRotation, 6.2832f, 6f);
            objectToPlace.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }


    // void Update()
    // {
    //     // This stuff is basic AR setup 
    //     List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //     raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

    //     // If the user is touching the screen
    //     if (hits.Count > 0)
    //     {
    //         // Sets position to plane nearest to where they touches
    //         objectToPlace.transform.position = hits[0].pose.position;

    //         // Basic algorithm to set rotation to always face the camera
    //         newRotation = playercam.transform.position - hits[0].pose.position;
    //         newDirection = Vector3.RotateTowards(objectToPlace.transform.forward, newRotation, 6.2832f, 6f);
    //         objectToPlace.transform.rotation = Quaternion.LookRotation(newDirection);
    //     }
    // }
}
