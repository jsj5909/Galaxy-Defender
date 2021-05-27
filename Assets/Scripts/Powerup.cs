using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    float _speed = 1;

    [SerializeField]
    private int _powerUpID;  //0 - triple shot, 1 - speed // 2 - shields

  
    [SerializeField]
    private AudioClip _clip;

    private Player _player;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime); 

        if (transform.position.y < -6f)
            Destroy(gameObject);

        if (Input.GetKey(KeyCode.C))
        {
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;

            transform.Translate(directionToPlayer * _speed * Time.deltaTime);
            
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            
            Player player = collision.gameObject.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip,transform.position);

            if(player != null)
            {
               switch(_powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.Heal();
                        break;
                    case 5:
                        player.BeamActive();
                        break;
                    case 6:
                        player.ReduceAmmo();
                        break;
                    default:
                        break;
                }
                
                
              
            }

            

            Destroy(gameObject);
        }
    }

    
}
