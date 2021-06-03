using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBaseClass
{
    [SerializeField] GameObject _triLaserPrefab;
    [SerializeField] GameObject _beam;
    [SerializeField] GameObject _shields;

    [SerializeField] UIManager _uiManager;

    [SerializeField] float _speed = 3;
    [SerializeField] float _lateralSpeed = 5;

    [SerializeField] float _centerY = 3.62f;

    [SerializeField] private int _maxTriFire = 10;

    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 100;

    private int _currentTriFireAmount = 0;
    private int _triFireAmount = 5;


    private float _canFire = -1;
    private float _fireRate = 1;

    // Start is called before the first frame update

   
    private bool _moving = false;
   
    private bool _movingLeft = true;
    private bool _movingRight = false;
    private bool _enteringScene = true;
    private bool _triFiring = false;

    private bool _returningToCenter = false;
    private bool _shieldsActive = false;

    private bool _alive = true;

    private Animator _anim;
    private AudioSource _audio;

    private Player _player;
    

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _uiManager.SetBossHealth(_maxHealth);
        _uiManager.ShowBossHealth();

        _anim = GetComponent<Animator>();

        _audio = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        //shields on while moving
        ///move down to center
        //fire a random pattern
        //move up
        //fire randompattern while movig left and right
     if(_alive)
        { 
        if(_enteringScene == true)
        {
            //StartCoroutine(FireWeapons());
            
            ActivateShields();
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if(transform.position.y <= _centerY)
            {
                _enteringScene = false;

                _triFiring = true;

                _triFireAmount = Random.Range(5, _maxTriFire);
            }
        }
        if (_moving == false && _enteringScene == false && _triFiring == true)
        {
            DeactivateShields();

            if(_currentTriFireAmount >= _triFireAmount)
            {
                _triFiring = false;
                _moving = true;
            }
                
            if (Time.time > _canFire)
            {
             FireTriWeapons();

             _canFire = Time.time + _fireRate;
            
            }
            
        }

            if (_moving)
            {
                ActivateShields();
                ActivateBeamWeapon();

                if (_movingLeft && _returningToCenter == false)
                {
                    transform.Translate(Vector3.left * _speed * Time.deltaTime);

                    if (transform.position.x < -8)
                    {
                        _movingLeft = false;
                        _returningToCenter = true;
                        _movingRight = true;

                    }
                }

                if (_movingRight && _returningToCenter == false)
                {
                    transform.Translate(Vector3.right * _speed * Time.deltaTime);

                    if (transform.position.x > 8)
                    {
                        _movingRight = false;
                        _returningToCenter = true;
                        _movingLeft = true;
                    }
                }



                if (_returningToCenter)
                {
                    Vector3 moveDirection = (new Vector3(0, transform.position.y, 0) - transform.position).normalized;

                    transform.Translate(moveDirection * _speed * Time.deltaTime);

                    if (transform.position.x >= -0.01 && transform.position.x <= 0.01)
                    {
                        _returningToCenter = false;
                        _moving = false;

                        DeactivateShields();
                        DeactivateBeamWeapon();
                        _triFiring = true;
                        _triFireAmount = Random.Range(5, _maxTriFire);
                        _currentTriFireAmount = 0;

                        Debug.Log("Back at center");
                    }
                }
            }
     }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser" || other.gameObject.tag == "Player_Beam_Weapon" || other.gameObject.tag == "Missile")
        {
            Debug.Log(other.gameObject.tag);
            if(_shieldsActive == false)
            {
                           
                Damage(other.gameObject.tag);

            }

            if (other.gameObject.tag == "Missile")
            {
                HomingMissile missile = other.gameObject.GetComponent<HomingMissile>();
                missile.DestroyMissile();
            }
            else if (other.gameObject.tag == "Laser")
            {
                Destroy(other.gameObject);
            }

        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player_Beam_Weapon")
        {
            if(_shieldsActive == false)
            {
                Damage(other.gameObject.tag);
            }
        }
    }

    private void Damage(string damageType)
    {
        if(damageType == "Missile")
        {
            _health -= 20;
        }
        if(damageType == "Laser")
        {
            _health -= 10;
        }
        if(damageType == "Player_Beam_Weapon")
        {
            Debug.Log("Beam Hitting");
            _health -= 0.2f;
        }

        _uiManager.SetBossHealth(_health);

        if(_health <= 0)
        {
            BossDeath();
        }
    }

    private void BossDeath()
    {
        _player.AddScore(50);
        
        //Debug.Log("Boss Died");
        DeactivateBeamWeapon();
        DeactivateShields();

        Destroy(GetComponent<Collider2D>());

        _speed = 0;
        _anim.SetTrigger("OnEnemyDeath");

        _alive = false;

        _audio.Play();

        Destroy(gameObject, 2.8f);

        _uiManager.HideBossHealth();

    }

    void FireTriWeapons()
    {
        _currentTriFireAmount++;
        
        GameObject bossLaser = Instantiate(_triLaserPrefab, transform.position, Quaternion.identity);

        Laser[] lasers = bossLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }

    }

    void ActivateBeamWeapon()
    {
        _beam.SetActive(true);
    }

    void ActivateShields()
    {
        _shields.SetActive(true);
        _shieldsActive = true;
    }

    void DeactivateShields()
    {
        _shields.SetActive(false);
        _shieldsActive = false;
    }
    void DeactivateBeamWeapon()
    {
        _beam.SetActive(false);
    }
}
