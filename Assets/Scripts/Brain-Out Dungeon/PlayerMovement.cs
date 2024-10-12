using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    float speed = 10f;
    float movementX;
    float movementY;

    // Properties 
    public float Speed 
    {
        get { return speed; }
        set { speed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Direction based on the input of the player from PlayerInput component
        Vector3 movementDirection = new Vector3(movementX, 0f, movementY);
        rb.AddForce(movementDirection * speed);
    }

    // This function is called when an input is detected by the InputAction
    public void OnMove(InputValue inputValue)
    {
        Vector2 movementVector = inputValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
}
