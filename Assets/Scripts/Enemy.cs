using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4;

    private Player _player;

    // Start is called before the first frame update

    Animator _anim;

    AudioSource _audio;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _maxLateralMove = 5;

    [SerializeField] 
    private GameObject _shield;

    private bool _movingLaterally = false;

    private bool _movingLeft = false;

    
    private float _fireRate = 3.0f;

    private float _canFire = -1.0f;

    float _xOffset = 0;

    private bool _alive = true;

    private bool _shieldActive = false;

    private WaveManager _waveManager;

    private bool _ramming = false;
    private bool _hasRammedPlayer = false;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        _anim = GetComponent<Animator>();

        _audio = GetComponent<AudioSource>();

        int lateralMove = Random.Range(0, 2);

        //debug only
        //lateralMove = 0;

        if(lateralMove == 0)
        {
            _movingLaterally = false;
        }
        else
        {
            _movingLaterally = true;
        }


        
        int shields = Random.Range(0, 5);
        if(shields < _waveManager.GetCurrentWave() )   //chances of shields active scales with wave
        {
            _shieldActive = true;
            _shield.SetActive(true);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (_alive)
        {

            if (_ramming == false)
            {

                if (_player != null)
                {
                    if (Vector3.Distance(_player.transform.position, transform.position) < 4.0f)
                   {
                       _ramming = true;
                    }
                }

            }


            if (_player != null)
            {
                if (_ramming == true && _hasRammedPlayer == false)
                {
                    RamPlayer();
                }
                else
                {
                    CalculateMovement();
                }

            }
            
            if (Time.time > _canFire)
            {
                
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;

                
                
                 GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                    //Debug.Break();
                 Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                 for (int i = 0; i < lasers.Length; i++)
                 {
                     lasers[i].AssignEnemyLaser();
                 }

                 if(_player != null)
                 {
                    if (transform.position.y < _player.transform.position.y)
                    {
                        for (int i = 0; i < lasers.Length; i++)
                        {
                            lasers[i].EnemyShootingBehind();
                        }
                    }

                }
                
                //Debug.Break();
            }
        }

        ScanForPickup();
        
    }

    void ScanForPickup()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position + new Vector3(0, -2, 0), Vector3.down,20);

        

        if(hitInfo.collider != null)
        {
            //Debug.DrawRay(transform.position + new Vector3(0, -2, 0), Vector3.down, Color.green);


            if (hitInfo.collider.tag == "Power_Up")
            {
                Debug.Log("Hit PowerUp");
                
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                //Debug.Break();
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
        }
    }

    private void RamPlayer()
    {

        Vector3 playerDirection = (_player.transform.position - transform.position).normalized;   

        transform.Translate(playerDirection * _speed * Time.deltaTime);

        if(transform.position.y < _player.transform.position.y)
        {
            _ramming = false;
        }
    }

   
    void CalculateMovement()
    {

        if(_movingLaterally)
        {
            if(_movingLeft)
            {
                _xOffset -= 0.015f;
                if( _xOffset < -_maxLateralMove)
                {
                    _movingLeft = false;

                   // Debug.Log("xOffset: " + _xOffset.ToString());
                   // Debug.Log("MLM:" + _maxLateralMove.ToString());
                }
            }
            else
            {
                _xOffset += 0.01f;
                if(_xOffset > _maxLateralMove)
                {
                    _movingLeft = true;
                  // Debug.Log("xOffset: " + _xOffset.ToString());
                  //  Debug.Log("MLM:" + _maxLateralMove.ToString());
                }
            }
            
        }


        transform.Translate((Vector3.down + new Vector3(_xOffset,0,0)) * _speed * Time.deltaTime);

        //add fixed amount for lateral move
       

        if (transform.position.y < -6)
        {
            float randomX = Random.Range(-10, 10);

            transform.position = new Vector3(randomX, 5.15f, 0);
        }

        if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        
        if (other.gameObject.tag == "Player")
        {
            _ramming = false;
            _hasRammedPlayer = true;


            if (_shieldActive)
            {
                DisableShields();
                return;
            }


            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");

            _alive = false;

            if (_player != null)
            {
                _player.Damage();
            }

            _audio.Play();

            DestroyEnemy();

            
        }

        if(other.gameObject.tag == "Laser" || other.gameObject.tag == "Player_Beam_Weapon")
        {
            if(_shieldActive)
            {
                DisableShields();
                return;
            }
            
            
            
            _speed = 0;

            if (other.gameObject.tag == "Laser")
            {
                Destroy(other.gameObject);
            }

            _alive = false;

            if(_player != null)
            {
                _player.AddScore(10);
            }

            _audio.Play();

           
            DestroyEnemy();

            _anim.SetTrigger("OnEnemyDeath");
        }


        
           
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player_Beam_Weapon")
        {
            if (_shieldActive)
            {
                DisableShields();
                return;
            }
            else
            {
                DestroyEnemy();
            }
        }
    }
    public void DestroyEnemy()
    {

        if (_shieldActive)
            DisableShields();
        
        Destroy(GetComponent<Collider2D>());

        _speed = 0;
        _anim.SetTrigger("OnEnemyDeath");

        _alive = false;

        _audio.Play();

        Destroy(gameObject, 2.8f);
    }

    private void DisableShields()
    {
        _shield.SetActive(false);
        _shieldActive = false;
    }

    public bool IsAlive()
    {
        return _alive;
    }

}
