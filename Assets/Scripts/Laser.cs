using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        //laser goes up 8m/s


        if(transform.position.y > 8)
        {
            Destroy(this.gameObject);
        }
        //laser destroyed after off screen

    }
}
