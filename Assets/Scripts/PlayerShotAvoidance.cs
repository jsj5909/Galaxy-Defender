using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotAvoidance : MonoBehaviour
{

    private Vector3 _rayCastOffset = new Vector3(0, -2, 0);

    private Vector3 _avoidanceStaringPosition;

    [SerializeField]
    private float _avoidanceAmount = 6;

    
    private bool _avoidanceStarted = false;
    private bool _movingLeft = true;

    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();

        if(_enemy == null)
        {
            Debug.LogError("Enemy is null");
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (_enemy.IsAlive())
        {
            RaycastHit2D hitInfo = Physics2D.CapsuleCast(transform.position + _rayCastOffset, new Vector2(2, 1), CapsuleDirection2D.Vertical, 0f, Vector2.down, 20f);

            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.tag == "Laser")
                {
                    if (_avoidanceStarted == false)
                    {
                        StartAvoidance(hitInfo);
                    }
                    

                    if (_movingLeft == true)
                    {
                        transform.Translate(Vector3.left * _avoidanceAmount * Time.deltaTime);

                        if (transform.position.x <= _avoidanceStaringPosition.x - _avoidanceAmount)
                        {
                            _avoidanceStarted = false;
                        }

                    }
                    else
                    {
                        transform.Translate(Vector3.right * _avoidanceAmount * Time.deltaTime);

                        if (transform.position.x >= _avoidanceStaringPosition.x + _avoidanceAmount)
                        {
                            _avoidanceStarted = false;
                        }

                    }



                }
            }
        }
    
    }
    
  private void StartAvoidance(RaycastHit2D hitInfo)
    {
        _avoidanceStaringPosition = transform.position;
        _avoidanceStarted = true;


        if (hitInfo.transform.position.x <= transform.position.x)
        {
            _movingLeft = false;
        }
        else
        {
            _movingLeft = true;
        }


    }


}
