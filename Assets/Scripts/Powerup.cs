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
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime); 

        if (transform.position.y < -6f)
            Destroy(gameObject);
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
                    default:
                        break;
                }
                
                
              
            }

          //  SpriteRenderer sprite = GetComponent<SpriteRenderer>();
          //  sprite.enabled = false;

            Destroy(gameObject);
        }
    }

    
}
