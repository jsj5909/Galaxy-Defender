using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originalPosition;
    
    [SerializeField] private float _shakeTime = 0.3f;

    [SerializeField] private GameObject _player;

    private float _shakeEnd = -1;

    private bool _shaking = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;



    }

    // Update is called once per frame
    void Update()
    {
        if(_shaking)
        {
            float x, y;
            x = Random.Range(_player.transform.position.x-1f, _player.transform.position.x+1f);
            y = Random.Range(_player.transform.position.y-1f, _player.transform.position.y+1f);
            
            if( Time.time < _shakeEnd)
            {
                // transform.position = Random.insideUnitSphere;

                transform.position = new Vector3(x, y, transform.position.z);
            }
            else
            {
                _shaking = false;
                transform.position = _originalPosition;
            }
        }



    }

    public void ShakeCamera()
    {
        _shakeEnd = Time.time + _shakeTime;
        _shaking = true;
    }
}
