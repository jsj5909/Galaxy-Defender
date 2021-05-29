using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private Text _waveCompleteText;

    [SerializeField]
    private float _waveCompleteFlickerTime = 3;

    private int _currentWave = 1;

    private int _killsThisWave = 0;

    private int _enemiesInWave = 1;

    [SerializeField]
    private int _waveEnemyScalingFactor = 2;

    [SerializeField]
    private int _minimumEnemiesPerWaive = 8;

    private bool _waveTransitioning = false;

    

    // Start is called before the first frame update
    void Start()
    {
        _enemiesInWave = _currentWave * _waveEnemyScalingFactor + _minimumEnemiesPerWaive;


       
    }

    // Update is called once per frame
    void Update()
    {
        //start at wave 1  x amount of enemies
        // when current kills = x amount of enemies
        //waive complete notify
        //start next waive
        //asteroid

        //next waive has waive number has 2 more enemies than previous
        if (_waveTransitioning == false)
        {

            if (_killsThisWave >= _enemiesInWave)
            {
                _spawnManager.StopSpawing();

               Enemy[] enemies = FindObjectsOfType<Enemy>();
                YellowEnemy[] yellowEnemies = FindObjectsOfType<YellowEnemy>();
                HomingMissile[] missiles = FindObjectsOfType<HomingMissile>();


                foreach (Enemy enemy in enemies)
                {
                    enemy.DestroyEnemy();
                }
                foreach (YellowEnemy enemy in yellowEnemies)
                {
                    enemy.DestroyEnemy();
                }
                foreach (HomingMissile missile in missiles)
                {
                    missile.DestroyMissile();
                }


                StartCoroutine(WaveTransitionFlicker());
            }
        }
/////Debuggin purposes ONly//////////////////////////////////////////////////////////////////////////
        if(Input.GetKeyDown(KeyCode.V))
        {
            _spawnManager.StopSpawing();

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            YellowEnemy[] yellowEnemies = FindObjectsOfType<YellowEnemy>();
            HomingMissile[] missiles = FindObjectsOfType<HomingMissile>();


            foreach (Enemy enemy in enemies)
            {
                enemy.DestroyEnemy();
            }
            foreach (YellowEnemy enemy in yellowEnemies)
            {
                enemy.DestroyEnemy();
            }
            foreach (HomingMissile missile in missiles)
            {
                missile.DestroyMissile();
            }


            StartCoroutine(WaveTransitionFlicker());
        }
///////End Debugging lines///////////////////////////////////////////////////////////////////////////////

    }
    private void PrepareNextWave()
    {
        _currentWave++;

        _killsThisWave = 0;

        _enemiesInWave = _currentWave * _waveEnemyScalingFactor + _minimumEnemiesPerWaive;   //8 is the minimum in a wave
    }

    IEnumerator WaveTransitionFlicker()
    {
        _waveTransitioning = true;
        
        float endFlicker = _waveCompleteFlickerTime + Time.time;

        _waveCompleteText.text = "Wave " + _currentWave + " Complete";

        while (Time.time < endFlicker)
        {
            _waveCompleteText.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            _waveCompleteText.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }
        
        endFlicker = _waveCompleteFlickerTime + Time.time;

        PrepareNextWave();
        _waveCompleteText.text = "Starting Wave " + _currentWave;

        while (Time.time < endFlicker)
        {
            _waveCompleteText.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            _waveCompleteText.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }

        _spawnManager.StartSpawning();

        _waveTransitioning = false;

    }


    public void IncrementKillCount()
    {
        _killsThisWave += 1;
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }
}
