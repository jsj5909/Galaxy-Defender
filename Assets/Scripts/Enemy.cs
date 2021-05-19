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

    
    private float _fireRate = 3.0f;

    private float _canFire = -1.0f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();

        _audio = GetComponent<AudioSource>();



    }

    // Update is called once per frame
    void Update()
    {
        
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            //Debug.Break();
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for( int i=0; i< lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }


            //Debug.Break();
        }

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            float randomX = Random.Range(-10, 10);

            transform.position = new Vector3(randomX, 5.15f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");

            if (_player != null)
            {
                _player.Damage();
            }

            _audio.Play();

            Destroy(gameObject,2.8f);

            
        }

        if(other.gameObject.tag == "Laser" || other.gameObject.tag == "Player_Beam_Weapon")
        {
            _speed = 0;

            if (other.gameObject.tag == "Laser")
            {
                Destroy(other.gameObject);
            }

            if(_player != null)
            {
                _player.AddScore(10);
            }

            _audio.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject,2.8f);

            _anim.SetTrigger("OnEnemyDeath");
        }


        
           
    }
 

}
