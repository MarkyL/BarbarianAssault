using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField] private float startWaitTime;
    private float waitTime;
    private UnityEngine.AI.NavMeshAgent nav;

    [SerializeField] private Transform[] moveSpots;
    private int startingPoint;


    

    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        startingPoint = Random.Range(0, moveSpots.Length);
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        nav.speed = speed;
        if (Vector3.Distance(transform.position, moveSpots[startingPoint].transform.position) >= 2)
        {
            if(nav.enabled)
                nav.SetDestination(moveSpots[startingPoint].transform.position);
        }
        else
        {
            startingPoint += 1;
            if(startingPoint >= moveSpots.Length)
            {
                startingPoint = 0;
            }
        }
        
    }
}
