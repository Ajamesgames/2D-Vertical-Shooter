using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserbeam : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player _playerScript = collision.GetComponent<Player>();

            _playerScript.Damage();
            this.gameObject.SetActive(false);
        }
        if (collision.CompareTag("Laser") || collision.CompareTag("Bomb"))
        {
            Destroy(collision.gameObject);
        }

    }
}
