using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyLaserbeam : MonoBehaviour
{

    private float _enemySpeed = 4;
    private bool _isLaserbeamReady = false;
    private bool _hasUsedLaserbeam = false;
    [SerializeField]
    private GameObject _laserbeam;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _laserbeamClip;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audioSource;
    private Animator _animator;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();

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
            _audioSource.clip = _laserbeamClip;
        }

        if (transform.position.x > 0)
        {
            transform.Rotate(0, 0, 270);
        }
        else if (transform.position.x < 0)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    void Update()
    {
        EnemyMovement();
        FireLaserbeam();
    }

    private void EnemyMovement()
    {
        transform.Translate(transform.up * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -9)
        {
            Destroy(this.gameObject);
        }
    }

    private void FireLaserbeam()
    {
        if (transform.position.y <= 1)
        {
            _isLaserbeamReady = true;
        }
        if (_isLaserbeamReady == true && _hasUsedLaserbeam == false)
        {
            StartCoroutine(LaserbeamAttack());
        }
    }
    IEnumerator LaserbeamAttack()
    {
        _hasUsedLaserbeam = true;
        _laserbeam.SetActive(true);
        _audioSource.Play();
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player _playerScript = other.GetComponent<Player>();
            if (_playerScript != null)
            {
                _playerScript.Damage();
            }
            _laserbeam.SetActive(false);
            _audioSource.clip = _explosionClip;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Laser"))
        {
            _laserbeam.SetActive(false);
            _audioSource.clip = _explosionClip;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Explosion"))
        {
            _laserbeam.SetActive(false);
            _audioSource.clip = _explosionClip;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Bomb"))
        {
            _laserbeam.SetActive(false);
            _audioSource.clip = _explosionClip;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.75f);
        }
    }

}

