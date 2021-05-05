using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    private float _fireRate = 0.5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _shields;
    
    private SpawnManager _spawnManager;

    private float _canFire = -1f;

  
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _shieldsActive = false;

    private int _score;

    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }

        _speed = _normalSpeed;

        _shields.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            { 
                FireLaser(); 
            }

        }
        Debug.Log("Lives " + _lives);
    }


    void FireLaser()
    {    
           _canFire = Time.time + _fireRate;
           Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);


        
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
            _shields.SetActive(false);
            _shieldsActive = false;
            return;
        }

        _lives -= 1;

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();

           
            
            Destroy(this.gameObject);

        }
    
        for(int i=0; i<_speed;i++)
        {

        }
        
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
    }

    public void AddScore(int points)
    {
        _score += points;

        _uiManager.UpdateScore(_score);
    }
}
