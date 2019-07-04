using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTree : MonoBehaviour
{
    [SerializeField]
    private float navigationUpdate;

    private Transform tree;
    private float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        tree = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        navigationTime += Time.deltaTime;
        if (navigationTime > navigationUpdate)
        {
            tree.position = Vector2.MoveTowards(tree.position, new Vector2(tree.position.x - 10, tree.position.y), 2f * navigationTime);
            navigationTime = 0;
        }
    }
}
