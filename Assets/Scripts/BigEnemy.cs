using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField]
    private float _bigEnemySpeed = 8f;
    [SerializeField]
    private float _descentSpeed = 2f;
    [SerializeField]
    private bool _moveLeft = false;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;


    void Start()
    {
        _moveLeft = true;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("SpawnManager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        BigEnemyMovement();
    }

    private void BigEnemyMovement() //moves enemy left and right
    {
        if (transform.position.y > 4f)
        {
            transform.Translate(Vector3.down * _descentSpeed * Time.deltaTime);
        }



        if (transform.position.x >= 5f)
        {
            _moveLeft = true;
        }
        if (_moveLeft == true)
        {
            transform.Translate(Vector3.left * _bigEnemySpeed * Time.deltaTime);
        }
        if (transform.position.x <= -5f)
        {
            _moveLeft = false;
        }
        if (_moveLeft == false)
        {
            transform.Translate(Vector3.right * _bigEnemySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}

