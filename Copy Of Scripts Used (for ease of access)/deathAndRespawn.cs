using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathAndRespawn : MonoBehaviour
{
    
    public GameObject death;
    public GameObject respawn;


    // Update is called once per frame
    void LateUpdate()
    {
        //checks to see if the ball should be respawned

        if (transform.position.y <= death.transform.position.y)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            transform.position = respawn.transform.position;
        }
    }
}
