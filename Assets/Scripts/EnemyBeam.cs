using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{

    private Player _player;

    private float _damageInterval = 1.5f;
    private float _canDamage = -1;

    private void Start()
    {
            
        
            _player = GameObject.Find("Player").GetComponent<Player>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
           // Debug.LogError("Damaging player");

            if (_player != null)
            {
                _player.Damage();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time > _canDamage)
        {

            if (other.gameObject.tag == "Player")
            {
                if (_player != null)
                {
                    _player.Damage();
                }
            }
            _canDamage = Time.time + _damageInterval;
        }
    }

}
