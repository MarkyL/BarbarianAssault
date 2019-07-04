using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{
    // my code
    public bool scrolling, paralax;

    public float backgroundSize;
    public float paralaxSpeed;
    public float lastCameraX;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex, rightIndex;

    // rocket mouse code
    //public Renderer upperBackground;
    //public Renderer middleBackground;
    //public Renderer lowerBackground;

    //public float upperBackgroundSpeed = 0.02f;
    //public float middleBackgroundSpeed = 0.06f;
    //public float lowerBackgroundSpeed = 0.1f;

    //public float offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        Debug.Log("start - transform.childCount = " + transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }
        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void scrollLeft()
    {
        int lastRight = rightIndex;
        layers[rightIndex].position = new Vector2(layers[rightIndex].position.x - backgroundSize, layers[rightIndex].position.y);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }
    
    private void ScrollRight()
    {
        
        int lastLeft = leftIndex;
        
        layers[leftIndex].position = new Vector2(layers[rightIndex].position.x + backgroundSize, layers[rightIndex].position.y);
        //Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        //new Vector2(layers[leftIndex].position.x + backgroundSize, layers[leftIndex].position.y);
        
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (paralax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * paralaxSpeed);
        }

        lastCameraX = cameraTransform.position.x;

        if (scrolling)
        {
            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
            {
                ScrollRight();
            }
            //else if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone)) {
            //    scrollLeft();
            //}
        }
        
    }
}

//float upperBackgroundOffset = offset * upperBackgroundSpeed;
//float middleBackgroundOffset = offset * middleBackgroundSpeed;
//float lowerBackgroundOffset = offset * lowerBackgroundSpeed;

//upperBackground.material.mainTextureOffset = new Vector2(upperBackgroundOffset, 0);
//middleBackground.material.mainTextureOffset = new Vector2(middleBackgroundOffset, 0);
//lowerBackground.material.mainTextureOffset = new Vector2(lowerBackgroundOffset, 0);
