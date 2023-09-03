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

        if (transform.position.y < -12)
        {
            Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }
}
