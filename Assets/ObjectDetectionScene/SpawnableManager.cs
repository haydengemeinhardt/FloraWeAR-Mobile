using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnableManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnablePrefab;

    Camera arCam;
    GameObject spawnedObject;
    GameObject spawnedObject2;
    private Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        //spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount == 0) 
            return; 
        Debug.Log("Hit!");
        // //spawnedObject == null;
        // //SpawnPrefab(new Vector3(2.0f, 0, 0)); 
        // //new Vector3(i * 2.0f, 0, 0)
        // RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(new Vector3(600, 600, 0));
        //Debug.Log("Hit2!");
        Debug.Log(Input.GetTouch(0).position);
        RaycastHit hit;
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit)) 
        { 
            Debug.Log("Hit5!");
        }
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;
        // // Does the ray intersect any objects excluding the player layer
        // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //     Debug.Log("Did Hit");
        // }
        // else
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //     Debug.Log("Did not Hit");
        // }
        // Debug.Log(Input.GetTouch(0).phase);
        // Debug.Log(Input.GetTouch(0).phase == TouchPhase.Began);
        // Debug.Log(ray);
        // Debug.Log(m_Hits);
        // Debug.Log(spawnedObject);
        // if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits)) 
        // { 
        //     Debug.Log("Hit3!");
        //     if(Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null) 
        //     { 
        //         Debug.Log("Hit4!");
        //         if (Physics.Raycast(ray, out hit)) 
        //         { 
        //             Debug.Log("Hit5!");
        //             if (hit.collider.gameObject.tag == "Spawnable") 
        //             { 
        //                 Debug.Log("Hit6!");
        //                 spawnedObject = hit.collider.gameObject; 
        //             } 
        //             else 
        //             { 
        //                 Debug.Log("Hit7!");
        //                 spawnedObject2 = GameObject.Find("Character-FarmWear(Clone)");
        //                 Debug.Log(spawnedObject2);
        //                 Destroy(spawnedObject2);
        //                 SpawnPrefab(m_Hits[0].pose.position); 
        //             } 
        //         } 
        //     } 
        //     else if(Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null) 
        //     { 
        //         Debug.Log("Hit8!");
        //         spawnedObject.transform.position = m_Hits[0].pose.position; 
        //     } 
        //     if(Input.GetTouch(0).phase == TouchPhase.Ended) 
        //     { 
        //         Debug.Log("Hit9!");
        //         spawnedObject = null; 
        //     } 
        // }

    }

    private void SpawnPrefab(Vector3 spawnPosition) 
    { 
        Debug.Log("Hit10!");
        Debug.Log(spawnPosition);
        //spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity); 
        //spawnedObject.transform.LookAt(Camera.main.transform);
    }
}
