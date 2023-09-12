using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 6f;
    private bool _firingLaserbeam = false;
    private bool _hasUsedLaserbeam = false;
    private bool _isShieldActive = false;
    private bool _isDead = false;
    private Vector3 _playerPos;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _laserBeam;
    private GameObject _player;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private AudioClip _explosionClip;
    [SerializeField]
    private AudioClip _laserbeamClip;
    private AudioSource _audioSource;
    private Player _playerScript;
    private Animator _animator;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.Log(transform.name + "Couldnt locate player");
        }
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

        transform.Rotate(0, 0, 180);

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

        if (transform.position.y <= _playerPos.y - 2 && _hasUsedLaserbeam == false && _isDead == false)
        {
            StartCoroutine(LaserbeamAttack());
        }

        if (transform.position.x < _playerPos.x && _firingLaserbeam == true)
        {
            transform.Translate(Vector3.left * (_enemySpeed /2) * Time.deltaTime);
        }
        if (transform.position.x > _playerPos.x && _firingLaserbeam == true)
        {
            transform.Translate(Vector3.right * (_enemySpeed /2) * Time.deltaTime);
        }

        if (transform.position.y <= -9)
        {
            float randomX = Random.Range(-5.3f, 5.3f);

            transform.position = new Vector3(randomX, 7f, 0);

            _hasUsedLaserbeam = false;
        }
    }

    IEnumerator LaserbeamAttack()
    {
        _firingLaserbeam = true;
        _hasUsedLaserbeam = true;
        _laserBeam.SetActive(true);
        _audioSource.Play();
        yield return new WaitForSeconds(Random.Range(1f,1.5f));
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
            _isDead = true;
            _audioSource.clip = _explosionClip;
            _shieldVisual.SetActive(false);
            _playerScript.Damage();
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _spawnManager.EnemiesRemainingTracker(1);
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
                _isDead = true;
                _audioSource.clip = _explosionClip;
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _enemySpeed = 0;
                Destroy(GetComponent<Collider2D>());
                _playerScript.AddScore(30);
                _spawnManager.EnemiesRemainingTracker(1);
                Destroy(other.gameObject);
                Destroy(this.gameObject, 0.75f);
            }
        }

        if (other.CompareTag("Explosion"))
        {
            _isDead = true;
            _audioSource.clip = _explosionClip;
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(30);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Bomb"))
        {
            _isDead = true;
            _audioSource.clip = _explosionClip;
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(30);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.75f);
        }
    }

}
