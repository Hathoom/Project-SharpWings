using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingItems : MonoBehaviour
{


    //public Transform facing;
    // Start is called before the first frame update

    public Vector3 _rotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //for when the powerup is to always face player
        // transform.LookAt(new Vector3(facing.position.x, transform.position.y, facing.position.z));
        transform.Rotate(_rotation * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}