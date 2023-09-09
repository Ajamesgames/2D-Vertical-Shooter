using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefabs; //0 = wave start enemy, 1 = 1st enemy, 2 = Enemy B, 3 = enemy C, 4 = enemy D, 5 = boss.
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups; //0 = ammo, 1 = attack, 2 = defense, 3 = life, 4 = Slow down, 5 = bomb, 6 = homing.
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
    private void NewLevelStart()
    {
        Instantiate(_enemyPrefabs[0], new Vector3(0, 8, 0), Quaternion.identity);
        _isLevelEnding = false;
    }

    public void EnemiesRemainingTracker(int enemiesDestroyed)
    {
        _enemiesRemaining -= enemiesDestroyed;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnRoutine() //0 = wave start enemy, 1 = enemy 1, 2 = Enemy 2, 3 = enemy 3, 4 = enemy 4, 5 = boss
    {
        _currentLevel++;
        _enemiesToSpawn = 1 + (_currentLevel * 5);
        _enemiesRemaining = _enemiesToSpawn;

        _uiManager.LevelTextUpdate(_currentLevel);

        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false && _enemiesToSpawn > 0 && _currentLevel == 1)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefabs[1], _randomSpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesToSpawn--;
            yield return new WaitForSeconds(1f);
        }

        while (_stopSpawning == false && _enemiesToSpawn > 0 && _currentLevel == 2)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            int enemyProbability = Random.Range(1, 101);
            if (enemyProbability > 50) // if 51-100 spawn enemy 1, 50% chance
            {
                GameObject newEnemy = Instantiate(_enemyPrefabs[1], _randomSpawnPos, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemiesToSpawn--;
            }
            else // if 1-50 spawn enemy 2, 50% chance
            {
                GameObject newEnemy = Instantiate(_enemyPrefabs[2], _randomSpawnPos, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemiesToSpawn--;
            }

            yield return new WaitForSeconds(1f);
        }

        while (_stopSpawning == false && _enemiesToSpawn > 0 && _currentLevel == 3)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            int enemyProbability = Random.Range(1, 4);
            GameObject newEnemy = Instantiate(_enemyPrefabs[enemyProbability], _randomSpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesToSpawn--;
            yield return new WaitForSeconds(1f);
        }

        while (_stopSpawning == false && _enemiesToSpawn > 0 && _currentLevel == 4)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            int enemyProbability = Random.Range(1, 5);
            GameObject newEnemy = Instantiate(_enemyPrefabs[enemyProbability], _randomSpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesToSpawn--;
            yield return new WaitForSeconds(1f);
        }

        while (_stopSpawning == false && _enemiesToSpawn > 0 && _currentLevel == 5)
        {
            _enemiesToSpawn = 1;
            Instantiate(_enemyPrefabs[5], new Vector3(0, 10, 0), Quaternion.identity);
            _enemiesToSpawn--;
            yield return new WaitForSeconds(1f);
        }

        _isLevelEnding = true;
    }

    IEnumerator SpawnPowerupRoutine()  //0 = ammo, 1 = attack, 2 = defense, 3 = life, 4 = Slow down, 5 = bomb, 6 = homing.
    {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            int powerupProbability = Random.Range(1, 101);
            if (powerupProbability > 80) //ammo powerup, 20% chance
            {
                Instantiate(_powerups[0], _randomSpawnPos, Quaternion.identity);
            }
            if (powerupProbability > 60 && powerupProbability <= 80) //attack powerup, 20% chance
            {
                Instantiate(_powerups[1], _randomSpawnPos, Quaternion.identity);
            }
            if (powerupProbability > 40 && powerupProbability <= 60) //defense powerup, 20% chance
            {
                Instantiate(_powerups[2], _randomSpawnPos, Quaternion.identity);
            }
            if (powerupProbability > 30 && powerupProbability <= 40) //life powerup, 10% chance
            {
                Instantiate(_powerups[3], _randomSpawnPos, Quaternion.identity);
            }
            if (powerupProbability > 20 && powerupProbability <= 30) //slow down powerup, 10% chance
            {
                Instantiate(_powerups[4], _randomSpawnPos, Quaternion.identity);
            }
            if (powerupProbability > 10 && powerupProbability <= 20) //bomb powerup, 10% chance
            {
                Instantiate(_powerups[5], _randomSpawnPos, Quaternion.identity);
            }
            if (powerupProbability <= 10) //homing powerup, 10% chance
            {
                Instantiate(_powerups[6], _randomSpawnPos, Quaternion.identity);
            }


            yield return new WaitForSeconds(5f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }




}


