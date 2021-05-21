using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    float _spawnTime;
    [SerializeField]
    GameObject []  _enemyPrefabs;
    [SerializeField]
    private GameObject _EnemyContainer;
    //[SerializeField]
    //private GameObject _tripleShotPowerupPrefab;
   // [SerializeField]
   // private GameObject _SpeedBoostPowerupPrefab;

    

    [SerializeField]
    private GameObject[] _powerUps;

    private bool _spawning = true;

    private int _enemiesSpawned = 0;

    private WaveManager _waveManager;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void StartSpawning()
    {

        if (_gameManager.IsGameOver() == false)
        {
            _spawning = true;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2);

        int currentWave = _waveManager.GetCurrentWave();

        if(currentWave > _enemyPrefabs.Length)
        {
            currentWave = _enemyPrefabs.Length;
        }    

        while(_spawning)
        {
           Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);

            int enemyToSpawn = Random.Range(0, currentWave);

           GameObject newEnemy = Instantiate(_enemyPrefabs[enemyToSpawn], posToSpawn, Quaternion.identity);

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

    public void StopSpawing()
    {
        _spawning = false;
    }
    public int GetEnemiesSpawned()
    {
        return _enemiesSpawned;
    }
   
}
