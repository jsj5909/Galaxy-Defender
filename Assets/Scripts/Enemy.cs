using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4;

    // Start is called before the first frame update
    void Start()
    {
        
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //damage Player
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
            
           
    }
}
