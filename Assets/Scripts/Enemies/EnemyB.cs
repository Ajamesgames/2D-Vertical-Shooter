using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;
    private float _fireRate = 3f;
    private float _canFire = 0f;
    private Vector3 _laserOffset = new Vector3(0, -1, 0);
    [SerializeField]
    private GameObject _enemyBLaserPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audioSource;
    private Player _playerScript;
    private Animator _animator;
    private SpawnManager _spawnManager;

    private bool _moveLeft;
    private bool _moveDown;
    private bool _moveSideways;

    [SerializeField]
    private GameObject _shieldVisual;
    private bool _isShieldActive = false;

    private bool _isDead = false;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();

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

        StartCoroutine(SnakeMovement());

        ChanceForShield();

        int randomDirection = Random.Range(0, 2);
        if (randomDirection == 0)
        {
            _moveLeft = true;
        }
        else if (randomDirection == 1)
        {
            _moveLeft = false;
        }
    }

    void Update()
    {
        EnemyMovement();
        if (_playerScript != null && _isDead == false)
        {
            EnemyFireLaser();
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
        if (_moveDown == true)
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        }

        if (transform.position.x >= 5.3f)
        {
            _moveLeft = true;
        }
        if (_moveLeft == true && _moveSideways == true)
        {
            transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        }
        if (transform.position.x <= -5.3f)
        {
            _moveLeft = false;
        }
        if (_moveLeft == false && _moveSideways == true)
        {
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        }

        if (transform.position.y <= -7)
        {
            float randomX = Random.Range(-5.3f, 5.3f);

            transform.position = new Vector3(randomX, 7f, 0);
            StartCoroutine(SnakeMovement());
        }

    }

    IEnumerator SnakeMovement()
    {
        float randomTime = Random.Range(0.5f, 1f);

        _moveDown = true;
        _moveSideways = false;
        yield return new WaitForSeconds(randomTime);
        _moveDown = false;
        _moveSideways = true;
        yield return new WaitForSeconds(2f);
        _moveDown = true;
        _moveSideways = false;
        yield return new WaitForSeconds(randomTime);
        _moveDown = false;
        _moveSideways = true;
        yield return new WaitForSeconds(2f);
        _moveDown = true;
        _moveSideways = false;
        yield return new WaitForSeconds(randomTime);
        _moveDown = false;
        _moveSideways = true;
        yield return new WaitForSeconds(2f);
        _moveDown = true;
        _moveSideways = false;
    }

    private void EnemyFireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyBLaserPrefab, (transform.position + _laserOffset), Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isDead = true;
            _shieldVisual.SetActive(false);
            transform.localScale = new Vector3(1f, 1f, 0);
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
                _isDead = true;
                transform.localScale = new Vector3(1f, 1f, 0);
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _enemySpeed = 0;
                Destroy(GetComponent<Collider2D>());
                _playerScript.AddScore(20);
                _spawnManager.EnemiesRemainingTracker(1);
                Destroy(other.gameObject);
                Destroy(this.gameObject, 0.75f);
            }
        }

        if (other.CompareTag("Explosion"))
        {
            _isDead = true;
            _shieldVisual.SetActive(false);
            transform.localScale = new Vector3(1f, 1f, 0);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(20);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Bomb"))
        {
            _isDead = true;
            _shieldVisual.SetActive(false);
            transform.localScale = new Vector3(1f, 1f, 0);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(20);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.75f);
        }
    }


}
