using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4;

    private Player _player;

    // Start is called before the first frame update

    Animator _anim;

    

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6)
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

            Destroy(gameObject,2.8f);
        }

        if(other.gameObject.tag == "Laser")
        {
            _speed = 0;
            
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(gameObject,2.8f);

            _anim.SetTrigger("OnEnemyDeath");
        }


        
           
    }
}
