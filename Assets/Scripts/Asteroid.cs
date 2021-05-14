using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] float _rotateSpeed = 2;

    [SerializeField] GameObject _explosion;

    private SpawnManager _spawnManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn manager reference is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);

            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.25f);

           
        }

        
    }
}
