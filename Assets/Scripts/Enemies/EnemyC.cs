using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    //this object is rotated 180* on Z axis

    [SerializeField]
    private float _enemySpeed = 4f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audioSource;
    private Player _playerScript;
    private Animator _animator;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisual;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _laserBeam;
    private GameObject _player;
    private Vector3 _playerPos;
    private bool _firingLaserbeam = false;
    private bool _hasUsedLaserbeam = false;
    [SerializeField]
    private AudioClip _laserbeamClip;



    private void Start()
    {
        _player = GameObject.Find("Player");
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
            _audioSource.clip = _laserbeamClip;
        }

        ChanceForShield();

    }
    void Update()
    {
        if (_player != null)
        {
            _playerPos = _player.transform.position;
            EnemyMovement();
        }
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
        if (_firingLaserbeam == false)
        {
            transform.Translate(Vector3.up * _enemySpeed * Time.deltaTime);
        }

        if (transform.position.y <= _playerPos.y - 2 && _hasUsedLaserbeam == false)
        {
            StartCoroutine(LaserbeamAttack());
        }

        if (transform.position.x < _playerPos.x && _firingLaserbeam == true)
        {
            transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        }
        if (transform.position.x > _playerPos.x && _firingLaserbeam == true)
        {
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        }



        if (transform.position.y <= -9)
        {
            float randomX = Random.Range(-7.25f, 7.25f);

            transform.position = new Vector3(randomX, 7.5f, 0);

            _hasUsedLaserbeam = false;
        }

    }

    IEnumerator LaserbeamAttack()
    {

        _firingLaserbeam = true;
        _hasUsedLaserbeam = true;
        _laserBeam.SetActive(true);
        _audioSource.Play();
        yield return new WaitForSeconds(1f);
        _firingLaserbeam = false;
        _laserBeam.SetActive(false);

    }

    public void StopLaserbeamOnHit()
    {
        _firingLaserbeam = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _audioSource.clip = _explosionClip;
            _shieldVisual.SetActive(false);
            _playerScript.Damage();
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Laser"))
        {
            if (_isShieldActive == true)
            {
                _shieldVisual.SetActive(false);
                _isShieldActive = false;
                Destroy(other.gameObject);
            }
            else
            {
                _audioSource.clip = _explosionClip;
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _enemySpeed = 0;
                Destroy(GetComponent<Collider2D>());
                _playerScript.AddScore(10);
                _spawnManager.EnemiesRemainingTracker(1);
                Destroy(other.gameObject);
                Destroy(this.gameObject, 0.75f);
            }
        }

        if (other.CompareTag("Explosion"))
        {
            _audioSource.clip = _explosionClip;
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(10);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Bomb"))
        {
            _audioSource.clip = _explosionClip;
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(10);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.75f);
        }
    }

}
