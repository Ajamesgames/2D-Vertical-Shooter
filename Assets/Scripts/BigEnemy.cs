using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField]
    private float _bigEnemySpeed = 4f;
    [SerializeField]
    private bool moveLeft = false;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        moveLeft = true;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        BigEnemyMovement();


    }

    private void BigEnemyMovement() //moves enemy left and right
    {
        if (transform.position.x >= 7f)
        {
            moveLeft = true;
        }
        if (moveLeft == true)
        {
            transform.Translate(Vector3.left * _bigEnemySpeed * Time.deltaTime);
        }
        if (transform.position.x <= -7f)
        {
            moveLeft = false;
        }
        if (moveLeft == false)
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
    //check for laser collision 2d trigger
    //instantiate explision at current pos
    //destroy explosion after 1second
}

