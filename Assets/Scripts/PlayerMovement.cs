using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float topSpeed;
    [SerializeField] [Range(0, 1)] private float velocityPower;
    [SerializeField] [Range(0, 1)] private float acceleration;
    [SerializeField] [Range(0, 1)] private float decceleration;
    [SerializeField] private float turnSpeed;

    [SerializeField] private float rotationOffsetDegrees;

    [SerializeField] private ParticleSystem playerPs;
    public static ParticleSystem PlayerParticle;

    private Rigidbody2D rb;
    public bool stunned;
    private float realSpeed;
    float refSpeed;

    public float invulnTime;
    private float boostTime;

    public SpriteRenderer sprite;

    private void Awake()
    {
        PlayerParticle = playerPs;
        rb = GetComponent<Rigidbody2D>();
        realSpeed = topSpeed;
    }

    public void Update()
    {
        if (invulnTime > 0)
        {
            invulnTime -= Time.deltaTime;
        }
        else
        {
            invulnTime = 0;
        }

        if (boostTime > 0)
        {
            boostTime -= Time.deltaTime;
        }
        else
        {
            boostTime = 0;
        }

        var powerups = new List<float> { invulnTime, boostTime, GameManager.timeDelay };
        // get highest timer in powerup to decide color to show.
        float maxValue = powerups.Max();
        int maxIndex = powerups.ToList().IndexOf(maxValue);

        // drema change this if you want.
        sprite.GetComponent<SpriteRenderer>().material.SetFloat("_ColourStrength", maxValue);
        sprite.GetComponent<SpriteRenderer>().material.SetInteger("_Enabled", maxValue <= 0f ? 0 : 1);

        switch(maxIndex)
        {
            case 0:
            sprite.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColour",Color.red); break;
            case 1:
            sprite.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColour", Color.green); break;
            case 2:
            sprite.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColour", Color.blue); break;

        }




    }

    void FixedUpdate()
    {

        rb.AddForce(MovementForce());

        //rotation with movement
        if (DirectionInput() != Vector2.zero && !stunned) transform.eulerAngles = Mathf.SmoothDampAngle(transform.eulerAngles.z, (Vector2.SignedAngle(Vector2.up, DirectionInput()) + rotationOffsetDegrees), ref refSpeed, turnSpeed) * Vector3.forward;

        //Rotation when stunned and a fix for a bug
        if (stunned) transform.eulerAngles += 15f * Vector3.forward;
        else if (topSpeed == 0) topSpeed = realSpeed;


    }

    //A function to calculate the amount of force needed to be applied to the player, to achieve the target speed
    //This one allows u to control the acceleration & deceleration of the movement
    private Vector2 MovementForce()
    {

        //calculating top speed of the player
        Vector2 targetSpeed = (DirectionInput().normalized * (1 + (boostTime/8f))) * topSpeed;
        Vector2 speedDiffirence = targetSpeed - rb.velocity;



        //Determines if it should accelerate or decellerate on each axis
        Vector2 accelerationRate = Vector2.zero;

        //check that will still apply friction even if the player is stunned
        if (stunned) accelerationRate = 6.4f * Vector2.one;
        else 
        { 
             if (Mathf.Abs(targetSpeed.x) > 0.01f) accelerationRate.x = acceleration * topSpeed * 1.2f;
             else accelerationRate.x = decceleration * topSpeed;

            if (Mathf.Abs(targetSpeed.y) > 0.01f) accelerationRate.y = acceleration * topSpeed * 1.2f;
            else accelerationRate.y = decceleration * topSpeed;
         }



        //calculates the amount of force needed to achieve the target velocity at the desired acceleration/decceleration rate on each axis
        //Power is here soley to make the movement curve feel a bit more natural with an exponential curve

        Vector2 finalForce = Vector2.zero;

        finalForce.x = (Mathf.Abs(speedDiffirence.x) * accelerationRate.x * velocityPower) * Mathf.Sign(speedDiffirence.x);
        finalForce.y = (Mathf.Abs(speedDiffirence.y) * accelerationRate.y * velocityPower) * Mathf.Sign(speedDiffirence.y);



        return finalForce;
    }



//a function to detect player input for movement
    public static Vector2 DirectionInput()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) direction += Vector2.up;
        if (Input.GetKey(KeyCode.A)) direction += Vector2.left;
        if (Input.GetKey(KeyCode.S)) direction += Vector2.down;
        if (Input.GetKey(KeyCode.D)) direction += Vector2.right;

        return direction;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // powerups
        if (collision.gameObject.tag == "Boost")
        {
            boostTime = 5f;
            Destroy(collision.gameObject);
            GameCanvas.addToMessageQueue("FOUND A BOOST! + 10");
            GameManager.scoreBonus += 10;
        }
        if (collision.gameObject.tag == "Clock")
        {
            GameCanvas.addToMessageQueue("FOUND A CLOCK! + 5");
            Destroy(collision.gameObject);
            GameManager.timeDelay += 5f;
            GameManager.scoreBonus += 5;
        }
        if (collision.gameObject.tag == "Star")
        {
            GameCanvas.addToMessageQueue("FOUND A STAR! + 20");
            invulnTime += 5f;
            Destroy(collision.gameObject);
            GameManager.scoreBonus += 20;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Debris"))
        {
            // basically kill all velocity. I don't know if this acutally works.
            rb.velocity = rb.velocity * 0.1f;

            if (!GameManager.trashesHit.Contains(collision.gameObject) && collision.gameObject.name.Contains("TrashCan"))
            {
                GameManager.trashesHit.Add(collision.gameObject);
                if (GameManager.trashesHit.Count == 5)
                {
                    GameCanvas.addToMessageQueue("HIT 5 TRASH CANS! + 10");
                    GameManager.scoreStyle += 10;
                }

                if (GameManager.trashesHit.Count == 10)
                {
                    GameCanvas.addToMessageQueue("HIT 10 TRASH CANS! + 30");
                    GameManager.scoreStyle += 30;
                }

                if (GameManager.trashesHit.Count == 15)
                {
                    GameCanvas.addToMessageQueue("HIT 15 TRASH CANS! + 70");
                    GameManager.scoreStyle += 70;
                }

            }
        }
    }

    //coroutine fur stunning the player for N seconds
    public IEnumerator Stun(float StunTime)
    {
        playerPs.Play();
        //remembers the original speed of the player
        float originalSpeed = topSpeed;
        stunned = true;
        topSpeed = 0f;

        //Waits for the stun durration
        yield return new WaitForSeconds(StunTime);

        topSpeed = originalSpeed;
        stunned = false;

        yield return null;
    }

}
