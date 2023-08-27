﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;
    private float _fireRate = 3f;
    private float _canFire = 0f;
    private Vector3 _laserOffset = new Vector3(0, -1, 0);
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audioSource;
    private Player _playerScript;
    private Animator _animator;

    private bool _moveLeft = false;

    [SerializeField]
    private float _sideMovementSpeed = 8f;

    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _shieldVisual;
    private bool _isShieldActive = false;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.Log("Spawn_Manager is null");
        }

        if (_playerScript == null)
        {
            Debug.Log("_playerscript is NULL");
        }

        if (_animator == null)
        {
            Debug.Log("_animator is NULL");
        }

        if (_audioSource == null)
        {
            Debug.Log("Audiosource is null");
        }
        else
        {
            _audioSource.clip = _explosionClip;
        }

        ChanceForShield();

    }
    void Update()
    {
        EnemyMovement();
        EnemyFireLaser();
    }

    private void ChanceForShield()
    {
        int shieldProbability = Random.Range(1, 101);
        if (shieldProbability > 50)
        {
            _shieldVisual.SetActive(true);
            _isShieldActive = true;
        }
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            float randomX = Random.Range(-7.25f, 7.25f);    

            transform.position = new Vector3(randomX, 7.5f, 0);
        }

        if (transform.position.x >= 7f)
        {
            _moveLeft = true;
        }
        if (_moveLeft == true)
        {
            transform.Translate(Vector3.left * _sideMovementSpeed * Time.deltaTime);
        }
        if (transform.position.x <= -7f)
        {
            _moveLeft = false;
        }
        if (_moveLeft == false)
        {
            transform.Translate(Vector3.right * _sideMovementSpeed * Time.deltaTime);
        }

    }

    private void EnemyFireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaserPrefab, (transform.position + _laserOffset), Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _shieldVisual.SetActive(false);
            _playerScript.Damage();
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _sideMovementSpeed = 0;
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.75f);
        }

        if(other.CompareTag("Laser"))
        {
            if (_isShieldActive == true)
            {
                _shieldVisual.SetActive(false);
                _isShieldActive = false;
                Destroy(other.gameObject);
            }
            else
            {
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _enemySpeed = 0;
                _sideMovementSpeed = 0;
                Destroy(GetComponent<Collider2D>());
                _playerScript.AddScore(10);
                _spawnManager.EnemiesRemainingTracker(1);
                Destroy(other.gameObject);
                Destroy(this.gameObject, 0.75f);
            }
        }

        if(other.CompareTag("Explosion"))
        {
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _sideMovementSpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(10);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(this.gameObject, 0.75f);
        }

        if(other.CompareTag("Bomb"))
        {
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _sideMovementSpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(10);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.75f);
        }
    }
}