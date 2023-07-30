﻿using System.Collections;
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
    private Vector3 _laserOffSet = new Vector3(0, 1.15f, 0);
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isDoubleShotActive = false;
    [SerializeField]
    private GameObject _doubleShotPrefab;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -2f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        

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

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float veritcalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, veritcalInput, 0);

        transform.Translate(direction * Time.deltaTime * _playerSpeed);
        //player movement speed and control

        if (transform.position.y > 4.5f)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, 0);
        }
        else if (transform.position.y < -5)
        {
            transform.position = new Vector3(transform.position.x, -5, 0);
        }

        if (transform.position.x > 7.5f)
        {
            transform.position = new Vector3(7.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -7.5f)
        {
            transform.position = new Vector3(-7.5f, transform.position.y, 0);
        }
        //player movement boundries
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate; //adds fire rate to time value

        if (_isDoubleShotActive == true)
        {
            Instantiate(_doubleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, (transform.position + _laserOffSet), Quaternion.identity);
        }
             

    }

    public void Damage()
    {
        _lives -= 1;

        if (_lives == 0)
        {
            //_stopSpawning = true
            _spawnManager.OnPlayerDeath();
            //tells spawnmanager to stop spawning
            Destroy(this.gameObject);
        }
    }

    public void LaserPowerupActive()
    {
        _isDoubleShotActive = true;
        StartCoroutine(PowerupCountdownRoutine());
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isDoubleShotActive = false;
    }



}
