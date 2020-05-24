using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (GameManager.instance == null)
            {
                Debug.Log("Game loaded");
                Instantiate(gameManager);
            }
        }

    }

}
