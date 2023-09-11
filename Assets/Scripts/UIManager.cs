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
    private TMP_Text _gameoverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private Image[] _livesImg;
    [SerializeField]
    private Slider _thrusterSlider;
    private GameManager _gameManager;

    [SerializeField]
    private TMP_Text _levelText;

    [SerializeField]
    private Slider _bossHealthSlider;
    [SerializeField]
    private TMP_Text _youWinText;

    [SerializeField]
    private Slider _ammoSlider;


    void Start()
    {
        _levelText.gameObject.SetActive(false);
        _livesImg[0].gameObject.SetActive(true);
        _livesImg[1].gameObject.SetActive(true);
        _livesImg[2].gameObject.SetActive(true);
        _gameoverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _youWinText.gameObject.SetActive(false);

        _scoreText.text = "Score: 0";

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
        if (currentLives == 3)
        {
            _livesImg[2].gameObject.SetActive(true);
            _livesImg[1].gameObject.SetActive(true);
            _livesImg[0].gameObject.SetActive(true);
        }
        if (currentLives == 2)
        {
            _livesImg[2].gameObject.SetActive(false);
            _livesImg[1].gameObject.SetActive(true);
            _livesImg[0].gameObject.SetActive(true);
        }
        
        if (currentLives == 1)
        {
            _livesImg[2].gameObject.SetActive(false);
            _livesImg[1].gameObject.SetActive(false);
            _livesImg[0].gameObject.SetActive(true);
        }

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoSlider.value = ammoCount;
    }

    private void GameOverSequence()
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

    public void ThrusterSliderUpdate(float thrusterValue)
    {
        _thrusterSlider.value = thrusterValue;
    }

    public void LevelTextUpdate(int waveCount)
    {
        _levelText.gameObject.SetActive(true);
        _levelText.text = "Level: " + waveCount;
        StartCoroutine(LevelTextTurnOff());
    }

    IEnumerator LevelTextTurnOff()
    {
        yield return new WaitForSeconds(3f);
        _levelText.gameObject.SetActive(false);
    }

    public void BossHealthSliderUpdate(int bossHealth)
    {
        _bossHealthSlider.value = bossHealth;
    }

    public void BossHealthAppear()
    {
        _bossHealthSlider.gameObject.SetActive(true);
    }

    public void BossHealthDisappear()
    {
        _bossHealthSlider.gameObject.SetActive(false);
    }

    public void YouWinScreen()
    {
        StartCoroutine(YouWinFlicker());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator YouWinFlicker()
    {
        while (true)
        {
            _youWinText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _youWinText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
