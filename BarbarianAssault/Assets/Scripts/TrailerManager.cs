using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerManager : MonoBehaviour
{
    public static TrailerManager instance = null;
    [SerializeField] private List<GameObject> cameras = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void startNextCamera()
    {
        Debug.Log("startNextCamera");
        cameras[0].SetActive(true);
        cameras[0].GetComponentInChildren<Camera>().enabled = true;
    }

    public void DestroyCamera(GameObject camera)
    {
        if (cameras.Count > 1)
        {
            cameras.RemoveAt(0);
            DestroyCamera(camera);
            startNextCamera();
        }
    }
}
