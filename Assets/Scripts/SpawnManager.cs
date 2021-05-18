using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    float _spawnTime;
    [SerializeField]
    GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _EnemyContainer;
    //[SerializeField]
    //private GameObject _tripleShotPowerupPrefab;
   // [SerializeField]
   // private GameObject _SpeedBoostPowerupPrefab;

    [SerializeField]
    private GameObject[] _powerUps;

    private bool _spawning = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2);
        
        while(_spawning)
        {
           Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);
            
           GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _EnemyContainer.transform;

            yield return new WaitForSeconds(_spawnTime);
            

        }
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2);

        while (_spawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            int powerUpIndex = Random.Range(0, _powerUps.Length);

            GameObject newPowerUp = Instantiate(_powerUps[powerUpIndex], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));

         }
    }

    public void OnPlayerDeath()
    {
        _spawning = false;
    }
}
