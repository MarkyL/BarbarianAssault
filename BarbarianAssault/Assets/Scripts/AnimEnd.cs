using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEnd : MonoBehaviour
{
    public void onAnimationEnd()
    {
        TrailerManager.instance.DestroyCamera(gameObject);
    }
}
