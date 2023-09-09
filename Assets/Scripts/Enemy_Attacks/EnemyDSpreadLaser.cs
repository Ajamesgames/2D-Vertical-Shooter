using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDSpreadLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;

    void Update()
    {
        transform.Translate(-transform.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -15)
        {
            Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
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
