using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float _playerSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -2f, 0); //Player starts at starting position


        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float veritcalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, veritcalInput, 0);

        //transform.Translate(Vector3.right * Time.deltaTime * _playerSpeed * horizontalInput);
        //transform.Translate(Vector3.up * Time.deltaTime * _playerSpeed * veritcalInput);

        transform.Translate(direction * Time.deltaTime * _playerSpeed);
        //player movement

        //if player position on y is greater than 5
        //player position = 5 on y
        //else if player position is less than -4
        //player position = -4 on y


        if (transform.position.y > 5)
        {
            transform.position = new Vector3(transform.position.x, 5, 0);
        }
        else if (transform.position.y < -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        //if player position on x is greater than 11
        //player wraps to position -11 on x
        //else if player position on x is less than -11
        //player wraps to positon 11 on x

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }



    }
}
