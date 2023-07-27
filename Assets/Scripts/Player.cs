using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 5f;
    [SerializeField]
    private float _fireRate = 0.2f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Vector3 _laserOffSet = new Vector3(0, 0.8f, 0);
    [SerializeField]
    private int _lives = 3;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -2f, 0); //Player starts at starting position


        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
                //fire laser with spacebar with cooldown
            }
        

        

    }

    public void Damage()
    {
        _lives -= 1;

        if(_lives == 0)
        {
            Destroy(this.gameObject);
        }
    }





    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        //adds fire rate to time value
        Instantiate(_laserPrefab, (transform.position + _laserOffSet), Quaternion.identity);
        //fire laser with firerate

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
