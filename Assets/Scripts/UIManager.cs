using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Image[] _livesImg;
    [SerializeField]
    private TMP_Text _gameoverText;
    [SerializeField]
    private TMP_Text _restartText;
    private GameManager _gameManager;

    [SerializeField]
    private TMP_Text _ammoText;


    // Start is called before the first frame update
    void Start()
    {
        _livesImg[0].gameObject.SetActive(true);
        _livesImg[1].gameObject.SetActive(true);
        _livesImg[2].gameObject.SetActive(true);
        _gameoverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _scoreText.text = "Score: 0";
        _ammoText.text = "Ammo: 15";

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        //as we lose lives, each life image is being disabled
        if (currentLives == 2)
        {
            _livesImg[2].gameObject.SetActive(false);
        }
        
        if (currentLives == 1)
        {
            _livesImg[1].gameObject.SetActive(false);
        }

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount;
    }

    void GameOverSequence()
    {
        _livesImg[0].gameObject.SetActive(false);
        StartCoroutine(GameOverFlicker());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator GameOverFlicker()
    {
        while(true)
        {
            _gameoverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameoverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
