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
    GameObject _attackPowerupPrefab;

    private bool _stopSpawning = false;

    

    void Start()
    {

        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());


    }

    IEnumerator SpawnRoutine()
    {
        
        while (_stopSpawning == false)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, _randomSpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3f);
        }
        //coroutine for spawning enemy prefab at random location every 5 seconds
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 _randomSpawnPos = new Vector3(Random.Range(-7.25f, 7.25f), 7.25f, 0);
            Instantiate(_attackPowerupPrefab, _randomSpawnPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }


    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}


