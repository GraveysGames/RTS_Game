using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput_PlayerMovement : MonoBehaviour
{

    private CharacterController charController;

    [SerializeField] private float playerMoveSpeed;

    [SerializeField] private float playerRotationSpeed;

    [SerializeField] private float playerZoomSpeed;

    [SerializeField] private float playerHeightLimit;

    [SerializeField] private float playerGroundLimit;


    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        MovePlayer();
        ZoomPlayer();
        RotatePlayer();
    }


    private void MovePlayer()
    {
        //movement direction

        //left right
        float deltaX = Input.GetAxis("Horizontal") * playerMoveSpeed;
        //forward back
        float deltaZ = Input.GetAxis("Vertical") * playerMoveSpeed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, playerMoveSpeed);


        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        charController.Move(movement);
    }

    private void RotatePlayer()
    {

        float playerRotation = 0;

        if (Input.GetKey("e"))
        {
            playerRotation += 1;
        }
        else if (Input.GetKey("q"))
        {
            playerRotation -= 1;
        }

        playerRotation *= playerRotationSpeed * Time.deltaTime;

        this.transform.Rotate(new Vector3(0, playerRotation, 0));

    }


    private void ZoomPlayer()
    {
        float deltaY = Input.mouseScrollDelta.y * playerZoomSpeed;


        Vector3 movement = new Vector3(0, -deltaY, 0);
        movement = Vector3.ClampMagnitude(movement, playerZoomSpeed);


        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        if ((transform.position.y + movement.y > playerGroundLimit) && (transform.position.y + movement.y < playerHeightLimit))
        {
            charController.Move(movement);
        }

        


    }
}
