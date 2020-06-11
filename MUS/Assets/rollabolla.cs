using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class rollabolla : MonoBehaviour
{
    public float speed;
    public float maxForce = 100.0f;

    public GameObject camera;
    private FMODEventPlayable ev;

    void FixedUpdate()
    {
        float movementHorizontal = Input.GetAxis("Horizontal");
        float movementVertical = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;

        Vector3 camPos = camera.transform.position;

        Vector3 look = pos - camPos;
        look.y = 0;
        Vector3 orthoLook = Vector3.Cross(look, Vector3.down);

        look.Normalize();   orthoLook.Normalize();

        float magnitude = GetComponent<Rigidbody>().velocity.magnitude;

        if (magnitude < maxForce)
        {
            GetComponent<Rigidbody>().AddForce(look * movementVertical * speed * Time.deltaTime);
            GetComponent<Rigidbody>().AddForce(orthoLook * movementHorizontal * speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("boop");
        }

        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.SetParameter("speed", magnitude * (15 / maxForce));
    }
}