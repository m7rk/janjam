using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private string intersectionLayer;
    [SerializeField] private float speed;

    private Vector3 carTarget;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the car is on an intersection, if so, turns to a random neighbouring one
        if (collision.gameObject.tag == "Intersection")
        {
            carTarget = collision.gameObject.GetComponent<IntersectionMarker>().getRandomDestination();
            carTarget = carTarget - transform.position;
            carTarget = carTarget.normalized;

            transform.eulerAngles = Vector2.SignedAngle(Vector2.up, carTarget) * Vector3.forward;

            //knocks the player back & stuns em for a while when hit
        } else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = (carTarget * 125);
            collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine("Stun", 3.5f);
        }
    }
   
    private void Update()
    {
        rb.velocity = transform.up * speed;
    }

}
