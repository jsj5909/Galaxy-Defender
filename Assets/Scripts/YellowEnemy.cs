using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowEnemy : MonoBehaviour
{
    [SerializeField] float _speed = 4;

    private Player _player;

    // Start is called before the first frame update

    Animator _anim;

    AudioSource _audio;

    [SerializeField]
    private GameObject _beamLaser;

    [SerializeField]
    private float _beamActivationTime = 3;

    [SerializeField]
    private float _maxLateralMove = 7;

    private bool _movingLaterally = false;

    private bool _movingLeft = false;


    private float _fireRate = 3.0f;

    private float _canFire = -1.0f;

    float _xOffset = 0;

    float _yDestination = 0;

    private bool _alive = true;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();

        _audio = GetComponent<AudioSource>();

        _beamLaser.gameObject.SetActive(false);

        _yDestination = Random.Range(1, 5);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_alive)
        {
            CalculateMovement();

            if (Time.time > _canFire && (transform.position.y <= _yDestination))  //dont start firing until we finish the drop
            {
                _fireRate = Random.Range(5f, 7f);
                _canFire = Time.time + _fireRate;
                StartCoroutine(ActivateBeam());
            }
        }
    }

    IEnumerator ActivateBeam()
    {
        _beamLaser.gameObject.SetActive(true);
        yield return new WaitForSeconds(_beamActivationTime);
        _beamLaser.gameObject.SetActive(false);
    }

    void CalculateMovement()
    {

        if (transform.position.y > _yDestination)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else
        {
            if(transform.position.x < -10)
            {
                _movingLeft = false;
            }
            if(transform.position.x > 10)
            {
                _movingLeft = true;
            }
            
            
            if (_movingLeft)
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                
            }
            else
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
        }

        


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");

            _alive = false;

            if (_player != null)
            {
                _player.Damage();
            }

            _audio.Play();

            //Destroy(gameObject, 2.8f);
            DestroyEnemy();


        }

        if (other.gameObject.tag == "Laser" || other.gameObject.tag == "Player_Beam_Weapon")
        {
            _speed = 0;

            if (other.gameObject.tag == "Laser")
            {
                Destroy(other.gameObject);
            }

            _alive = false;

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _audio.Play();

            Destroy(GetComponent<Collider2D>());
            //Destroy(gameObject, 2.8f);
            DestroyEnemy();

            _anim.SetTrigger("OnEnemyDeath");
        }

    }
    public void DestroyEnemy()
    {
        _beamLaser.gameObject.SetActive(false);
        
        _speed = 0;
        _anim.SetTrigger("OnEnemyDeath");

        _alive = false;

        _audio.Play();

        

        Destroy(gameObject, 2.8f);
    }
}
