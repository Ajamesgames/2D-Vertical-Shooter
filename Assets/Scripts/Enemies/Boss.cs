using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int _lifeTotal = 30;
    private float _enemySpeed = 4f;
    private bool _moveLeft;
    private bool _fightStarted = false;
    private bool _musicFadeOut = false;
    private bool _startBossAttacks = false;
    private BoxCollider2D _bossCollider;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject[] _bossAttacks; //0 = spread shot, 1 = homing shot, 2 = laser attack
    private GameObject _player;
    private Animator _animator;
    private UIManager _uiManagerScript;
    private SpawnManager _spawnManager;
    private AudioSource _gameMusic;
    [SerializeField]
    private AudioClip _bossMusic;
    [SerializeField]
    private AudioClip _victoryMusic;

    void Start()
    {
        _bossCollider = GetComponent<BoxCollider2D>();
        _animator = gameObject.transform.GetComponent<Animator>();
        _player = GameObject.Find("Player");
        _uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _gameMusic = GameObject.Find("Background_music").GetComponent<AudioSource>();

        if (_bossCollider == null)
        {
            Debug.Log("Box collider is null");
        }
        if (_animator == null)
        {
            Debug.LogError("animator is null");
        }
        if (_player == null)
        {
            Debug.LogError("player is null");
        }
        if (_uiManagerScript == null)
        {
            Debug.Log("Ui manager is null");
        }
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
        }
        if (_gameMusic == null)
        {
            Debug.Log("Game music is null");
        }

        StartCoroutine(StartBossFight());

        RandomStartDirection();

        _musicFadeOut = true;
    }

    void Update()
    {
        FadeOutMusic();
        _uiManagerScript.BossHealthSliderUpdate(_lifeTotal);
        EnemyMovement();
        if (_startBossAttacks == true && _fightStarted == true)
        {
            StartCoroutine(BossAttackRoutine());
        }

    }
    IEnumerator StartBossFight()
    {
        yield return new WaitForSeconds(4.5f);
        _uiManagerScript.BossHealthAppear();
        _startBossAttacks = true;
        _fightStarted = true;
        _gameMusic.volume = 0.8f;
        _gameMusic.clip = _bossMusic;
        _gameMusic.Play();
    }

    private void RandomStartDirection()
    {
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

    private void FadeOutMusic()
    {
        if (_musicFadeOut == true)
        {
            if (_gameMusic.volume <= 0.01f)
            {
                _gameMusic.Stop();
                _musicFadeOut = false;
            }
            else
            {
                _gameMusic.volume += - (0.3f * Time.deltaTime);
            }
        }
    }

    private void EnemyMovement()
    {
        if (transform.position.y > 3.25)
        {
            transform.Translate(Vector3.down * (_enemySpeed /2)  * Time.deltaTime);
        }
        if (transform.position.x >= 4f)
        {
            _moveLeft = true;
        }
        if (_moveLeft == true && _fightStarted == true)
        {
            transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        }
        if (transform.position.x <= -4f)
        {
            _moveLeft = false;
        }
        if (_moveLeft == false && _fightStarted == true)
        {
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        }
        if (_fightStarted == true)
        {
            _bossCollider.enabled = true;
        }
    }

    IEnumerator BossAttackRoutine()
    {
        _startBossAttacks = false;
        yield return new WaitForSeconds(0.75f);

        while (_fightStarted == true && _player != null)
        {
            int _randomAttack = Random.Range(0, 3);
            if (_randomAttack == 2)
            {
                Vector3 _laserSpawnPos1 = new Vector3(5.3f, 6.5f, 0);
                Vector3 _laserSpawnPos2 = new Vector3(-5.3f, 6.5f, 0);
                for (int i = 0; i < 3; i++)
                {
                    int _randomLaserPos = Random.Range(0, 2);
                    if (_randomLaserPos == 0)
                    {
                        Instantiate(_bossAttacks[2], _laserSpawnPos1, Quaternion.identity);
                    }
                    else if (_randomLaserPos == 1)
                    {
                        Instantiate(_bossAttacks[2], _laserSpawnPos2, Quaternion.identity);
                    }
                    yield return new WaitForSeconds(1f);
                }
            }
            else
            {
                Vector3 _laserOffset = new Vector3(0, -3, 0);
                Instantiate(_bossAttacks[_randomAttack], transform.position + _laserOffset, Quaternion.identity);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void BossDamage()
    {
        _lifeTotal--;
        _animator.SetTrigger("Damaged");

        if (_lifeTotal == 0)
        {
            _gameMusic.Stop();
            _gameMusic.clip = _victoryMusic;
            _gameMusic.Play();
            _fightStarted = false;
            _uiManagerScript.BossHealthDisappear();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManagerScript.YouWinScreen();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player _playerScript = other.GetComponent<Player>();
            if( _playerScript != null)
            {
                _playerScript.Damage();
            }
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            BossDamage();
        }

        if (other.CompareTag("Bomb"))
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            BossDamage();
        }
    }
}
