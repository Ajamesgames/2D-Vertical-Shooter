using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 10f;
    [SerializeField]
    private float _fireRate = 0.2f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private float _thrusterFuel;
    [SerializeField]
    private float _thrusterRegenRate = 25;
    [SerializeField]
    private float _thrusterUseRate = 75;
    [SerializeField]
    private float _shakeIntensity = 1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    private int _shieldStrength = 3;
    [SerializeField]
    private int _ammoCount;
    private int _maxAmmoCount;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isDoubleShotActive = false;
    [SerializeField]
    private bool _isBombActive = false;
    [SerializeField]
    private bool _isHomingLaserActive = false;
    private bool _isSlowDownActive = false;
    private bool _isThrusterCalled = false;
    private bool _isCameraShaking = false;
    private Vector3 _laserOffSet = new Vector3(0, 1.15f, 0);
    private Vector3 _camOriginPos;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _doubleShotPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _homingLaserPrefab;
    [SerializeField]
    private GameObject _bombPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _outOfAmmoClip;
    [SerializeField]
    private AudioClip _damagedClip;
    private AudioSource _audioSource;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    void Start()
    {
        _maxAmmoCount = 40;
        _ammoCount = _maxAmmoCount;
        _camOriginPos = _mainCamera.transform.position;
        _thrusterFuel = 100;
        _score = 0;
        transform.position = new Vector3(0, -2f, 0);
        _rightEngine.gameObject.SetActive(false);
        _leftEngine.gameObject.SetActive(false);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
        }
        if (_uiManager == null)
        {
            Debug.Log("UIManager is null");
        }
        if (_audioSource == null)
        {
            Debug.Log("Audiosource is null");
        }
    }

    void Update()
    {
        CalculateMovement();

        CalculateThrusterActivity(); //makes _isThrusterCalled true or false based on shift key
        if (_isThrusterCalled == false) //if shift key not down then fuel will regen
        {
            ThrusterFuelRegen();
        }
        _uiManager.ThrusterSliderUpdate(_thrusterFuel);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0)
            {
                FireLaser();
            }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount == 0)
            {
                _audioSource.clip = _outOfAmmoClip; 
                _audioSource.Play(); 
            }

        if (_isCameraShaking == true)
        {
            StartCoroutine(CameraShakeRoutine());
        }

        VacuumPowerups();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float veritcalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, veritcalInput, 0);

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterFuel > 0)
        {
            _playerSpeed = 20f;
            _thruster.SetActive(true);
            ThrusterFuelUse();
        }
        else
        {
            _playerSpeed = 10f;
            _thruster.SetActive(false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterFuel > 0 && _isSlowDownActive == true) //slowdown powerup logic
        {
            _playerSpeed = 10f;
            _thruster.SetActive(true);
            ThrusterFuelUse();
        }
        else if (_isSlowDownActive == true)
        {
            _playerSpeed = 5f;
            _thruster.SetActive(false);
        }

        transform.Translate(direction * Time.deltaTime * _playerSpeed);

        if (transform.position.y > 4.5f)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, 0);
        }
        else if (transform.position.y < -5)
        {
            transform.position = new Vector3(transform.position.x, -5, 0);
        }

        if (transform.position.x > 5.15f)
        {
            transform.position = new Vector3(5.15f, transform.position.y, 0);
        }
        else if (transform.position.x < -5.15f)
        {
            transform.position = new Vector3(-5.15f, transform.position.y, 0);
        }
    }

    private void CalculateThrusterActivity()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isThrusterCalled = true;
        }
        else
        {
            _isThrusterCalled = false;
        }
    }

    private void ThrusterFuelRegen()
    {

        if (_thrusterFuel < 100)
        {
            _thrusterFuel += _thrusterRegenRate * Time.deltaTime;
        }

    }
    private void ThrusterFuelUse()
    {
        _thrusterFuel -= _thrusterUseRate * Time.deltaTime;
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isBombActive == true)
        {
            Instantiate(_bombPrefab, (transform.position + _laserOffSet), Quaternion.identity);
        }
        else if (_isHomingLaserActive == true)
        {
            Instantiate(_homingLaserPrefab, (transform.position + _laserOffSet), Quaternion.identity);
        }
        else if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isDoubleShotActive == true)
        {
            Instantiate(_doubleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, (transform.position + _laserOffSet), Quaternion.identity);
        }

        _ammoCount -= 1;
        _uiManager.UpdateAmmo(_ammoCount);
        _audioSource.clip = _laserSoundClip;
        _audioSource.Play();
    }

    IEnumerator CameraShakeRoutine()
    {
        _mainCamera.transform.position = _camOriginPos + (Random.insideUnitSphere * _shakeIntensity);
        yield return new WaitForSeconds(0.25f);
        _mainCamera.transform.position = _camOriginPos;
        _isCameraShaking = false;
    }

    private void VacuumPowerups()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject[] _powerupsLocated = GameObject.FindGameObjectsWithTag("Powerup");
            foreach (GameObject _powerup in _powerupsLocated)
            {
                Powerup _powerupScript = _powerup.GetComponent<Powerup>();
                _powerupScript.GoToPlayerPosition();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy Laser"))
        {
            Damage();
            Destroy(collision.gameObject);
        }
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
            _audioSource.clip = _damagedClip;
            _audioSource.Play();
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
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        _isCameraShaking = true;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmoPowerupActivate()
    {
        _ammoCount = _maxAmmoCount;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void ShieldActivate()
    {
        SpriteRenderer spriteRenderer = _shieldVisual.transform.GetComponent<SpriteRenderer>();

        _shieldStrength = 3;
        _isShieldActive = true;
        spriteRenderer.color = Color.white;
        _shieldVisual.SetActive(true);
    }

    public void LifePowerupPickup()
    {
        if (_lives < 3)
        {
            _lives += 1;
            _uiManager.UpdateLives(_lives);
        }

        if (_lives == 3)
        {
            _rightEngine.gameObject.SetActive(false);
            _leftEngine.gameObject.SetActive(false);
        }
        if (_lives == 2)
        {
            _rightEngine.gameObject.SetActive(true);
            _leftEngine.gameObject.SetActive(false);
        }
        if (_lives == 1)
        {
            _rightEngine.gameObject.SetActive(true);
            _leftEngine.gameObject.SetActive(true);
        }
    }

    public void LaserPowerupActive()
    {
        if (_isDoubleShotActive == false)
        {
            _isDoubleShotActive = true;
        }
        else if (_isDoubleShotActive == true && _isTripleShotActive == false)
        {
            _isTripleShotActive = true;
        }
        else if (_isDoubleShotActive == true && _isTripleShotActive == true)
        {
            _ammoCount = _maxAmmoCount;
            _uiManager.UpdateAmmo(_ammoCount);
        }
        //attack powerup is permanent, gives double shot, then triple shot, then replenishes ammo instead at max strength
    }

    public void BombPowerupActive()
    {
        _isBombActive = true;
        StartCoroutine(BombPowerDownRoutine());
    }
    IEnumerator BombPowerDownRoutine()
    {
        yield return new WaitForSeconds(3f);
        _isBombActive = false;
    }

    public void SlowDownPowerupActive()
    {
        _isSlowDownActive = true;
        StartCoroutine(SlowDownTurnOffRoutine());
    }
    IEnumerator SlowDownTurnOffRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSlowDownActive = false;
    }

    public void HomingLaserPowerupActive()
    {
        _isHomingLaserActive = true;
        StartCoroutine(HomingPowerupCountdownRoutine());
    }
    IEnumerator HomingPowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isHomingLaserActive = false;
    }

}
