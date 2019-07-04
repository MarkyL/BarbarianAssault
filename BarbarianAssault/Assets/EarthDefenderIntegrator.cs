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
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInside)
        {
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
            Debug.Log("before");
            GameManager.instance.freezeEnemies(true);
            Debug.Log("after");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("A player entered EarthDefender collider");
            instructionsText.SetActive(true);
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
