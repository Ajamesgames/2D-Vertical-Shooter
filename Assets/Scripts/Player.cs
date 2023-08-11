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
    private float _speedPowerupSpeed = 20f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isDoubleShotActive = false;
    private Vector3 _laserOffSet = new Vector3(0, 1.15f, 0);
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _doubleShotPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisual;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _thruster;

    private int _shieldStrength = 3;


    void Start()
    {
        _score = 0;
        transform.position = new Vector3(0, -2f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _rightEngine.gameObject.SetActive(false);
        _leftEngine.gameObject.SetActive(false);
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.Log("Audiosource is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
        }
        if (_uiManager == null)
        {
            Debug.Log("UIManager is null");
        }

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _playerSpeed = 20f;
            _thruster.SetActive(true);
        }
        else
        {
            _playerSpeed = 10f;
            _thruster.SetActive(false);
        }


        if(_isSpeedBoostActive == true)
        {
            transform.Translate(direction * Time.deltaTime * _speedPowerupSpeed);
        }
        else
        {
            transform.Translate(direction * Time.deltaTime * _playerSpeed);
        }
        

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

        _audioSource.Play();

    }

    public void Damage()
    {
        SpriteRenderer spriteRenderer = _shieldVisual.transform.GetComponent<SpriteRenderer>();

        if (_isShieldActive == true)
        {
            _shieldStrength -= 1;
        }
        else if (_isShieldActive == false)
        {
            _lives -= 1;
        }

        if (_shieldStrength == 2)
        {
            spriteRenderer.color = Color.yellow;
        }
        if (_shieldStrength == 1)
        {
            spriteRenderer.color = Color.red;
        }

        if (_isShieldActive == true && _shieldStrength == 0)
        {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }


        if (_lives == 2)
        {
            _rightEngine.gameObject.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.gameObject.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath(); //tells spawnmanager to stop spawning

            Destroy(this.gameObject);
        }
    }

    public void ShieldActivate()
    {
        SpriteRenderer spriteRenderer = _shieldVisual.transform.GetComponent<SpriteRenderer>();

        _shieldStrength = 3;
        _isShieldActive = true;
        spriteRenderer.color = Color.white;
        _shieldVisual.SetActive(true);
    }

    public void LaserPowerupActive()
    {
        _isDoubleShotActive = true;
        StartCoroutine(PowerupCountdownRoutine());
    }

    public void SpeedPowerupActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isDoubleShotActive = false;

        
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy Laser"))
        {
            Damage();
            Destroy(collision.gameObject);
        }
    }




}
