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

    [SerializeField]
    private GameObject _bossPrefab;

    [SerializeField]
    private GameObject[] _powerUps;

    private List<GameObject>_commonPowerUps;

    private bool _spawning = true;

    private int _enemiesSpawned = 0;

    private WaveManager _waveManager;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _commonPowerUps = new List<GameObject>() { _powerUps[0], _powerUps[1], _powerUps[2], _powerUps[6] };
       
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void StartSpawning()
    {
        //create list of powerups for (speed,shield,tripleshot,ammo reducer)


        if (_gameManager.IsGameOver() == false)
        {
            _spawning = true;
            if (_waveManager.IsBossWave())
            {
                Vector3 spawnPos = new Vector3(0, 10, 0);

                Instantiate(_bossPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                StartCoroutine(SpawnEnemyRoutine());
            }
            
            StartCoroutine(SpawnPowerupRoutine());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2 - (_waveManager.GetCurrentWave()/100) % 100);

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
        int powerUpIndex = 0;

        yield return new WaitForSeconds(2);

        while (_spawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            
            
            int lootType = Random.Range(0, 100);
            
            if(lootType <= 40)
            {
                powerUpIndex = 6;    //ammo is index 6
            }
            else if( lootType > 40 && lootType < 70)
            {
                powerUpIndex = Random.Range(0, 4); //common powerups speed, triple shot, shields, ammo reducer
            }
            else if(lootType >= 70 && lootType <80)
            {
                powerUpIndex = 5;//this is the beam weapon powerup
            }
            else if(lootType >=80 && lootType < 90)
            {
                powerUpIndex = 7;    //missile is index 7
            }
            else
            {
                powerUpIndex = 4; //health power up
            }

          
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
