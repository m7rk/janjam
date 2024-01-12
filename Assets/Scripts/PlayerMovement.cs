using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float topSpeed;
    [SerializeField] [Range(0, 1)] private float velocityPower;
    [SerializeField] [Range(0, 1)] private float acceleration;
    [SerializeField] [Range(0, 1)] private float decceleration;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void FixedUpdate()
    {

        rb.AddForce(MovementForce());


    }

    //A function to calculate the amount of force needed to be applied to the player, to achieve the target speed
    //This one allows u to control the acceleration & deceleration of the movement
    private Vector2 MovementForce()
    {

        //calculating top speed of the player
        Vector2 targetSpeed = directionInput() * topSpeed;
        Vector2 speedDiffirence = targetSpeed - rb.velocity;



        //Determines if it should accelerate or decellerate on each axis
        Vector2 accelerationRate = Vector2.zero;

        if (Mathf.Abs(targetSpeed.x) > 0.01f) accelerationRate.x = acceleration * topSpeed * 1.2f;
        else accelerationRate.x = decceleration * topSpeed;

        if (Mathf.Abs(targetSpeed.y) > 0.01f) accelerationRate.y = acceleration * topSpeed * 1.2f;
        else accelerationRate.y = decceleration * topSpeed;



        //calculates the amount of force needed to achieve the target velocity at the desired acceleration/decceleration rate on each axis
        //Power is here soley to make the movement curve feel a bit more natural with an exponential curve

        Vector2 finalForce = Vector2.zero;

        finalForce.x = (Mathf.Abs(speedDiffirence.x) * accelerationRate.x * velocityPower) * Mathf.Sign(speedDiffirence.x);
        finalForce.y = (Mathf.Abs(speedDiffirence.y) * accelerationRate.y * velocityPower) * Mathf.Sign(speedDiffirence.y);



        return finalForce;
    }



//a function to detect player input for movement
    private Vector2 directionInput()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) direction += Vector2.up;
        if (Input.GetKey(KeyCode.A)) direction += Vector2.left;
        if (Input.GetKey(KeyCode.S)) direction += Vector2.down;
        if (Input.GetKey(KeyCode.D)) direction += Vector2.right;

        return direction;
    }


}
