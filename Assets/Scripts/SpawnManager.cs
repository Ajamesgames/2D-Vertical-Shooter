using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;
    [SerializeField]
    GameObject _enemyContainer;

    private bool _stopSpawning = false;
    

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SpawnRoutine());


    }

    // Update is called once per frame
    void Update()
    {
        


    }

    //spawn enemys every 5 seconds
    IEnumerator SpawnRoutine()
    {
        
        while (_stopSpawning == false)
        {
            Vector3 _enemySpawnPos = new Vector3(Random.Range(-8.5f, 8.5f), 7.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, _enemySpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3f);
        }
        //coroutine for spawning enemy prefab at random location every 5 seconds
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}


