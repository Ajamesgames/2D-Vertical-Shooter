using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;
    [SerializeField]
    GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups; //0 = speed, 1 = attack, 2 = defense, 3 = life

    private bool _stopSpawning = false;

    
    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_stopSpawning == false)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, _randomSpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(1f);
        }
        //coroutine for spawning enemy prefab at random location every 5 seconds
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

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}


