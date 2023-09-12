using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameover = false;

    void Update()
    {
        KeyCommands();
    }

    public void GameOver()
    {
        _isGameover = true;
    }

    private void KeyCommands()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameover == true)
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.M) && _isGameover == true)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
