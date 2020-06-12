using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private float boardTiltMax = 15f; // Maximum angle to tilt the camera to fake the level tilting
    private Vector3 desiredPosition;
    private GameObject desiredPositionObject;

    private float rotationDamping = 10f;
    private float movementDamping = 150f;
    private float turnSpeed = 100f;

    private float turnAngle = 0.0f;
    private float initUpAngle = 0.0f;
    private float upAngle = 0.0f;

    // Use this for initialization
    void Start()
    {
        offset = transform.position;
        desiredPosition = transform.position;
        desiredPositionObject = new GameObject("cameraFollow");
        DontDestroyOnLoad(desiredPositionObject);
        desiredPositionObject.transform.position = transform.position;

        initUpAngle = Vector3.Angle(offset, player.transform.position);

        if (player == null)
        {

            Debug.LogError("Could not find object \"Player\" ! Aborting GameCamera load.");
            DestroyImmediate(gameObject);
        }
    }

    void Update()
    {
        // Rotate the camera around the ball to adjust movement when Left or Right are pressed
        turnAngle += Input.GetAxis("Turn") * turnSpeed * Time.deltaTime;

        // Rotate the camera around the ball to adjust movement when Up or Down are pressed
        upAngle += Input.GetAxis("Up") * turnSpeed * Time.deltaTime;

        if (initUpAngle + upAngle <= 20.0f) upAngle = 20.0f - initUpAngle;
        else if (initUpAngle + upAngle >= 80.0f) upAngle = 80.0f - initUpAngle;
    }

    void LateUpdate()
    {
        // find the ZX direction from the player to the camera
        var heading = transform.position - player.transform.position;
        heading.y = 0f;
        var distance = heading.magnitude;
        var direction = heading / distance;

        // Find the right vector for the diretion
        var rotationVectorRight = Vector3.Cross(direction, Vector3.up);

        // Move the camera
        desiredPositionObject.transform.position = player.transform.position + offset;

        // Rotate around the players Y axis by the turn value
        desiredPositionObject.transform.RotateAround(player.transform.position, Vector3.up, turnAngle);

        Vector3 pos = transform.position;

        Vector3 playerPos = player.transform.position;

        Vector3 look = pos - playerPos;
        look.y = playerPos.y;
        Vector3 orthoLook = Vector3.Cross(look, Vector3.down);
        orthoLook.Normalize();

        // Rotate around the players Y axis by the turn value
        desiredPositionObject.transform.RotateAround(player.transform.position, orthoLook, upAngle);

        // Ensure we're looking at the player before the roll rotation is applied
        desiredPositionObject.transform.LookAt(player.transform.position);

        // Lerp the cameras position to match the target object
        transform.position = Vector3.Slerp(transform.position, desiredPositionObject.transform.position, Time.deltaTime * movementDamping);

        // Lerp the cameras rotation to match the target object
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                              desiredPositionObject.transform.rotation,
                                              Time.deltaTime * rotationDamping);

        // Re-center the camera on the object to account for new roll rotation
        CenterCameraOnTarget();

    }

    private void CenterCameraOnTarget()
    {
        Plane plane = new Plane(transform.forward, player.transform.position);
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0.0f));
        float distance;
        plane.Raycast(ray, out distance);

        var point = ray.GetPoint(distance);
        var offset = player.transform.position - point;
        transform.position += offset;
    }
}