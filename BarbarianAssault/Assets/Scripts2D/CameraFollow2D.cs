using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private float navigationUpdate;

    private float navigationTime = 0;
    private float distanceToTarget;

    // Use this for initialization
    void Start()
    {
        distanceToTarget = transform.localPosition.x - targetObject.transform.localPosition.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        navigationTime += Time.deltaTime;
        if (navigationTime > navigationUpdate) { 
            float targetObjectX = targetObject.transform.localPosition.x;

            Vector3 newCameraPosition = transform.localPosition;
            newCameraPosition.x = targetObjectX + distanceToTarget;
            transform.localPosition = newCameraPosition;
            navigationTime = 0;
         }
    }
}
