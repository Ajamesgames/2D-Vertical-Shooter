using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefabs; //0 = wave start enemy, 1 = 1st enemy
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups; //0 = speed, 1 = attack, 2 = defense, 3 = life
    [SerializeField]
    private GameObject[] _rarePowerups; //0 = bomb
    private bool _stopSpawning = false;

    private UIManager _uiManager;

    private int _currentLevel = 0;
    [SerializeField]
    private int _enemiesToSpawn;
    [SerializeField]
    private int _enemiesRemaining;
    private bool _isLevelEnding = true;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.Log("UIManager is Null");
        }
    }

    private void Update()
    {
        if (_enemiesRemaining == 0 && _isLevelEnding == true)
        {
            NewLevelStart();
            StopAllCoroutines();
        }
    }

    public void EnemiesRemainingTracker(int enemiesDestroyed)
    {
        _enemiesRemaining -= enemiesDestroyed;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());

        
    }

    private void NewLevelStart()
    {
        Instantiate(_enemyPrefabs[0], new Vector3(0, 8, 0), Quaternion.identity);
        _isLevelEnding = false;
    }

    IEnumerator SpawnRoutine()
    {
        _currentLevel++;
        _enemiesToSpawn = 1 + (_currentLevel * 5);
        _enemiesRemaining = _enemiesToSpawn;

        _uiManager.LevelTextUpdate(_currentLevel);

        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false && _enemiesToSpawn > 0)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefabs[1], _randomSpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesToSpawn--;
            yield return new WaitForSeconds(1f);
        }

        _isLevelEnding = true;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_stopSpawning == false)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            int randomPowerup = Random.Range(0, 4);
            Instantiate(_powerups[randomPowerup], _randomSpawnPos, Quaternion.identity);

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(15f);

        while (_stopSpawning == false)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            int randomPowerup = Random.Range(0, 2);
            Instantiate(_rarePowerups[0], _randomSpawnPos, Quaternion.identity);

            yield return new WaitForSeconds(30f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }




}


