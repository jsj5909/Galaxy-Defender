using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _thrustCost = 0.1f;
    
    [SerializeField]
    private float _speedBoost = 8.5f;

    [SerializeField]
    private float _normalSpeed = 5;

    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _beamPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _shields;

    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private AudioClip _laserSound;

    [SerializeField]
    private float _thrusterSpeed = 10f;

    private int _shieldStrength = 3;

    private AudioSource _audio;

    private SpawnManager _spawnManager;

    private float _canFire = -1f;

    private int _maxAmmo = 15;

    private int _currentAmmo = 15;


    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _shieldsActive = false;

    private bool _isBeamActive = false;

    private int _score;

    private UIManager _uiManager;

    private bool _thrustersActive = false;

    private float _currentThrustPower = 1f;

   

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _audio = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }

        if (_audio == null)
        {
            Debug.LogError("The audio source is null");
        }

        _speed = _normalSpeed;

        _shields.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {


        CalculateMovement();

        if (Input.GetKey(KeyCode.LeftShift) && _speed != _speedBoost)
        {
            if (_currentThrustPower > 0.1f)
            {
                _speed = _thrusterSpeed;
                _thrustersActive = true;
            }
            else
            {
                _thrustersActive = false;
                _speed = _normalSpeed;
            }
        }
        else if (_speed != _speedBoost)
        {
            _speed = _normalSpeed;
            _thrustersActive = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _currentAmmo > 0)
        {
            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                FireLaser();
            }
            _audio.PlayOneShot(_laserSound);
        }
        Debug.Log("Lives " + _lives);

        if (_shieldsActive)
        {
            SpriteRenderer shieldRenderer = _shields.GetComponent<SpriteRenderer>();

            switch (_shieldStrength)
            {
                case 0:
                    _shields.SetActive(false);
                    _shieldsActive = false;
                    break;
                case 1:
                    shieldRenderer.color = Color.red;
                    break;
                case 2:
                    shieldRenderer.color = Color.yellow;
                    break;
                case 3:
                    shieldRenderer.color = Color.white;
                    break;
                default:
                    break;
            }
        }

        if(_thrustersActive)
        {
            _currentThrustPower -=_thrustCost * Time.deltaTime;

            
            if (_currentThrustPower  < 0)
            {
                _currentThrustPower = 0;
            }

            _uiManager.SetThrusterPower(_currentThrustPower);
        }
        else
        {
            _currentThrustPower += _thrustCost/2 * Time.deltaTime;

            if (_currentThrustPower > 1f)
            {
                _currentThrustPower = 1f;
            }

            _uiManager.SetThrusterPower(_currentThrustPower);
        }

    }


    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        _currentAmmo -= 1;
        _uiManager.UpdateAmmo(_currentAmmo);


    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(move * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_shieldsActive)
        {
            _shieldStrength--;
            return;
        }

        _lives -= 1;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();



            Destroy(this.gameObject);

        }
        else
        {
            Camera.main.GetComponent<CameraShake>().ShakeCamera();
        }



    }

    public void BeamActive()
    {
        _isBeamActive = true;
        _beamPrefab.SetActive(true);
        StartCoroutine(BeamPowerDownRoutine());

    }

    IEnumerator BeamPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _beamPrefab.SetActive(false);
        _isBeamActive = false;

    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        _speed = _speedBoost;
        yield return new WaitForSeconds(5);
        _speed = _normalSpeed;
    }

    public void ShieldsActive()
    {
        _shieldsActive = true;
        _shields.SetActive(true);
        _shieldStrength = 3; // there are 3 levels of shields
    }

    public void AddScore(int points)
    {
        _score += points;

        _uiManager.UpdateScore(_score);
    }

    public void RefillAmmo()
    {
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmo(_currentAmmo);
    }
    public void Heal()
    {
        _lives += 1;


        if (_lives < 0)
            _lives = 0;

        if (_lives > 3)
            _lives = 3;


        _uiManager.UpdateLives(_lives);
    }
}

  
