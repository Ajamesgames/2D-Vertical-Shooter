using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserbeam : MonoBehaviour
{
    private Player _playerScript;

    void Start()
    {
        _playerScript = GameObject.Find("Player").GetComponent<Player>();

        if (_playerScript == null)
        {
            Debug.LogError("player script is null");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_playerScript != null)
            {
                _playerScript.Damage();
                this.gameObject.SetActive(false);
            }

        }
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
        }

    }
}
