using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    
    private Player _player;

    [SerializeField]
    private float _speed = 8;

    Animator _anim;

    private EnemyBaseClass _target;

    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();

        FindNewTarget();
        
    }

    // Update is called once per frame
    void Update()
    {


        if (_target == null)
        {
            FindNewTarget();
            
        }
        else
        {

            Vector3 directionToPlayer = (_target.transform.position - transform.position).normalized;


            transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);

            transform.Translate(directionToPlayer * _speed * Time.deltaTime);
        }

    }

    private void FindNewTarget()
    {
        if(_player == null)
        {
            DestroyMissile();
            return;
        }


        float distanceToClosestEnemy = Mathf.Infinity;

      
        EnemyBaseClass[] enemies = FindObjectsOfType<EnemyBaseClass>();
        
        Debug.Log("Enemies Found: " + enemies.Length);

        foreach (EnemyBaseClass enemy in enemies)
        {
            float distanceToNextEnemy = Vector3.Distance(_player.transform.position, enemy.transform.position);

            if (distanceToNextEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToNextEnemy;
                _target = enemy;
            }

        }

        


    }

    public void DestroyMissile()
    {
      
            Destroy(GetComponent<Collider2D>());

            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");

            Destroy(gameObject,3);
        

    }

}
