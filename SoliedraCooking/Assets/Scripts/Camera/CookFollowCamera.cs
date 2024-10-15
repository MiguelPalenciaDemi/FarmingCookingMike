using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float speed;

    private Vector3 _offset;
    private Vector3 _targetCameraPosition;//Posicion en la que se deberá colocar la camara
    private Vector3 _velocity;//Posicion en la que se deberá colocar la camara
    // Start is called before the first frame update
    void Start()
    {
        Follow();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        _offset = new Vector3(0, height, -distance);
        
        _targetCameraPosition = target.position + _offset;// La camara por ahora no va a rotar
        
        //transform.position = Vector3.MoveTowards(transform.position, _targetCameraPosition, speed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, _targetCameraPosition, ref _velocity, speed );
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 10*speed*Time.deltaTime);
        
        //transform.LookAt(target);  
    }
}
