using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    [SerializeField]private GameObject mainCam;
    [SerializeField] private GameObject secondaryCam;

    AudioListener mainAudLis;
    AudioListener secondaryAudLis;

    // Start is called before the first frame update
    void Start()
    {
        mainAudLis = mainCam.GetComponent<AudioListener>();
        secondaryAudLis = secondaryCam.GetComponent<AudioListener>();


        changeCameraPosition(PlayerPrefs.GetInt("CameraPosition"));
    }

    // Update is called once per frame
    void Update()
    {
        switchCamera();
    }

    void switchCamera()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            changeCameraCounter();
        }

    }

     void changeCameraCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        changeCameraPosition(cameraPositionCounter);

    }

    void changeCameraPosition(int cameraPosition)
    {
        if (cameraPosition > 1)
            cameraPosition = 0;

        PlayerPrefs.SetInt("CameraPosition", cameraPosition);

        if(cameraPosition == 0)
        {
            mainCam.GetComponentInChildren<Camera>().enabled = true;
            //mainCam.SetActive(true);
            mainAudLis.enabled = true;

            secondaryCam.GetComponentInChildren<Camera>().enabled = false;
           // secondaryCam.SetActive(false);
            secondaryAudLis.enabled = false;
        }

        if(cameraPosition == 1)
        {
            mainCam.GetComponentInChildren<Camera>().enabled = false;
            //mainCam.SetActive(false);
            mainAudLis.enabled = false;

            secondaryCam.GetComponentInChildren<Camera>().enabled = true;
            //secondaryCam.SetActive(true);
            secondaryAudLis.enabled = true;
        }
    }
}
