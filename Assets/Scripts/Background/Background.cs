using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private float _backgroundSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        //starts at middle
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _backgroundSpeed * Time.deltaTime);

        if(transform.position.y < -1.5f)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

    }
}
