using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float _enemySpeed = 4f;
    private bool _moveLeft;
    private bool _fightStarted = false;
    private BoxCollider2D _bossCollider;
    [SerializeField]
    private int _lifeTotal = 25;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Animator _animator;
    [SerializeField]
    private GameObject[] _bossAttacks; //0 = spread shot, 1 = homing shot, 2 = laser attack
    private bool _startBossAttacks = false;
    private GameObject _player;
    private UIManager _uiManagerScript;
    private SpawnManager _spawnManager;

    void Start()
    {
        _bossCollider = GetComponent<BoxCollider2D>();
        _animator = gameObject.transform.GetComponent<Animator>();
        _player = GameObject.Find("Player");
        _uiManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
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

        StartCoroutine(StartBossFight());

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
        _uiManagerScript.BossHealthSliderUpdate(_lifeTotal);
        EnemyMovement();
        if (_startBossAttacks == true && _fightStarted == true)
        {
            StartCoroutine(BossAttackRoutine());
        }

    }
    IEnumerator StartBossFight()
    {
        yield return new WaitForSeconds(4f);
        _uiManagerScript.BossHealthAppear();
        _startBossAttacks = true;
        _fightStarted = true;
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
        yield return new WaitForSeconds(1f);

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
            yield return new WaitForSeconds(2f);
        }
    }

    private void BossDamage()
    {
        _lifeTotal--;
        _animator.SetTrigger("Damaged");

        if (_lifeTotal == 0)
        {
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

            _playerScript.Damage();
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
