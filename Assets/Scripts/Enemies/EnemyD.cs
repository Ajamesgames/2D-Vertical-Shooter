using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyD : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 3f;
    [SerializeField]
    private GameObject _enemySpreadLaserPrefab;
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
    private BoxCollider2D _playerCollider;
    private bool _detectedTarget = false;
    private bool _canFireLaser = true;

    private bool _moveLeft;
    private bool _moveUp = false;
    private bool _isDodgingLaser = false;
    private bool _canDodgeLaser = true;
    [SerializeField]
    private float _dodgeMovementSpeed = 6f;
    [SerializeField]
    private GameObject _thrusterVisual;

    private bool _isDead = false;

    private void Start()
    {
        _playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.transform.GetComponent<Animator>();
        _audioSource = gameObject.transform.GetComponent<AudioSource>();

        if (_playerCollider == null)
        {
            Debug.Log("player collider is null");
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
            _audioSource.clip = _explosionClip;
        }

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
        EnemyDodgeLaserDetect();
        EnemyMovement();

        EnemyLaserDetection();
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

    private void EnemyDodgeLaserDetect()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2.5f);

        foreach (Collider2D hitDetected in hitColliders)
        {
            if (hitDetected.CompareTag("Laser"))
            {
                _isDodgingLaser = true;
            }
        }
    }

    void EnemyMovement()
    {
        if (_isDodgingLaser == true && _canDodgeLaser == true)
        {
            StartCoroutine(DodgeLaserRoutine());
        }

        //vertical movement
        if (transform.position.y <= 0.5f)
        {
            _moveUp = true;
        }
        if (_moveUp == true)
        {
            transform.Translate(Vector3.up * _enemySpeed * Time.deltaTime);
        }
        if (transform.position.y >= 4.5)
        {
            _moveUp = false;
        }
        if (_moveUp == false)
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        }
        //horizontal movement
        if (transform.position.x >= 5.3f)
        {
            _moveLeft = true;
        }
        if (_moveLeft == true)
        {
            transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        }
        if (transform.position.x <= -5.3f)
        {
            _moveLeft = false;
        }
        if (_moveLeft == false)
        {
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        }
    }

    IEnumerator DodgeLaserRoutine()
    {
        if (_moveLeft == true)
        {
            _moveLeft = false;
            _canDodgeLaser = false;
        }
        else if (_moveLeft == false)
        {
            _moveLeft = true;
            _canDodgeLaser = false;
        }

        _enemySpeed = _dodgeMovementSpeed;
        _thrusterVisual.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _thrusterVisual.SetActive(false);
        _isDodgingLaser = false;
        _canDodgeLaser = true;
        _enemySpeed = 4f;
    }

    private void EnemyLaserDetection()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.down);

        foreach (RaycastHit2D hitDetect in hit)
        {
            if (hitDetect.collider == _playerCollider || hitDetect.collider.CompareTag("Powerup"))
            {
                _detectedTarget = true;
            }
        }
    }

    private void EnemyFireLaser()
    {
        if (_detectedTarget == true && _canFireLaser == true && _isDead == false)
        {
            StartCoroutine(EnemyFireLaserRoutine());
        }
    }

    IEnumerator EnemyFireLaserRoutine()
    {
        Instantiate(_enemySpreadLaserPrefab, transform.position, Quaternion.identity);
        _canFireLaser = false;
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        _detectedTarget = false;
        _canFireLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isDead = true;
            _shieldVisual.SetActive(false);
            _playerScript.Damage();
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _dodgeMovementSpeed = 0;
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
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _enemySpeed = 0;
                _dodgeMovementSpeed = 0;
                Destroy(GetComponent<Collider2D>());
                _playerScript.AddScore(40);
                _spawnManager.EnemiesRemainingTracker(1);
                Destroy(other.gameObject);
                Destroy(this.gameObject, 0.75f);
            }
        }

        if (other.CompareTag("Explosion"))
        {
            _isDead = true;
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _dodgeMovementSpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(40);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(this.gameObject, 0.75f);
        }

        if (other.CompareTag("Bomb"))
        {
            _isDead = true;
            _shieldVisual.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _dodgeMovementSpeed = 0;
            Destroy(GetComponent<Collider2D>());
            _playerScript.AddScore(40);
            _spawnManager.EnemiesRemainingTracker(1);
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.75f);
        }
    }

}
