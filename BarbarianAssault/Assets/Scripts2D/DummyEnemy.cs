using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{

    [SerializeField]
    private float navigationUpdate;

    private Transform enemy;
    private float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        navigationTime += Time.deltaTime;
        if (navigationTime > navigationUpdate)
        {
            enemy.position = Vector2.MoveTowards(enemy.position, new Vector2(enemy.position.x + 10, enemy.position.y), 2f * navigationTime);
            navigationTime = 0;
        }
    }
}
