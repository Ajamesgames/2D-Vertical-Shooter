using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;
    private Vector3 _playerPosition;

    private void Start()
    {
        _playerPosition = GameObject.Find("Player").transform.position;

        if (_playerPosition == null)
        {
            Debug.Log("Player position is Null");
        }
        StartCoroutine(DestroyLaser());
    }

    void Update()
    {
        transform.Translate((_playerPosition - transform.position).normalized * _laserSpeed * Time.deltaTime);
    }

    IEnumerator DestroyLaser()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Explosion"))
        {
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Bomb"))
        {
            Destroy(this.gameObject);
        }
    }
}
