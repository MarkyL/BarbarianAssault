using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
      public void onMenuPlayPressed()
    {
        Debug.Log("onMenuPlayPressed");
        SceneManager.LoadScene("Level1");
    }
}
