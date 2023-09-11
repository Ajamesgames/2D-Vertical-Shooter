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
    private bool _moveLeft;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _ammoPowerup;


    void Start()
    {
        int randomDirection = Random.Range(0, 2);
        if (randomDirection == 0)
        {
            _moveLeft = true;
        }
        else if (randomDirection == 1)
        {
            _moveLeft = false;
        }
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("SpawnManager is null");
        }

        StartCoroutine(SpawnAmmoPowerup());
    }

    // Update is called once per frame
    void Update()
    {
        BigEnemyMovement();
    }

    IEnumerator SpawnAmmoPowerup()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Instantiate(_ammoPowerup, transform.position, Quaternion.identity);
        }
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
            Instantiate(_ammoPowerup, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            Player _playerScript = collision.GetComponent<Player>();

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Instantiate(_ammoPowerup, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            _playerScript.Damage();
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Bomb"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Instantiate(_ammoPowerup, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}

