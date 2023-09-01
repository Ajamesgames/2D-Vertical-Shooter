using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserbeam : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    private EnemyC _enemyScript;
    private Player _playerScript;




    void Start()
    {
        _enemyScript = _enemy.GetComponent<EnemyC>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();

        if (_playerScript == null)
        {
            Debug.LogError("player script is null");
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _playerScript.Damage();
            _enemyScript.StopLaserbeamOnHit();
            this.gameObject.SetActive(false);
        }


    }


}
