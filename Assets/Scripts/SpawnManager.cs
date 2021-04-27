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

    private bool _spawning = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while(_spawning)
        {
           Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);
            
           GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _EnemyContainer.transform;

           yield return new WaitForSeconds(_spawnTime);
        }

       
    }

    public void OnPlayerDeath()
    {
        _spawning = false;
    }
}
