using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthDefenderIntegrator : MonoBehaviour
{
    [SerializeField]
    GameObject instructionsText;

    private bool isPlayerInside = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInside && !GameManager.instance.HasWon2D)
        {
            StartCoroutine(load2DSceneAsync());
            // Freezing enemies is something we want to do right when the user clicks to move to 2D.       
            GameManager.instance.freezeEnemies(true);
        }
    }

    IEnumerator load2DSceneAsync()
    {
        Debug.Log("load2dSceneAsync");
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);
        
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");
            yield return null;
        }
        Debug.Log("Loading completed, Toggling 3D scene visibility.");
        GameManager.instance.toggleSceneVisibilityState(false);
    }

    private bool hasRecievedReward = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("A player entered EarthDefender collider");
            if (!hasRecievedReward)
            {
                if (GameManager.instance.HasWon2D)
                {
                    instructionsText.GetComponent<TextMesh>().text = "Congratulations!!! \n Collect your power ups";
                    GameManager.instance.spawnRewardPowerUps();
                    hasRecievedReward = true;
                }
                instructionsText.SetActive(true);
            }
            isPlayerInside = true;
            //instructionsText.GetComponent<TextMesh>().text = "TEST";
            //instructionsText.text = "test";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("A player exited EarthDefender collider");
            instructionsText.SetActive(false);
            isPlayerInside = false;
            //instructionsText.GetComponent<TextMesh>().text = "TEST";
            //instructionsText.text = "test";
        }
    }
}
