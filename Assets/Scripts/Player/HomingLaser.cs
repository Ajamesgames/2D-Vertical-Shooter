using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;
    [SerializeField]
    private float _rotateSpeed = 100f;
    private float _distanceToClosestEnemy = Mathf.Infinity;
    private float _distanceToEnemy;
    private GameObject[] _allEnemies;
    private GameObject _closestEnemy;
    private Vector3 _closestEnemyPos;
    private Quaternion _rotateTarget;

    private void Start()
    {
        HomingLaserDetection();
    }

    void Update()
    {
        HomingLaserMovement();
    }

    private void HomingLaserDetection()
    {
        _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (_allEnemies != null)
        {
            foreach (GameObject currentEnemy in _allEnemies)
            {
                _distanceToEnemy = (currentEnemy.transform.position - this.gameObject.transform.position).sqrMagnitude;

                if (_distanceToEnemy < _distanceToClosestEnemy)
                {
                    _distanceToClosestEnemy = _distanceToEnemy;
                    _closestEnemy = currentEnemy;
                }
            }
        }
    }

    private void HomingLaserMovement()
    {
        if (_allEnemies == null || _closestEnemy == null)
        {
            transform.Translate(Vector3.up * _laserSpeed / 2 * Time.deltaTime);
        }
        else if (_allEnemies != null)
        {
            _closestEnemyPos = _closestEnemy.transform.position;
            transform.Translate((_closestEnemyPos - transform.position).normalized * _laserSpeed / 2 * Time.deltaTime);

            _rotateTarget = Quaternion.LookRotation(Vector3.forward, (_closestEnemyPos - transform.position).normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateTarget, _rotateSpeed * Time.deltaTime);
        }

        if (transform.position.y > 8 || transform.position.y < -8 || transform.position.x > 12 || transform.position.x < -12)
        {
            Destroy(this.gameObject);
        }
    }
}
