﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserbeam : MonoBehaviour
{
    private EnemyC _enemyScript;

    void Start()
    {
        _enemyScript = transform.parent.GetComponent<EnemyC>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player _playerScript = collision.GetComponent<Player>();

            _playerScript.Damage();
            _enemyScript.StopLaserbeamOnHit();
            this.gameObject.SetActive(false);
        }
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
        }
    }


}
