using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;
    private Player _playerScript;

    private void Start()
    {
        _playerScript = GameObject.Find("Player").transform.GetComponent<Player>();

    }
    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            float randomX = Random.Range(-7.25f, 7.25f);
            //random x range variable

            transform.position = new Vector3(randomX, 7.5f, 0);
        }
        //if enemy goes under 6y, teleport to x boundary range at 7.5y

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

        if(other.CompareTag("Laser"))
        {
            _playerScript.AddScore(10);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        //collision information with player and laser

    }

}
