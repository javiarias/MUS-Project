using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;

public class rollabolla : MonoBehaviour
{
    public float speed;
    public float maxForce = 100.0f;
    public float jumpForce = 5.0f;

    public GameObject camera;
    private FMODEventPlayable ev;
    public Slider slider;

    private bool jump = false;
    private bool ground = false;

    void FixedUpdate()
    {
        ///////////////////////////////////////
        //MOVEMENT
        ///////////////////////////////////////
        ///
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


        ///////////////////////////////////////
        //JUMP
        ///////////////////////////////////////

        if (jump)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }

    private void Update()
    {

        ///////////////////////////////////////
        //MODIFYING FMOD PARAMETERS
        ///////////////////////////////////////
        float magnitude = GetComponent<Rigidbody>().velocity.magnitude;

        slider.value = magnitude / maxForce;    //this is used to reflect the ball's speed on the red slider at the top of the screen

        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.SetParameter("speed", magnitude * (16 / maxForce)); //in essence, we raise the value of "speed" in proportion to the ball's current TOTAL velocity, with 16 being "speed"'s maximum value.
                                                                    //As "speed" rises, the tracks start to fade in, and from 15 to 16 all the tracks are audible.

        if (Input.GetKeyDown("space") && ground)
            jump = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        ground = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        ground = false;
    }
}