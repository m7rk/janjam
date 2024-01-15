using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private string intersectionLayer;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private enum IntersectionTurn { LEFT, RIGHT};

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the car is on an intersection, if so, turns to a random neighbouring one
        if (collision.gameObject.tag == "Intersection")
        {
            Vector3 carTarget;
            var newDir = -transform.up;
            while (transform.up == -newDir)
            {
                //reroll destination until we get a new direction. Don't go backwards.
                carTarget = collision.gameObject.GetComponent<IntersectionMarker>().getRandomDestination();
                newDir = (carTarget - collision.gameObject.transform.position).normalized;
            }
            // if we're going forward, don't do anything.
            // if we're going left, go left after 3 units.
            // if we're going right, go right after 6 units.
            Debug.Log(Vector2.Angle(transform.up, newDir));
            transform.up = newDir;
            

            

            //knocks the player back & stuns em for a while when hit
        } else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = rb.velocity;
            collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine("Stun", 3.5f);
        }
    }
   
    private void Update()
    {
        rb.velocity = transform.up * speed;
    }

}
