using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 6f;
    [SerializeField]
    private float _rotateSpeed = 1000f;
    private GameObject _player;
    private Vector3 _playerPos;




    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.Log("Player is null");
        }

        StartCoroutine(DestroyAfterTime());
    }


    void Update()
    {
        if (transform.name == "Boss_Homing_Left_laser" || transform.name == "Boss_Homing_Right_laser")
        {
            BossHomingLaserMovement();
        }
    }
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private void BossHomingLaserMovement()
    {
        if (_player != null)
        {
            _playerPos = _player.transform.position;

            transform.Translate((_playerPos - transform.position).normalized * _laserSpeed * Time.deltaTime);

            Quaternion _rotateTarget = Quaternion.LookRotation(transform.forward, -(_playerPos - transform.position).normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateTarget, _rotateSpeed * Time.deltaTime);
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
